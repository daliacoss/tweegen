using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq; //for xelement
using System.Text.RegularExpressions;

enum Tag {
	Link,
	Bold,
	Italic,
	Underline,
	Subscript,
	Superscript,
	Monospace,
	UnorderedList,
	OrderedList,
	Macro
}
enum Operator {
	Eq,Neq,Lt,Lte,Gt,Gte,And,Not,Or
}

struct Condition {
	public string Left;
	public Operator Op;
	public string Right;
}

class Subpassage {
	public string Text = "";
	public List<Tag> Formatting = new List<Tag>();

	//we can just use the passage title for the link address
	public string LinkAddress = "";

	//we'll use this for if/else blocks maybe?
	Subpassage Sub;

	public Subpassage(): this("", new List<Tag>(), ""){
	}

	public Subpassage(string text, List<Tag> formatting, string linkAddress){
		Text = text;
		Formatting = new List<Tag>(formatting);
		LinkAddress = linkAddress;
	}

	/* return deep copy of the subpassage */
	public Subpassage Copy(){
		Subpassage sp = new Subpassage(Text, Formatting, LinkAddress);
		if (Sub != null){
			sp.Sub = Sub.Copy();
		}
		return sp;
	}

	/* return descriptive, multi-line string */
	public string ToLongString(){
		string s = "Text: " + Text + "\nFormatting: ";
		//string tags = "";
		foreach (Tag t in Formatting){
			s += t.ToString() + " ";
		}
		s += "\nLinkAddress: " + LinkAddress;
		return s; 
	}
}

class Passage : List<Subpassage>{
	public string Title = "";

	override public string ToString(){
		return Title + " [" + Count.ToString() + " subpassages]";
	}
}

class TweeParser {
	public string Stream;
	public readonly Dictionary<string,Tag> TagMaps = new Dictionary<string,Tag>(){
		{"link", Tag.Link},
		{"strong", Tag.Bold},
		{"em", Tag.Italic},
		{"underline", Tag.Underline},
		{"subscript", Tag.Subscript},
		{"superscript", Tag.Superscript},
		{"monospace", Tag.Monospace},
		{"macro", Tag.Macro}
	};
	public readonly Dictionary<string,string[]> TagStrings = new Dictionary<string,string[]>(){
		//tags that use the same strings for close and open will be used in regexes
		//(macros are detected separately)
		{"link", new string[2]{"[[","]]"}},
		{"strong", new string[1]{@"\'\'"}},
		{"em", new string[1]{"//"}},
		{"underline", new string[1]{"__"}},
		{"subscript", new string[1]{"~~"}},
		{"superscript", new string[1]{@"\^\^"}},
		{"monospace", new string[2]{"{{{","}}}"}},
		//{"olli", new string[1]{"#"}},
		//{"ulli", new string[1]{@"\*"}},
		//we need to escape these so they don't get confused with xml
		//{"macro", new string[2]{@"<<",@">>"}},
	};
	public readonly string MacroName = "macro";
	//this time the strings are keys - we are more likely to search for "eq" than Operator.Eq
	public readonly Dictionary<string,Operator> OpStrings = new Dictionary<string,Operator>(){
		{"eq",Operator.Eq},
		{"is",Operator.Eq},
		{"neq",Operator.Neq},
		{"lt",Operator.Lt},
		{"<",Operator.Lt},
		{"lte",Operator.Lte},
		{"<=",Operator.Lte},
		{"gt",Operator.Gt},
		{">",Operator.Gt},
		{"gte",Operator.Gte},
		{">=",Operator.Gte},
		{"and",Operator.And},
		{"not",Operator.Not},
		{"or",Operator.Or},
	};
	private Passage currentPassage;

	public TweeParser(){
		currentPassage = new Passage();
	}

	public void LoadStream(string file){

	}

