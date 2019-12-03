
// ---------------------------------------------------------------------
// IDLE SCENE
// ---------------------------------------------------------------------

//declaring the class
class IdleScene {
  public:
  // initialise global values for idle scene
  void init() {
    for(int i=0; i<4; i++) {
      lianaFront[i] = stripLen;
      lianaBack[i] = stripLen;
      sensorStates[i] = 0; // optional
    }
  }
  
  // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
  // notify unity of new state of liana
  void notifyUnityOfLianaChange(int liana, bool newSensorState) {
    if (newSensorState == true) {
       Serial.print(">I");
    } else {
       Serial.print(">O");
    }
    Serial.println(liana);
  }
  
  // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
  // goes one step forward in the liana animation
  // scene I : small gauge based on interaction
  // /!\ gauge is at end of LED strip !
  void updateLianas() {
    static int IdleStripLen = 20;
    for (int i=0; i<4; i++) {
      if (sensorStates[i]) {
        if (lianaFront[i] > stripLen - IdleStripLen) {
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
};

//and declaring the variable which will be used in main
IdleScene idleScene;
