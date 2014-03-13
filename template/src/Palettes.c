#include "Palettes.h"

/* define your starting palettes here */
u16 pals[4][16] = {
	{0x60c,0xfff,0x44E,0x008,0x08C,0x000,0x000,0x000,
	0x000,0x000,0x000,0x000,0x000,0x000,0x000,0xfaa},

	{0x000,0x000,0x44E,0x008,0x08C,0x000,0x000,0x000,
	0x000,0x000,0x000,0x000,0x000,0x000,0x000,0xddd},

	{0x000,0x000,0x44E,0x008,0x08C,0x000,0x000,0x000,
	0x000,0x000,0x000,0x000,0x000,0x000,0x000,0xddd},

	{0x000,0x000,0x44E,0x008,0x08C,0x000,0x000,0x000,
	0x000,0x000,0x000,0x000,0x000,0x000,0x000,0xddd}
};

void Palettes_init(){
	VDP_setPalette(PAL0, pals[0]);
	VDP_setPalette(PAL1, pals[1]);
	VDP_setPalette(PAL2, pals[2]);
	VDP_setPalette(PAL3, pals[3]);
}
