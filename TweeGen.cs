using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cosstropolis.Twee;
using Mono.Options;

class TweeGen {
	/* width in chars of the screen */
	static public int ScreenWidth = 40;
	/* screen margins */
	static public int ScreenMarginX = 1;

	/* generate string for C struct instance */
	static public string GeneratePassageStruct(Passage p){
		string stream = "";
		int startColumn = 0;

		/*
		Passage p_MyTitle = {
			4, {

		*/
		string head = "Passage p_" + p.Title.Replace(' ','_') + " = {\n\t" + p.Count + ", {\n";

		/*
			}
		}
		*/
		string foot = "\n\t}\n};\n";

		foreach (Subpassage sub in p){
			//get list of lines in subpassage
			var text = escapeChars(sub.Text);
			var textAfterInsert = insertBreaks(text,ScreenWidth-(ScreenMarginX*2),startColumn,true);
			string[] lines = textAfterInsert.Key.Split('\n');
			startColumn = textAfterInsert.Value;

			//lines = (from l in lines select l.TrimStart(' ')).ToArray();

			stream += "\t\t{\n";

			/*
				4, FALSE, {0}, NULL
			*/
			string tagArray = "{" + String.Concat(
				from i in sub.FormattingAsBoolArray select i.ToString().ToLower() + ","
			) + "}";
			string address = sub.LinkAddress;
			stream += String.Format("\t\t\t{0}, {1}, {2}, {3},\n",
				lines.Length,
				lines[lines.Length-1].EndsWith("\n").ToString().ToLower(),
				tagArray,
				//use NULL for empty address
				(address != "") ? "&p_" + address.Replace(' ','_') : "NULL"
			);

			/*
				{"line","line2"}
			*/
			stream += "\t\t\t{";
			foreach (string line in lines){
				stream += "\"" + line + "\", ";
			}
			stream += "}\n\t\t},\n";
		}

		return head + stream + foot;
	}

	/* naive newline insertion - does not preserve words 
	 * returns new string and last column position
	*/
	static protected KeyValuePair<string, int> insertBreaks(string s, int width, int start, bool removeWindowsNl){
		string copy = "";
		int i = start;
		foreach (char c in s){
			//copy char and remove windows newline if necessary
			if (!(removeWindowsNl && c == '\r')){
				copy += c;
			}
			//reset i if newline is found
			else if (c == '\n'){
				i = 0;
			}
			//break to new line when we have reached the width
			else if (i > 0 && i % width == 0){
				copy += '\n';
				i++;
			}
			//i++;
		}
		return new KeyValuePair<string, int>(copy, i);
	}

	static protected string escapeChars(string s){
		string copy = "";
		int i = 0;
		foreach (char c in s){
			if (c == '"' || c == '\\'){
				copy += '\\';
			}
			copy += c;
			i++;
		}
		return copy;
	}

	static void Main(string[] args){
		var o = new OptionSet(){
			//{"t|test=", "test option", v => test = v}
		};
		List<string> extra;
		try {
			extra = o.Parse(args);
		}
		catch (OptionException e) {
			Console.WriteLine("woah");
			return;
		}

		TweeParser tp = new TweeParser();
		tp.LoadStream(extra[0]);
		PassageCollection tree = tp.Parse();

		using (System.IO.StreamWriter cfile = new System.IO.StreamWriter(@"template\src\Passages.c"))
		using (System.IO.StreamWriter hfile = new System.IO.StreamWriter(@"template\src\Passages.h")){
			cfile.WriteLine("#include \"Passages.h\"\n");
			hfile.WriteLine("#include \"Types.h\"");
			foreach(KeyValuePair<string,Passage> kv in tree){
				cfile.Write(TweeGen.GeneratePassageStruct(kv.Value));
				//cfile.Write(kv.Value.ToLongString());
				hfile.WriteLine("extern Passage p_" + kv.Value.Title.Replace(' ','_') + ";");
			}
		}

	}
}