	public Dictionary<string,Passage> Parse(string stream){
		Dictionary<string,Passage> passageTree = new Dictionary<string,Passage>();

		//all titles begin with (newline):: and optional whitespace, and end with newline
		stream = "\n" + stream;
		MatchCollection matches = Regex.Matches(stream, @"\r*\n+::\s*\w+\r*\n+");
		char[] charsToStrip = {':',' ','\n','\r'};
		string ptitle, pbody;
		int start, end;
		for (int i=0; i<matches.Count; i++){
			ptitle = matches[i].Value.Trim(charsToStrip);

			//find the start of a passage
			start = matches[i].Index + matches[i].Value.Length;
			//find the end of the passage
			if (i<matches.Count-1){
				end = matches[i+1].Index;	
			}
			else{
				end = stream.Length;
			}
			//body exists between end of title and end of passage
			if (start<end){
				pbody = stream.Substring(start,end-start);
				//parse passage body
				passageTree[ptitle] = BodyToPassage(pbody, ptitle);
			}
		}

		return passageTree;
	}

	/* convert passage text to Passage instance */
	public Passage BodyToPassage(string body, string title){
		currentPassage.Clear();
		currentPassage.Title = title;

		XDocument tree = BodyToXml(body);

		//now we parse the xml into a passage (list of subpassages)
		foreach (XElement node in tree.Element("passage").Elements()){
			recurseElements(node, new List<Tag>());
		}

		return currentPassage;
	}

	/* convert passage text to XML document (XDocument) */
	public XDocument BodyToXml(string body){
		//check for macros first so they don't get confused with xml
		body = body.Replace("<<", "<"+MacroName+">");
		body = body.Replace(">>", "</"+MacroName+">");

		//now do the other formatting tags
		//regex stolen from Milan Negovan's MarkDownSharp:
		string exp = "({0})" + @" (?=\S) (.+?[*_]*) (?<=\S) \1";
		foreach (var ts in TagStrings){
			var s = ts.Value;
			//add all of the tag strings to the regex
			if (s.Length == 1){
				Regex bold = new Regex(String.Format(exp, s[0]),
					RegexOptions.IgnorePatternWhitespace | 
					RegexOptions.Singleline | 
					RegexOptions.Compiled);
				body = bold.Replace(body, "<"+ts.Key+">$2"+"</"+ts.Key+">");
			}
			else if (s.Length == 2){
				body = body.Replace(s[0],"<"+ts.Key+">");
				body = body.Replace(s[1],"</"+ts.Key+">");
			}
		}

		//format xml to fit spec
		body = "<passage>" + body + "</passage>";
		XDocument tree = XDocument.Parse(body);
		applyText(tree);

		return tree;
	}

	/*wrap all free text in <text></text>*/
	protected void applyText(XContainer tree){
		var nodes = tree.Nodes();
		foreach (XNode enode in nodes){
			if (enode is XText){
				string val = ((XText) enode).Value;
				enode.ReplaceWith(new XElement("text", val));
			}
			else if (enode is XContainer){
				applyText((XContainer)enode);
			}

		}
	}

	/* recursively turn XElement into subpassages and add them to currentPassage */
	protected void recurseElements(XElement tree, List<Tag> formatting){
		List<Tag> fcopy = new List<Tag>(formatting);
		if (tree.Name == "text"){
			Subpassage sp = new Subpassage();
			sp.Text = tree.Value;
			sp.Formatting = fcopy;
			currentPassage.Add(sp);
		}
		else{		
			fcopy.Add(TagMaps[tree.Name.ToString()]);
			foreach (XElement el in tree.Elements()){
				recurseElements(el,fcopy);
			}
		}	
	}
}

class TweeGen {
	static void Main(){
		TweeParser tp = new TweeParser();
		var tree = tp.Parse("\n::Start\r\nthis text is [[''bold //and italic __and underlined__//'']]\n");

		foreach(KeyValuePair<string,Passage> kv in tree){
			Console.WriteLine(kv.Value.ToString());
			foreach(Subpassage sp in kv.Value){
				Console.WriteLine(sp.ToLongString());
			}
		}
	}
}