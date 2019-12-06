
// ---------------------------------------------------------------------
// ACTION SCENE
// ---------------------------------------------------------------------

//declaring the class
class ActionScene
{
  public : 
  // are we in the filling part or erasing part of the liana interaction ?
  bool lianaFilling[4];
  
  // initialise global values for action scene
  void init() {
    for(int i=0; i<4; i++) {
      initLiana(i);
    }
  }
  
  // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
  // initialise global values for liana i in Action scene
  void initLiana(int i) {
    lianaFront[i] = stripLen;
    lianaBack[i] = stripLen;
    lianaFilling[i] = true;
    sensorStates[i] = 0; // optional
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
  
  void updateLianas() {
  
    for (int i=0; i<4; i++) {
      if (lianaFilling[i]) {
        fillLiana(i);
      } else {
        eraseLiana(i);
      }
    }
  }
  
  // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
  // 1st part of the liana update
  void fillLiana(int i) {
    //lianaFront moves forward when sensor is activated (1st part)
    if (sensorStates[i]) {
      if (lianaFront[i] >= 0) {
        lianaFront[i] --;
  
        // is true for one frame when the liana has been filled entirely
        if (lianaFront[i] <= 0) {
          SendLianaTriggerToUnity(i);
          lianaFilling[i] = false;
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

    // updating mask for animations
    lianaMin[i] = lianaBack[i];
    lianaMax[i] = stripLen;
  }
  
  // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
  // 2nd part of the liana update
  void eraseLiana(int i) {
    if (lianaBack[i] > 0){
      lianaBack[i]--;
      return;
    } //else
    // allow new action only if we first let go of the liana 
    if (sensorStates[i] == false) {
      initLiana(i);
    }

    // updating mask for animations
    lianaMin[i] = -1;
    lianaMax[i] = lianaBack[i];
  }
  
  // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
  // notify unity of new state of liana
  void SendLianaTriggerToUnity(int liana) {
    Serial.print(">L");
    Serial.println(liana);
  }
};

//and declaring the variable which will be used in main
ActionScene actionScene;
