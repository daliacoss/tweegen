#include <genesis.h>
#include "TextEngine.h"

static void init();
static void update();
static void joyEvent(u16 joy, u16 changed, u16 state);
static u16 colourOctal(u8,u8,u8);

static void init(){
	JOY_init();
	JOY_setEventHandler(&joyEvent);

	Palettes_init();
	TextEngine_init();
	VDP_waitVSync();
}

static void update(){
	TextEngine_update();
	VDP_waitVSync();
}

static void joyEvent(u16 joy, u16 changed, u16 state){
	TextEngine_joyEvent(joy, changed, state);
}

/* convert octal rgb values (0-7) to even integer) */
static u16 colourOctal(u8 r, u8 g, u8 b){
	return (r*2) + ((g*2)<<4) + ((b*2)<<8);
}

/* return amplitude in triangle wave
x: position (0-1024)
max: max amplitude */
static u16 tri_u16(u16 x, u16 max){
	//u16 half = 512;

}

int main(){
	init();
	while (1) update();
	return 0;
}
