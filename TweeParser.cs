using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq; //for xelement
using System.Text.RegularExpressions;

namespace Cosstropolis.Twee {
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
		return ToLongString("");
	}
	public string ToLongString(string indent){
		string s = indent + "Text: " + Text + "\n";
		s += indent + "Formatting: ";
		foreach (Tag t in Formatting){
			s += t.ToString() + " ";
		}
		s += "\n" + indent + "LinkAddress: " + LinkAddress + "\n";
		return s; 
	}
}

class Passage : List<Subpassage>{
	public string Title = "";

	public Passage(string title){
		Title = title;
	}

	public Passage Copy(){
		Passage p = new Passage(Title);
		foreach (Subpassage sub in this){
			p.Add(sub.Copy());
		}

		return p;
	}

	override public string ToString(){
		return Title + " [" + Count.ToString() + " subpassages]";
	}

	public string ToLongString(){
		string s = ToString() + "\n";
		foreach (Subpassage sub in this){
			s += sub.ToLongString("    ");
		}

		return s;
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
	public readonly string LinkAttributeName = "address";
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
		currentPassage = new Passage("");
	}

	public void LoadStream(string filename){
		Stream = File.ReadAllText(filename);
	}

	/* convert the loaded stream into a dict of Passages */
	public Dictionary<string,Passage> Parse(){
		return Parse(Stream);
	}
	/* convert .tw stream into a dictionary of Passages */
	public Dictionary<string,Passage> Parse(string stream){
		Dictionary<string,Passage> passageTree = new Dictionary<string,Passage>();

		//after (newline):: and optional whitespace, title must begin with word character,
		//may contain anything other than (newline), and must end with (newline)
		stream = "\n" + stream;
		MatchCollection matches = Regex.Matches(stream, @"\r*\n+::\s*\w[^\r\n]*\r*\n");
		//MatchCollection matches = Regex.Matches(stream, @"\r*\n+::\s*\w+\r*\n+");
		char[] charsToStrip = {':','\n','\r'};
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
			recurseElements(node, new List<Tag>(), "");
		}

		return currentPassage.Copy();
	}

	/* convert passage text to XML document (XDocument) */
	public XDocument BodyToXml(string body){
		//replace pointy brackets with entity names
		body = body.Replace("<", "&lt;");
		body = body.Replace(">", "&gt;");
		//replace macro tags
		body = body.Replace("&lt;&lt;", "<"+MacroName+">");
		body = body.Replace("&gt;&gt;", "</"+MacroName+">");

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
		normalize(tree);

		return tree;
	}

	/*wrap all free text in <text></text>*/
	protected void normalize(XContainer tree){
		var nodes = tree.Nodes();
		foreach (XNode enode in nodes){
			if (enode is XText){
				string val = ((XText) enode).Value;
				enode.ReplaceWith(new XElement("text", val));
			}
			else if (enode is XContainer){
				if (enode is XElement && ((XElement) enode).Name == "link"){
					var el = (XElement) enode;
					int seploc = el.Value.IndexOf('|');
					string address;
					//if the vertical bar exists, right side is address
					if (seploc >= 0){
						address = el.Value.Substring(seploc+1);
						el.Value = el.Value.Substring(0, seploc);
					}
					else {
						address = el.Value;
					}
					el.SetAttributeValue(LinkAttributeName, address);
				}
				normalize((XContainer)enode);
			}

		}
	}

	/* recursively turn XElement into subpassages and add them to currentPassage */
	protected void recurseElements(XElement tree, List<Tag> formatting, string address){
		List<Tag> fcopy = new List<Tag>(formatting);
		if (tree.Name == "text"){
			Subpassage sp = new Subpassage();
			sp.Text = tree.Value;
			sp.Formatting = fcopy;
			sp.LinkAddress = address;
			currentPassage.Add(sp);
		}
		else{		
			fcopy.Add(TagMaps[tree.Name.ToString()]);
			if (tree.Name == "link"){
				//address = "nuthin";
				//if (tree.Attribute("address") != null) address = tree.Attribute("address").Value;
				//else address = "nuthin";// tree.Attribute(LinkAttributeName).Value;
			}
			foreach (XElement el in tree.Elements()){
				recurseElements(el,fcopy,address);
			}
		}	
	}
}
}
