#include <Adafruit_NeoPixel.h>
#ifdef __AVR__
  #include <avr/power.h>
#endif

// ---------------------------------------------------------------------
// GLOBAL VARIABLES
// ---------------------------------------------------------------------

// COMMUNICATION VARS
String inputString = "";              // a string to hold incoming data
bool inputStringComplete = false;     // whether the string is complete

// HARDWARE VARS
int sensorPins[4] = {A0,A1,A2,A3};   // Pins in which the sensors are plugged

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

bool A_lianaFilling[4]; // specific at Action scene

bool sensors[4] = {false,false,false,false};

// ---------------------------------------------------------------------
// SETUP & LOOP
// ---------------------------------------------------------------------

void setup() {
  Serial.begin(9600);

  for (int i=0; i<4; i++) {
    pixels[i].begin();    
  }
  initScene(currScene);
}

// -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
void loop() {
  uint32_t loopStartTime = millis();  
  
  String serialMsg = getSerialMessage();
  changeSceneOnCommand(serialMsg);
  
  switch (currScene) {
    case 'I': // idle scene
      GetSensorInput();
      I_updateLianas();
      // todo
      // I_sendSensorInput(); 
      break;
    case 'D': // demo scene
      D_beginLianaAnimationOnCommand(serialMsg);
      D_updateLianas();
      break;
    case 'A': // action scene
      GetSensorInput();
      A_updateLianas();
      break;
    default: break;
  }
  drawLianas();
  while((millis()-loopStartTime) < deltaTime){
    //wait
  }
}

// -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
// read a sring command (from the serial port)
// and change scene if the command says so
void changeSceneOnCommand(String str) {
  if(getCommand(str) == 'S') {
    currScene = getScene(str);
    Serial.print("changing scene for ");
    Serial.println(currScene);
    // todo : transition part ?
    // todo : init next scene
    initScene(currScene);
  }
}

// -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
// initialise global values for a given scene
void initScene(char scene) {
  for (int i=0; i<4; i++) {
    pixels[i].clear();    
  }
  switch(scene) {
    case 'I':
      I_init();
      break;
    case 'A':
      // todo
      A_init(); 
      break;
    case 'D':
      D_init();
      break;
    default:
      break;
  }
}

// -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
// get current value from the sensors and update 
// the boolean values we use in our program
void GetSensorInput() {
  //Serial.print("sensor values [");
  int sensorValue =0;
  for (int i=0; i<4; i++) {
    sensorValue = analogRead(sensorPins[i]);
    sensors[i] = (sensorValue < 128);

    //Serial.print(", ");
    //Serial.print(sensors[i]);    
  }
  //Serial.println("]");
}

// -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
// linana display is controlled by 2 pixels per LED strip : 
// 'front' pixel lights up every led on it's way
// 'back' pixel shuts down every led on it's way
// so we can easily create chase patterns
void drawLianas() {
  for (int i=0; i<4; i++) {
    int id = lianaFront[i]; // tmp id of the pixel that will be assigned
    pixels[i].setPixelColor(id, pixels[i].Color(getLEDR(i, id), getLEDG(i, id), getLEDB(i, id)));
    pixels[i].setPixelColor(lianaBack[i], pixels[i].Color(0, 0, 0));
    pixels[i].show();
  }
}

// -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
// returns the R component for the color of a given LED in a given Liana
int getLEDR(int liana, int id) {
  return getLEDComponent(liana, id, 0);
}
// -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
// returns the G component for the color of a given LED in a given Liana
int getLEDG(int liana, int id) {
  return getLEDComponent(liana, id, 1);
}
// -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
// returns the B component for the color of a given LED in a given Liana
int getLEDB(int liana, int id) {
  return getLEDComponent(liana, id, 2);
}

// -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
// returns the R, G or B component for the color of a given LED in a given Liana
int getLEDComponent(int liana, int id, int component) {
  return (int)lerp(
    colors[liana][0][component],
    colors[liana][1][component],
    (float)id / (float)stripLen
  );  
}

// -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
// linear interpolation
float lerp(float v0, float v1, float t) {
  return (1 - t) * v0 + t * v1;
}

// ---------------------------------------------------------------------
// DEMO SCENE 
// ---------------------------------------------------------------------

// initialise global values for demo scene
void D_init() {
  for(int i=0; i<4; i++) {
    lianaFront[i] = -1;
    lianaBack[i] = -1;
    sensors[i] = 0; // optional
  }
}

// -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
// start an animation on a liana for the Demo scene
// has no effect if an animation is allready running
// on the given liana
void D_beginLianaAnimationOnCommand(String str) {
  //Serial.print("begin liana ");
  //Serial.println(str);
  if(getCommand(str) == 'L') {
    int liana = getLiana(str);
    if (0 <= liana && liana < 4) {
      lianaFront[liana] ++; 
      // moving the lianaFront from -1 to 0 triggers 
      // the animation cycle in updateLianas_D
    }
  }
}

