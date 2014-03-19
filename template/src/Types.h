#include <genesis.h>

#define true TRUE
#define false FALSE

#define PASS_NUM_TAGS 10
#define PASS_SUB_NUM_LINES 20

typedef enum{
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
} Tag;

typedef struct _nodeString{
	char *data;
	struct _nodeString *next;
} NodeString;

typedef struct {
	u16 length;
	NodeString *head;
	NodeString *tail;
} ListString;

/* Subpassage
 * lineCount: number of lines (0-20)
 * endWithNewLine: set whether subpassage ends with newline (0/1)
 * formatting[]: formatting flags (0/1) - indices correspond to Tag values
 * linkAddress: pointer to another passage
 * lines: array of of all lines in the subpassage
 */
typedef struct _subpassage{
	u16 lineCount;
	u8 endWithNewline;
	u8 formatting[PASS_NUM_TAGS];
	struct _passage *linkAddress;
	char *lines[PASS_SUB_NUM_LINES];
	u8 active;
	Vect2D_u16 positionStart;
} Subpassage;

/* Passage
 * length: number of subpassages
 * subs: array of subpassages
 */
typedef struct _passage{
	u16 count;
	Subpassage subs[];
} Passage;
