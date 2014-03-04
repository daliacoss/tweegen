using System;
using System.Collections.Generic;
using Cosstropolis.Twee;
using Mono.Options;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq; //for xelement

class TweeGen {

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
		Dictionary<string,Passage> tree;
		tp.LoadStream(extra[0]);
		tree = tp.Parse();

		foreach(KeyValuePair<string,Passage> kv in tree){
			Console.WriteLine(kv.Value.ToLongString());
		}	
	}
}
