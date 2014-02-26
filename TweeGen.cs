using System;
using System.Collections.Generic;
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
		public /*List<Subpassage>*/string LinkAddress;

		//we'll use this for nested macros maybe?
		Subpassage Sub;
	}

	public string Stream;
	//maybe i'll change the string arrays to regexes, and use them as the keys?
	public readonly Dictionary<Tag,string[]> TagStrings = new Dictionary<Tag,string[]>(){
		{Tag.Link, new string[3]{@"\[\[",@"\|",@"\]\]"}},
		{Tag.Bold, new string[2]{"''","''"}},
		{Tag.Italic, new string[2]{"//","//"}},
		{Tag.Underline, new string[2]{"__","__"}},
		{Tag.Subscript, new string[2]{"~~","~~"}},
		{Tag.Superscript, new string[2]{@"\^\^",@"\^\^"}},
		{Tag.Monospace, new string[2]{"{{{","}}}"}},
		{Tag.OrderedList, new string[1]{"#"}},
		{Tag.UnorderedList, new string[1]{@"\*"}},
		{Tag.Macro, new string[2]{"<<",">>"}},
	};
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
		MatchCollection matches = null;

		//parse all the escape characters...
		string expBase = "[{0}].*[{1}]", expLeft = "", expRight = "";

		//regex stolen from Milan Negovan's MarkDownSharp:
		Regex bold = new Regex(@"(\'\') (?=\S) (.+?[*_]*) (?<=\S) \1",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.Compiled);
		Console.WriteLine(bold.Replace(body, "<strong>$2</strong>"));

		foreach (var ts in TagStrings){
			var s = ts.Value;
			//add all of the tag strings to the regex
			if (s.Length > 1) expLeft += "("+s[0]+")";
			if (s.Length == 2) expRight = "("+s[1]+")" + expRight;
			else if (s.Length == 3) expRight += s[2];

		}
		//Console.WriteLine(String.Format(expBase, expLeft, expRight));
		//matches = Regex.Matches(body, String.Format(expBase, expLeft, expRight));
		//parseMatch(matches);
		//recursiveMatch(body, String.Format(expBase, expLeft, expRight));

		sp.Text = body;
		subpassages.Add(sp);
		return subpassages;
	}

	protected void recursiveMatch(string s, string exp){
		MatchCollection matches = Regex.Matches(s, exp);
		if (matches.Count > 0){
			foreach (Match m in matches){
				Console.WriteLine(m.Value + " : " + m.Index);
				//recursiveMatch(m.Value, exp);
			}
		}
		else Console.WriteLine("ohno");
	}

	protected void parseMatch(MatchCollection matches){
		if (matches!=null) foreach (Match m in matches){
			Console.WriteLine(m.Value + ": " + m.Index);
		}
	}
}

class TweeGen {
	static void Main(){
		TweeParser tp = new TweeParser();
		//:: End\nyes
		var tree = tp.Parse("\n::Start\r\n''bl<<ah [[link|passage]]>>'' __etc__\n");
		foreach (var i in tree){
			Console.WriteLine(i.Value[0].Text);
		}
		
		/*MatchCollection mc = Regex.Matches(@"<<ah >>[[link|passage]]", @"[(\[\[)(<<)].*[(\]\])(>>)]");
		foreach (var m in mc){
			//MatchCollection
			Console.WriteLine(m);
		}*/
	}
}