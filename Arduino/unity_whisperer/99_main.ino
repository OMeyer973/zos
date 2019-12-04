
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
  
  String serialMsg = serialCommunication.getSerialMessage();
  changeSceneOnCommand(serialMsg);
  
  switch (currScene) {
    case 'I': // idle scene
      GetSensorInput();
      idleScene.updateLianas();
      break;
    case 'D': // demo scene
      demoScene.beginLianaAnimationOnCommand(serialMsg);
      demoScene.updateLianas();
      break;
    case 'A': // action scene
      GetSensorInput();
      actionScene.updateLianas();
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
  if(serialCommunication.getCommand(str) == 'S') {
    currScene = serialCommunication.getScene(str);
    Serial.print("changing scene for ");
    Serial.println(currScene);
    // todo : transition
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
      idleScene.init();
      break;
    case 'A':
      actionScene.init();
      // todo : auto go back to idle after some time ?
      break;
    case 'D':
      demoScene.init();
      break;
    default:
      break;
  }
}

// -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
// get current value from the sensorStates and update 
// the boolean values we use in our program
void GetSensorInput() {
  //Serial.print("sensor values [");
  int sensorValue =0;
  for (int i=0; i<4; i++) {
    sensorValue = analogRead(sensorPins[i]);
    bool newSensorState = sensorValue < 128;

    // on sensor state change
    if (newSensorState != sensorStates[i]) {
      if (currScene == 'I') {
        idleScene.notifyUnityOfLianaChange(i, newSensorState);
      }
    }
    
    sensorStates[i] = newSensorState;

    //Serial.print(", ");
    //Serial.print(sensorStates[i]);    
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
