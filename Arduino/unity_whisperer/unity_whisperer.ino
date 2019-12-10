#include <Adafruit_NeoPixel.h>
#ifdef __AVR__
  #include <avr/power.h>
#endif

// ---------------------------------------------------------------------
// GLOBAL VARIABLES AND MATH FUNCTIONS
// ---------------------------------------------------------------------

// HARDWARE VARS
int sensorPins[4] = {A0,A1,A2,A3};   // Pins in which the sensorStates are plugged

int sensorThreshold = 10; 

int stripLen = 60;                   // number of LEDs on the strips

int lianaPins[4] = {5,6,10,11};       // pins in which tle strips are plugged

Adafruit_NeoPixel pixels0(stripLen, lianaPins[0], NEO_GRB + NEO_KHZ800);
Adafruit_NeoPixel pixels1(stripLen, lianaPins[1], NEO_GRB + NEO_KHZ800);
Adafruit_NeoPixel pixels2(stripLen, lianaPins[2], NEO_GRB + NEO_KHZ800);
Adafruit_NeoPixel pixels3(stripLen, lianaPins[3], NEO_GRB + NEO_KHZ800);

Adafruit_NeoPixel pixels[4] = {pixels0,pixels1,pixels2,pixels3};

// LOOK AND FEEL VARS
float brightness = 1;

class Color {
  public:
  int r;
  int g;
  int b;

  Color(int rParam, int gParam, int bParam) {
    r = rParam * brightness; 
    g = gParam * brightness;
    b = bParam * brightness;
  }
};

int colors[4][2][3] = { // colors for each strip in RGB
  { //(2 colors by strip because gradients)
    {255 * brightness, 94  * brightness, 0   * brightness}, // BLUE
    {255 * brightness, 0   * brightness, 184 * brightness}  // YELLOW
  },{
    {248 * brightness, 0   * brightness, 255 * brightness}, // YELLOW
    {26  * brightness, 255 * brightness, 100 * brightness}  // bLUE
  },{
    {255 * brightness, 54  * brightness, 100 * brightness}, // RED
    {255 * brightness, 246 * brightness, 0   * brightness}  // CYAN
  },{
    {255 * brightness, 229 * brightness, 0   * brightness}, // GREEN
    {255 * brightness, 118 * brightness, 0   * brightness}  // PURPLE
  }
};

int failColor[3] = {255, 0, 0};
int idleColor[3] = {255, 255, 255};
int demoColor[3] = {220, 255, 0};
int actionColor[3] = {0, 255, 10};


// adafruit gamma fix
const uint8_t gamma8[] = {
    0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
    0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  1,  1,  1,  1,
    1,  1,  1,  1,  1,  1,  1,  1,  1,  2,  2,  2,  2,  2,  2,  2,
    2,  3,  3,  3,  3,  3,  3,  3,  4,  4,  4,  4,  4,  5,  5,  5,
    5,  6,  6,  6,  6,  7,  7,  7,  7,  8,  8,  8,  9,  9,  9, 10,
   10, 10, 11, 11, 11, 12, 12, 13, 13, 13, 14, 14, 15, 15, 16, 16,
   17, 17, 18, 18, 19, 19, 20, 20, 21, 21, 22, 22, 23, 24, 24, 25,
   25, 26, 27, 27, 28, 29, 29, 30, 31, 32, 32, 33, 34, 35, 35, 36,
   37, 38, 39, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 50,
   51, 52, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 66, 67, 68,
   69, 70, 72, 73, 74, 75, 77, 78, 79, 81, 82, 83, 85, 86, 87, 89,
   90, 92, 93, 95, 96, 98, 99,101,102,104,105,107,109,110,112,114,
  115,117,119,120,122,124,126,127,129,131,133,135,137,138,140,142,
  144,146,148,150,152,154,156,158,160,162,164,167,169,171,173,175,
  177,180,182,184,186,189,191,193,196,198,200,203,205,208,210,213,
  215,218,220,223,225,228,231,233,236,239,241,244,247,249,252,255 };

uint32_t  deltaTime = 0L; // minimum time between 2 loops (in ms) 16ms->60fps
// -> directly linked to the speed of the animations

// LOGIC VARS
int frame = 0;// the current update frame

char currScene = 'I';
int lianaFront[4] = {-1,-1,-1,-1};
int lianaBack[4] = {-1,-1,-1,-1};
int lianaMin[4] = {-1,-1,-1,-1};
int lianaMax[4] = {-1,-1,-1,-1};

bool sensorStates[4] = {false,false,false,false};


// -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
// linear interpolation
float lerp(float v0, float v1, float t) {
  return (1 - t) * v0 + t * v1;
}


// -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
// clamp x between a and b
int clamp(int x, int a, int b) {
  if (a > b) {
    int c = b;
    b = a;
    a = c;
  }
  
  if(x < a) return a;
  if (x > b) return b;
  return x;
}

// -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
// return true if x is between a and b
bool between(int x, int a, int b) {
  if (a > b) {
    int c = b;
    b = a;
    a = c;
  }
  
  if(x < a) return false;
  if (x > b) return false;
  return true;
}