// -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
// goes one step forward in the liana animation
// scene D : chase
void D_updateLianas() {
  for (int i=0; i<4; i++) {
    if (lianaFront[i] < stripLen && lianaFront[i] >= 0) {
      lianaFront[i] ++;
    }
    if (lianaFront[i] >= stripLen && lianaBack[i] <= stripLen) {
      lianaBack[i] ++;
    }
    if (lianaFront[i] >= stripLen && lianaBack[i] >= stripLen) {
      lianaBack[i] = -1;        
      lianaFront[i] = -1;        
    }
  }
}

// ---------------------------------------------------------------------
// IDLE SCENE
// ---------------------------------------------------------------------

// initialise global values for idle scene
void I_init() {
  for(int i=0; i<4; i++) {
    lianaFront[i] = stripLen;
    lianaBack[i] = stripLen;
    sensors[i] = 0; // optional
  }
}

// -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
// goes one step forward in the liana animation
// scene I : small gauge based on interaction
// /!\ gauge is at end of LED strip !
void I_updateLianas() {
  static int actionStripLen = 20;
  for (int i=0; i<4; i++) {
    if (sensors[i]) {
      if (lianaFront[i] > stripLen - actionStripLen) {
        lianaFront[i] --;
      }
    } else {
      if (lianaFront[i] < stripLen) {
        lianaFront[i] ++; 
      }
    }
    lianaBack[i] = lianaFront[i] - 1;
  }
}
// ---------------------------------------------------------------------
// ACTION SCENE
// ---------------------------------------------------------------------

// initialise global values for action scene
void A_init() {
  for(int i=0; i<4; i++) {
    A_initLiana(i);
  }
}

// -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
// initialise global values for liana i in Action scene
void A_initLiana(int i) {
  lianaFront[i] = stripLen;
  lianaBack[i] = stripLen;
  A_lianaFilling[i] = true;
  sensors[i] = 0; // optional
}

// -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
// goes one step forward in the liana animation
// scene I : small gauge based on interaction
// /!\ gauge is at end of LED strip !

// 1st part : filling '-' led OFF, 'o' LED on
//   0123... (LED ids)
//   ----------------------bf -> when sensor off
//   -----------bfooooooooooo <- when sensor on

// 2nd part : erasing
// bfoooooooooooooooooooooo when at the end, LianaBack goes to 
// foooooooooooooob-------- <- other end of strip and erase all

void A_updateLianas() {
  static int actionStripLen = 20;

  for (int i=0; i<4; i++) {
    if (A_lianaFilling[i]) {
      A_fillLiana(i);
    } else {
      A_eraseLiana(i);
    }
  }
}

// -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
// 1st part of the liana update
void A_fillLiana(int i) {
  //lianaFront moves forward when sensor is activated (1st part)
  if (sensors[i]) {
    if (lianaFront[i] > 0) {
      lianaFront[i] --;

      // is true for one frame when the liana has been filled entirely
      if (lianaFront[i] <= 0) {
        // todo : send message to unity
        A_lianaFilling[i] = false;
        lianaBack[i] = stripLen;
        return;
      }
    }
  } else {
    if (lianaFront[i] < stripLen) {
      lianaFront[i] ++; 
    }
  }
  lianaBack[i] = lianaFront[i] - 1;
}

// -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
// 2nd part of the liana update
void A_eraseLiana(int i) {
  lianaBack[i]--;
  if (lianaBack[i] < 0){
    A_initLiana(i);
  }
}

// ---------------------------------------------------------------------
// COMUNICATION AND PARSING
// ---------------------------------------------------------------------

// returns a string message from the serial.
// if no message has came at  that frame, returns ""
String getSerialMessage() {
  String out = "";
  if(inputStringComplete) {
    out = inputString;
    inputStringComplete = false;
    inputString = "";
  }
  if (out != "") {
    Serial.println("command received : " + out);
  }
  return out;
}


// -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
// returns the command character from this command string
char getCommand(String str) {
  if(
    str.length() > 0 &&
    (str[0] == 'S' || str[0] == 'L')
  ){
    return str[0];
  }
  return '>'; // needs to be checked or discarded by calling method !
}

// -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
// returns the scene character from this command string
char getScene(String str) {
  if(
    str.length() > 1 && 
    (str[1]== 'I' || str[1]== 'A' || str[1]== 'D')
  ){
    return str[1];
  }
  return '>'; // needs to be checked or discarded by calling method !
}

// -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
// returns the liana id from this command string
int getLiana(String str) {
  if(str.length() > 1 && '0' <= str[1] && str[1] < '4') {
    return str[1] - '0'; 
  }
  return -1; // needs to be checked or discarded by calling method !
}

// -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
// SerialEvent() is called each time a message is received 
// and the reception buffer (under the hood) is empty
// ie is is called only once if several messages 
// are sent in a chain.

// each message is ended by 2 ASCII Characters of codes 13 & 10.
// commands are sent one character at a time, so we receive
// packets of 3 characters where only the first is interesting.
// we only care about character with ASCII code > 32
void serialEvent() {
  while (Serial.available()) {
    // get the new byte:
    char inChar = (char)Serial.read();
    // add it to the inputString:
    if ( inChar > 32 ) {
      inputString += inChar;
      // if the incoming character is a  END_MESSAGE > , set a flag
      // so the main loop can do something about it:
      if (inChar == '>') {
        inputStringComplete = true;
      }
    }
  }
}
