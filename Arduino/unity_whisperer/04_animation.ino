
// ---------------------------------------------------------------------
// ANIMATIONS & TRANSITIONS
// ---------------------------------------------------------------------

//declaring the class
class Animation
{
  public :
  uint32_t animationDeltaTime = 0; // minimum time between 2 loops (in ms) 16ms->60fps
  int halfStripLen = stripLen/2;

  int randomCount = 20;
  float randomFloats[20] = { // random values used by animations
    0.1975, 0.5977, 0.0219, 0.7098, 0.2146, 0.9342, 0.7323, 0.7339, 
    0.3824, 0.5468, 0.6305, 0.9366, 0.3132, 0.6912, 0.7837, 0.5978, 
    0.0091, 0.0648, 0.2713, 0.6705
  };

  // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
  // displays a fade in & out animation on the LED stri
  void rainAnimation(float animationSpeed, bool goingUp, int r, int g, int b) {
    int dropLength = 2;
    
    float t = animationSpeed * (float)frame;
    int animationFrame = (int)t;
    float v = getFadeIn(t, animationSpeed)*.1;
    float w = getFadeOut(t, animationSpeed)*.1;
    
    for (int liana=0; liana<4; liana++) {
      for (int i=1; i<4; i++) { // had a glitchy led when starting at 0...
        int frontPixel = int((randomFloats[liana] + randomFloats[i])*stripLen + animationFrame) % (stripLen +2); // +2 for oob margin lol
        int backPixel = (frontPixel - dropLength) % (stripLen +2); // +2 for oob margin lol
        if (goingUp) {
          frontPixel = stripLen - frontPixel - 1;
          backPixel = stripLen - backPixel - 1;
        }
        if (!between(frontPixel, lianaMin[liana], lianaMax[liana]))
          pixels[liana].setPixelColor(frontPixel, pixels[liana].Color(r*v, g*v, b*v));
        if (!between(backPixel, lianaMin[liana], lianaMax[liana]))
          pixels[liana].setPixelColor(backPixel, pixels[liana].Color(r*w, g*w, b*w));
      }
      //pixels[liana].show();
    }
  }
  
  // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
  // displays a fade in & out animation on the LED strip
  void rainDownAnimation(float animationSpeed, int r, int g, int b) {
    rainAnimation(animationSpeed, false, r, g, b);
  }
  
  // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
  // displays a fade in & out animation on the LED strip
  void rainUpAnimation(float animationSpeed,  int r, int g, int b) {
    rainAnimation(animationSpeed, true, r, g, b);
  }
  
  // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
  // play a transition
  void playTransition(char transition) {
    switch (transition) {
    case 'S': // SimonStart
      //openAnimation(.4);
      fadeAnimation(1, idleColor[0], idleColor[1], idleColor[2]);
      fadeAnimation(.15, demoColor[0], demoColor[1], demoColor[2]);
      //goDownAnimation(.8);
      break;
    case 'A': // BeginAction
      fadeAnimation(.15, actionColor[0], actionColor[1], actionColor[2]);
      //goUpAnimation(.8);
      break;
    case 'V': // SmallVictory
      fadeAnimation(.15, demoColor[0], demoColor[1], demoColor[2]);
      //goDownAnimation(.8);
      break;
    case 'W': // BigVictory
      fadeAnimation(.2, idleColor[0], idleColor[1], idleColor[2]);
      openAnimation(.3, idleColor[0], idleColor[1], idleColor[2]);
      fadeAnimation(.2, idleColor[0], idleColor[1], idleColor[2]);
      break;
    case 'F': // Fail
      fadeAnimation(1, failColor[0], failColor[1], failColor[2]);
      closeAnimation(1, failColor[0], failColor[1], failColor[2]);
      closeAnimation(1, failColor[0], failColor[1], failColor[2]);
      closeAnimation(1, failColor[0], failColor[1], failColor[2]);
      break;
    default:
      break;
    }
  }
  
  // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
  // get the display value for a smooth fade in.
  // only fade if the animationSpeed is low enough
  float getFadeIn(float t, float animationSpeed) {
    int v;
    if (animationSpeed < .15) {
      int i = (int)t;
      v = (t*255 - i*255) * brightness;
      return gamma8[v] / (float)255;
    }
    return gamma8[int(255*brightness)] / (float)255;
  }
  
  // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
  // get the display value for a smooth fade out
  // only fade if the animationSpeed is low enough
  int getFadeOut(float t, float animationSpeed) {
    if (animationSpeed < .3) {
      int i = (int)t;
      int w = (255-(t*255 - i*255)) * brightness; // current erase value for a smooth fade
      return gamma8[w] / (float)255;  
    }
    return 0;
  }

  // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
  // displays a fade in & out animation on the LED strip
  void fadeAnimation(float animationSpeed, int r, int g, int b) {
    int bandLength = halfStripLen;
    
    float dt = 2*PI / stripLen * animationSpeed;
    for (float t=0; t<2*PI; t+=dt) {
      uint32_t loopStartTime = millis();
      float v = gamma8[int((.5-cos(t)*.5)*255*brightness)] / (float)255;
      
      for (int liana=0; liana<4; liana++) {
        for (int i=0; i<stripLen; i++) {
          pixels[liana].setPixelColor(i, pixels[i].Color(r*v, g*v, b*v));
        }
        pixels[liana].show();
      }
      while((millis()-loopStartTime) < animationDeltaTime){
          //wait
      }
    }
  }
  // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
  // displays a blind opening animation on the LED strip
  void openAnimation(float animationSpeed, int r, int g, int b) {
    int bandLength = halfStripLen;

    for (float t=0; t<=halfStripLen+bandLength+2; t+=animationSpeed) {
      int i = (int)t;
      uint32_t loopStartTime = millis();
      float v = getFadeIn(t, animationSpeed);
      float w = getFadeOut(t, animationSpeed);
      for (int liana=0; liana<4; liana++) {
        pixels[liana].setPixelColor(halfStripLen + i, pixels[i].Color(r*v, g*v, b*v));
        pixels[liana].setPixelColor(halfStripLen - i, pixels[i].Color(r*v, g*v, b*v));
        if(i > bandLength) {
          pixels[liana].setPixelColor(max(halfStripLen + i - bandLength, halfStripLen), pixels[i].Color(r*w, g*w, b*w));
          pixels[liana].setPixelColor(min(halfStripLen - i + 1 + bandLength, halfStripLen), pixels[i].Color(r*w, g*w, b*w));          
        }
        pixels[liana].show();
      }
      while((millis()-loopStartTime) < animationDeltaTime){
        //wait
      }  
    }
  }
  
  // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
  // displays a blind closing animation on the LED strip
  void closeAnimation(float animationSpeed, int r, int g, int b) {
    int bandLength = halfStripLen;

    for (float t=0; t<=halfStripLen+bandLength + 2; t+=animationSpeed) {
      int i = (int)t;
      uint32_t loopStartTime = millis();
      float v = getFadeIn(t, animationSpeed);
      float w = getFadeOut(t, animationSpeed);
      for (int liana=0; liana<4; liana++) {
        if (i < halfStripLen){          
          pixels[liana].setPixelColor(min(i + 1, halfStripLen), pixels[i].Color(v, 0, 0));
          pixels[liana].setPixelColor(max(stripLen - i, halfStripLen), pixels[i].Color(v, 0, 0));
        }
        pixels[liana].setPixelColor(min(i - bandLength, halfStripLen), pixels[i].Color(w, 0, 0));
        pixels[liana].setPixelColor(max(bandLength + stripLen - i, halfStripLen), pixels[i].Color(w, 0, 0));

        pixels[liana].show();
      }
      while((millis()-loopStartTime) < animationDeltaTime){
        //wait
      }  
    }
  }

  // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
  // displays a blind going down animation on the LED strip
  void oneWayAnimation(float animationSpeed, bool isUp) {
    int bandLength = stripLen;
    for (float t=0; t<=stripLen+bandLength+1; t+=animationSpeed) {
      int i;
      if (isUp)
        i = (int)t;
      else
        i = stripLen - (int)t;
      uint32_t loopStartTime = millis();
      float v = getFadeIn(t, animationSpeed);
      float w = getFadeOut(t, animationSpeed);
      for (int liana=0; liana<4; liana++) {
        pixels[liana].setPixelColor(stripLen - i, pixels[i].Color(v,v,v));
        if (isUp)
          pixels[liana].setPixelColor(stripLen - max(i - bandLength, 0), pixels[i].Color(w,w,w));
        else
          pixels[liana].setPixelColor(stripLen - max(i + bandLength, 0), pixels[i].Color(w,w,w));
        pixels[liana].show();
      }
      while((millis()-loopStartTime) < animationDeltaTime){
        //wait
      }  
    }
  }
  
  // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
  // displays a blind going down animation on the LED strip
  void goDownAnimation(float animationSpeed) {
      oneWayAnimation(animationSpeed, false);
  }
  
  // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
  // displays a blind going up animation on the LED strip
  void goUpAnimation(float animationSpeed) {
      oneWayAnimation(animationSpeed, true);    
  }

  
};

Animation animation;
