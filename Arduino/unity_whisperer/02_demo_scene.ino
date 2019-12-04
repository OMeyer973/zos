
// ---------------------------------------------------------------------
// DEMO SCENE 
// ---------------------------------------------------------------------

//declaring the class
class DemoScene {
  public:
  // initialise global values for demo scene
  void init() {
    for(int i=0; i<4; i++) {
      lianaFront[i] = -1;
      lianaBack[i] = -1;
      sensorStates[i] = 0; // optional
    }
  }
  
  // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
  // start an animation on a liana for the Demo scene
  // has no effect if an animation is allready running
  // on the given liana
  void beginLianaAnimationOnCommand(String str) {
    //Serial.print("begin liana ");
    //Serial.println(str);
    if(serialCommunication.getCommand(str) == 'L') {
      int liana = serialCommunication.getLiana(str);
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
  void updateLianas() {
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
      
    // updating mask for animations
    lianaMin[i] = lianaBack[i];
    lianaMax[i] = lianaFront[i];
    }
  }
};

//and declaring the variable which will be used in main
DemoScene demoScene;
