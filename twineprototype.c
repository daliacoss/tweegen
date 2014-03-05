#include <stdio.h>

typedef struct _hyperlink{
	unsigned short int startc;
	unsigned short int endc;
	struct _passage *passage;
} Hyperlink;

typedef struct _hListNode{
	struct _hListNode *prev;
	struct _hListNode *next;
	Hyperlink link;
} HListNode;

typedef struct{
	HListNode *head;
	HListNode *tail;
	int size;
} HList;

typedef struct _passage{
	char text[2100];
	HList links;
} Passage;

HList createHList(){}

int main(){
	printf("hello world\n");
	return 0;
}
