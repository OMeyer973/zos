
// ---------------------------------------------------------------------
// COMUNICATION AND PARSING
// ---------------------------------------------------------------------

//declaring the class
class SerialCommunication
{
  public : 
  // COMMUNICATION VARS
  String inputString = "";              // a string to hold incoming data
  bool inputStringComplete = false;     // whether the string is complete
  
  
  // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
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
  // returns the transition character from this command string
  char getTransition(String str) {
    if(str.length() > 2) {
      return str[2]; 
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
};

//and declaring the variable which will be used in main
SerialCommunication serialCommunication;

void serialEvent() {
  serialCommunication.serialEvent();
}
