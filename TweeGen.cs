using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
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

class TweeParser {
	public struct Condition {
		string Var;
		Operator Op;
		string Value;
	}
	public class Subpassage {
		public string Text;
		public Dictionary<Tag,bool> Tags;
		//we can just use the passage title for the link address
		public string LinkAddress;

		//we'll use this for nested macros maybe?
		Subpassage Sub;
	}

	public string Stream;
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

	public void LoadStream(string file){

	}

	public Dictionary<string,List<Subpassage>> Parse(string stream){
		Dictionary<string,List<Subpassage>> passageTree = new Dictionary<string,List<Subpassage>>();

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
				passageTree[ptitle] = BodyToSubpassages(pbody);
			}
		}

		return passageTree;
	}

	public List<Subpassage> BodyToSubpassages(string body){
		List<Subpassage> subpassages = new List<Subpassage>();
		Subpassage sp = new Subpassage();

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
		//wrap all free text in <text></text>
		XDocument tree = XDocument.Parse(body);
		foreach (XElement node in tree.Elements()){
			foreach (var enode in node.DescendantNodes()){
				if (enode.GetType() == typeof(XText)){
					XText tmp = (XText) enode;
					enode.ReplaceWith(new XElement("text", tmp.Value));
					//Console.WriteLine(enode + " : " + enode.GetType());
				}
			}
		}

		//now we parse the xml into a passage (list of subpassages)
		foreach (XElement node in tree.Element("passage").Elements()){
			Console.WriteLine(node);
		}

		sp.Text = body;
		subpassages.Add(sp);
		return subpassages;
	}
}

class TweeGen {
	static void Main(){
		TweeParser tp = new TweeParser();
		//:: End
		var tree = tp.Parse("\n::Start\r\nthis text is ''bold''\n");
		foreach (var i in tree){
			//Console.WriteLine(i.Value[0].Text);
		}
	}
}