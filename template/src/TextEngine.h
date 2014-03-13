#include <genesis.h>
#include "resources.h"
#include "Palettes.h"
#include "Passages.h"

//note: FONT_INDEX is at the end of the tile block - we need to move backwards
#define TE_NUM_FONTS 5

//must be used within joyEvent
#define getButtonDown(b) (b) & changed & state

extern u8 marginX;
extern u8 marginY;
extern u8 buttonNext;
extern u8 buttonPrev;

void TextEngine_init();
void TextEngine_update();
void TextEngine_joyEvent(u16 joy, u16 changed, u16 state);
void TextEngine_displayPassage(Passage *p);
void VDP_loadFontExt(const TileSet *font, u8 colourIndex, u8 use_dma);
void VDP_drawTextBGExt(u16 plan, const char *str, u16 flags, u16 x, u16 y, u8 color);

s32 wrap(s32 n, s32 min, s32 max);
