#include "Passages.h"

Passage p_Start = {
	4, {
		{
			3,
			0,
			{0},
			NULL,
			{"this is one line of a subpassage","","this is another line of a subpassage"},
		},
		{
			3,
			0,
			{1,0,0,0,0,0,0,0,0,0},
			&p_Start,
			{".","","here is a link, "},
		},
		{
			1,
			0,
			{0},
			NULL,
			{"and another "},
		},
		{
			1,
			0,
			{1,0,0,0,0,0,0,0,0,0},
			&p_Start,
			{"hyperlink"},
		}
	}	
};

void Passages_init(){
	//p_Start.
	//char *lines[] = {"a", "b"};
	//createListString((char **){"this is a line", "this is a new line"}, 2);
}
