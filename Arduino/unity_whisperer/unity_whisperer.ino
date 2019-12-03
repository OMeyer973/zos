#include <Adafruit_NeoPixel.h>
#ifdef __AVR__
  #include <avr/power.h>
#endif

// ---------------------------------------------------------------------
// GLOBAL VARIABLES
// ---------------------------------------------------------------------

// HARDWARE VARS
int sensorPins[4] = {A0,A1,A2,A3};   // Pins in which the sensorStates are plugged

int stripLen = 40;                   // number of LEDs on the strips

int lianaPins[4] = {5,6,10,11};       // pins in which tle strips are plugged

Adafruit_NeoPixel pixels0(stripLen, lianaPins[0], NEO_GRB + NEO_KHZ800);
Adafruit_NeoPixel pixels1(stripLen, lianaPins[1], NEO_GRB + NEO_KHZ800);
Adafruit_NeoPixel pixels2(stripLen, lianaPins[2], NEO_GRB + NEO_KHZ800);
Adafruit_NeoPixel pixels3(stripLen, lianaPins[3], NEO_GRB + NEO_KHZ800);

Adafruit_NeoPixel pixels[4] = {pixels0,pixels1,pixels2,pixels3};

// LOOK AND FEEL VARS
float brightness = .1;
int colors[4][2][3] = { // colors for each strip in RGB
  { //(2 colors by strip because gradients)
    {255 * brightness, 0   * brightness, 0   * brightness}, // RED
    {0   * brightness, 255 * brightness, 255 * brightness}  // CYAN
  },{
    {0   * brightness, 255 * brightness, 0   * brightness}, // GREEN
    {255 * brightness, 0   * brightness, 255 * brightness}  // PURPLE
  },{
    {0   * brightness, 0   * brightness, 255 * brightness}, // BLUE
    {255 * brightness, 255 * brightness, 0   * brightness}  // YELLOW
  },{
    {255 * brightness, 255 * brightness, 0   * brightness}, // YELLOW
    {0   * brightness, 0   * brightness, 255 * brightness}  // bLUE
  }
};

uint32_t deltaTime = 16L; // minimum time between 2 loops (in ms) 16ms->60fps
// -> directly linked to the speed of the animations

// LOGIC VARS
char currScene = 'I';
int lianaFront[4] = {-1,-1,-1,-1};
int lianaBack[4] = {-1,-1,-1,-1};

bool sensorStates[4] = {false,false,false,false};
