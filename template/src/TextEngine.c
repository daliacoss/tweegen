#include "TextEngine.h"

static Vect2D_u16 displaySubpassage(Subpassage *sub, Vect2D_u16 pos);
static void flashActiveLink();
static void testProgram();
static void cycleLink(s8 dir);

static void drawText(char text[], u8 x, u8 y);
static void drawLink(char text[], u8 x, u8 y);
static void drawLinkActive(char text[], u8 x, u8 y);

u8 marginX = 1;
u8 marginY = 3;

u8 buttonNext = BUTTON_RIGHT | BUTTON_DOWN;
u8 buttonPrev = BUTTON_LEFT | BUTTON_UP;

/* private */
u8 currentTextColour = 1;
u8 currentTextPal = PAL0;
Passage *currentPassage;
u16 currentSubpassageIndex;
//u16 currentNumLinks;

void TextEngine_init(){
	u8 i;

	//load all font tilesets
	for (i=0; i<TE_NUM_FONTS; i++){
		VDP_loadFontExt(&tweefont, i, 0);
	}

	TextEngine_displayPassage(&p_Start);
}

void TextEngine_update(){
	flashActiveLink();
}

void TextEngine_joyEvent(u16 joy, u16 changed, u16 state){
	if ((BUTTON_A | BUTTON_RIGHT) & changed & state){
		VDP_drawTextBG(
			APLAN,
			"WORLD",
			TILE_ATTR(PALETTE_LINK_ACTIVE,0,1,0),
			8,
			11
		);
	}
}

void TextEngine_displayPassage(Passage *p){
	u16 i,j;
	u16 curLine=0, curCol=0;
	Vect2D_u16 curPos;
	curPos.x = curPos.y = 0;
	Subpassage *sub;

	currentPassage = p;

	//for each subpassage
	for (i=0; i < p->count; i++){
		curPos = displaySubpassage(&(p->subs[i]), curPos);

	}
}

/*draw sub starting at curPos, return ending position */
static Vect2D_u16 displaySubpassage(Subpassage *sub, Vect2D_u16 pos){
	u16 j;

	sub->positionStart.x = pos.x;
	sub->positionStart.y = pos.y;
	// for each line in subpassage
	for (j=0; j < sub->lineCount; j++, pos.y++){
		if (sub->formatting[Link]){
			drawLink(sub->lines[j], marginX+pos.x, marginY+pos.y);
		}
		else{
			drawText(sub->lines[j], marginX+pos.x, marginY+pos.y);
		}

		//only reset col if there's another line in the passage
		if (j != sub->lineCount - 1){
			pos.x = 0;
		}
	}
	//endWithNewline is either 0 or 1; -1 because of how the loop iteration works
	pos.y += sub->endWithNewline - 1;
	pos.x = (sub->endWithNewline) ? 0 : pos.x + strlen(sub->lines[j-1]);

	return pos;
}

static void drawText(char text[], u8 x, u8 y){
	VDP_drawTextBGExt(
		APLAN,
		text,
		TILE_ATTR(currentTextPal,0,0,0),
		x,
		y,
		currentTextColour
	);
}
static void drawLink(char text[], u8 x, u8 y){
	//colour index 0xF is reserved for links
	VDP_drawTextBG(APLAN, text, TILE_ATTR(PALETTE_LINK_INACTIVE,0,0,0), x, y);
}
static void drawLinkActive(char text[], u8 x, u8 y){
	//colour index 0xF is reserved for links
	VDP_drawTextBG(APLAN, text, TILE_ATTR(PALETTE_LINK_ACTIVE,0,0,0), x, y);
}


static void cycleLink(s8 dir){
	u16 count = currentPassage->count;
	u16 index = (u16) wrap(currentSubpassageIndex + dir, 0, count);
	Subpassage *sub = &(currentPassage->subs[index]);
	//currentSubpassageIndex += dir;
	while (! sub->formatting[Link]){

		index = (u16) wrap(index+dir, 0, count);
		currentSubpassageIndex = index;
		sub = &(currentPassage->subs[index]);
	}
}

static void testProgram(){
	VDP_drawTextBGExt(APLAN, "hello", TILE_ATTR(PAL0,0,0,0), 2, 10, 0x1);
	VDP_drawTextBG(APLAN, "WORLD", TILE_ATTR(PAL1,0,0,0), 8, 10);
	VDP_drawTextBGExt(APLAN, "hello", TILE_ATTR(PAL0,0,1,0), 2, 11, 0x1);
	VDP_drawTextBGExt(APLAN, "WORLD", TILE_ATTR(PAL0,0,1,0), 8, 11, 0x2);
}

void VDP_loadFontExt(const TileSet *font, u8 colourIndex, u8 use_dma){
	u16 i, j, index;
	u32 newRow, row;
	u32 fontTiles[768];

	if (colourIndex >= TE_NUM_FONTS) return;

	for (i=0; i<768; i++){
		row = font->tiles[i];
		newRow = 0;
		//if colourIndex is 0, use 0xF
		if (!colourIndex){
			fontTiles[i] = row;
		}
		//else, transform character to use new colourIndex
		else{
			for (j=0; j<8 && row>0; j++, row /= 16){
				if (row % 16){
					//if current digit is nonzero, change to colour
					newRow += ((u32) colourIndex) << (j * 4);
				}
			}
			fontTiles[i] = newRow;			
		}
	}
	index = TILE_FONTINDEX - (FONT_LEN * colourIndex);
	VDP_loadTileData(fontTiles, index, font->numTile, 0);
}

void VDP_drawTextBGExt(u16 plan, const char *str, u16 flags, u16 x, u16 y, u8 colour){
	u32 len;
	u16 data[128];
	u16 i, index;

	if (colour >= TE_NUM_FONTS) return;
	index = TILE_FONTINDEX - (FONT_LEN * colour);

	// get the horizontal plan size (in cell)
	i = VDP_getPlanWidth();
	len = strlen(str);

	// if string don't fit in plan, we cut it
	if (len > (i - x))
		len = i - x;

	for (i = 0; i < len; i++){
		data[i] = index + (str[i] - 32);
	}
	VDP_setTileMapDataRectEx(plan, data, flags, x, y, len, 1, len);
}

static void flashActiveLink(){
	static u16 color = 0x888, timer = 0, index = 0;
	u16 inc = 0x222;
	static s8 dir = 1;

	if (color >= 0xeee && dir > 0){
		dir = -1;
	}
	else if (color <= 0x888 && dir < 0){
		dir = 1;
	}

	VDP_setPaletteColor(PALETTE_LINK_ACTIVE * 16 + COLOUR_LINK, color);

	if (timer == 3){
		color += inc * dir;
		color %= 0xf000;
		timer = 0;
		index++;
	}
	else timer++;
}

s32 wrap(s32 n, s32 min, s32 max){
	return (n<min) ? wrap(max-(n*-1), min, max) :
		(n>max) ? wrap(n-max, min, max) :
		n;
}
