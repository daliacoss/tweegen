#include <genesis.h>
#include "resources.h"
#include "Palettes.h"
#include "Passages.h"

//note: FONT_INDEX is at the end of the tile block - we need to move backwards
#define TE_NUM_FONTS 5

//must be used within joyEvent
#define getButtonDown(b) ((b) & changed & state)

extern u8 marginX;
extern u8 marginY;
extern u8 buttonNext;
extern u8 buttonPrev;

/* initialize TextEngine */
void TextEngine_init();

/* update callback for TextEngine */
void TextEngine_update();

/* input callback for TextEngine */
void TextEngine_joyEvent(u16 joy, u16 changed, u16 state);

/* display a passage on the screen
 * (recommended to call TextEngine_clearPassage() first)
 * Passage *p: address of passage to be displayed
 */
void TextEngine_displayPassage(Passage *p);

/* clear currently displayed passage from the screen 
 * (later, we will want to pass an index, so we can display multiple passages
 */
void TextEngine_clearPassage();

/* extension for VDP_loadFont
 * u8 colourIndex: palette entry to use for all non-zero pixels
 */
void VDP_loadFontExt(const TileSet *font, u8 colourIndex, u8 use_dma);

/* extension for VDP_drawTextBG
 * u8 colour: colour of font tileset copy to search for
 */
void VDP_drawTextBGExt(u16 plan, const char *str, u16 flags, u16 x, u16 y, u8 colour);

s32 wrap(s32 n, s32 min, s32 max);
