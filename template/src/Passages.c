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
			1
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

Passage p_drown = {
	4, {
		{
			4,
			FALSE,
			{0},
			NULL,
			{"He dreamed that a dragon","dreadful to behold","Came driving over the deep","to "},
		},
		{
			1,
			FALSE,
			{1,0,0,0,0,0,0,0,0,0},
			&p_Start,
			{"drown"},
			1
		},
		{
			1,
			FALSE,
			{1,0,0,0,0,0,0,0,0,0},
			&p_Start,
			{" his "},
		},
		{
			1,
			FALSE,
			{1,0,0,0,0,0,0,0,0,0},
			&p_Start,
			{"people"},
		},
	}	
};

void Passages_init(){
	//p_Start.
	//char *lines[] = {"a", "b"};
	//createListString((char **){"this is a line", "this is a new line"}, 2);
}
