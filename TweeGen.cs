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
		//tree = tp.Parse(":: StoryTitle\nArthur's Dream of the Dragon and the Bear\n:: completely equipped cabin\nKing Arthur, on a huge vessel with a host of knights,\nWas enclosed in __a__\n");
		tree = tp.Parse();

		foreach(KeyValuePair<string,Passage> kv in tree){
			Console.WriteLine(kv.Value.ToLongString());
		}

	}
}
