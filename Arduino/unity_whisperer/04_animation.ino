
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
  // displays a fade in & out animation on the LED strip
  void rainDownAnimation(float animationSpeed) {
    int dropLength = 2;
    
    float t = animationSpeed * (float)frame;
    int animationFrame = (int)t;
    int v = getFadeIn(t, animationSpeed);
    int w = getFadeOut(t, animationSpeed);
    
    for (int liana=0; liana<4; liana++) {
      for (int i=1; i<6; i++) { // had a glitchy led when starting at 0...
        int frontPixel = int((randomFloats[liana] + randomFloats[i])*stripLen + animationFrame) % (stripLen +2); // +2 for oob margin lol
        int backPixel = (frontPixel - dropLength) % (stripLen +2); // +2 for oob margin lol
        if (!between(frontPixel, lianaMin[i], lianaMax[i]))
          pixels[liana].setPixelColor(frontPixel, pixels[i].Color(v, v, v));
        if (!between(backPixel, lianaMin[i], lianaMax[i]))
          pixels[liana].setPixelColor(backPixel, pixels[i].Color(w, w, w));
      }
      //pixels[liana].show();
    }
  }
  

  
  // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
  // play a transition
  void playTransition(char transition) {
    switch (transition) {
    case 'S': // SimonStart
      //openAnimation(.4);
      fadeAnimation(.2);
      goDownAnimation(.5);
      break;
    case 'A': // BeginAction
      fadeAnimation(.2);
      goUpAnimation(.5);
      break;
    case 'V': // SmallVictory
      fadeAnimation(.2);
      goDownAnimation(.5);
      break;
    case 'W': // BigVictory
      fadeAnimation(.2);
      openAnimation(1);
      openAnimation(1);
      openAnimation(1);
      break;
    case 'F': // Fail
      closeAnimation(1);
      closeAnimation(1);
      closeAnimation(1);
      break;
    default:
      break;
    }
  }
  
  // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
  // get the display value for a smooth fade in.
  // only fade if the animationSpeed is low enough
  int getFadeIn(float t, float animationSpeed) {
    int v;
    if (animationSpeed < .3) {
      int i = (int)t;
      int v = (t*255 - i*255) * brightness;
      return gamma8[v];
    }
    return gamma8[int(255*brightness)];
  }
  
  // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
  // get the display value for a smooth fade out
  // only fade if the animationSpeed is low enough
  int getFadeOut(float t, float animationSpeed) {
    if (animationSpeed < .3) {
      int i = (int)t;
      int w = (255-(t*255 - i*255)) * brightness; // current erase value for a smooth fade
      return gamma8[w];  
    }
    return 0;
  }

  // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
  // displays a fade in & out animation on the LED strip
  void fadeAnimation(float animationSpeed) {
    int bandLength = halfStripLen;
    
    float dt = 2*PI / stripLen * animationSpeed;
    for (float t=0; t<2*PI; t+=dt) {
      uint32_t loopStartTime = millis();
      int v = gamma8[int((.5-cos(t)*.5)*255*brightness)];
      
      for (int liana=0; liana<4; liana++) {
        for (int i=0; i<stripLen; i++) {
          pixels[liana].setPixelColor(i, pixels[i].Color(v, v, v));
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
  void openAnimation(float animationSpeed) {
    int bandLength = halfStripLen;

    for (float t=0; t<=halfStripLen+bandLength+2; t+=animationSpeed) {
      int i = (int)t;
      uint32_t loopStartTime = millis();
      int v = getFadeIn(t, animationSpeed);
      int w = getFadeOut(t, animationSpeed);
      for (int liana=0; liana<4; liana++) {
        pixels[liana].setPixelColor(halfStripLen + i, pixels[i].Color(v, v, v));
        pixels[liana].setPixelColor(halfStripLen - i, pixels[i].Color(v, v, v));
        if(i > bandLength) {
          pixels[liana].setPixelColor(max(halfStripLen + i - bandLength, halfStripLen), pixels[i].Color(w, w, w));
          pixels[liana].setPixelColor(min(halfStripLen - i + 1 + bandLength, halfStripLen), pixels[i].Color(w, w, w));          
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
  void closeAnimation(float animationSpeed) {
    int bandLength = halfStripLen;

    for (float t=0; t<=halfStripLen+bandLength + 2; t+=animationSpeed) {
      int i = (int)t;
      uint32_t loopStartTime = millis();
      int v = getFadeIn(t, animationSpeed);
      int w = getFadeOut(t, animationSpeed);
      for (int liana=0; liana<4; liana++) {
        if (i < halfStripLen){          
          pixels[liana].setPixelColor(min(i + 1, halfStripLen), pixels[i].Color(v, v, v));
          pixels[liana].setPixelColor(max(stripLen - i, halfStripLen), pixels[i].Color(v, v, v));
        }
        pixels[liana].setPixelColor(min(i - bandLength, halfStripLen), pixels[i].Color(w, w, w));
        pixels[liana].setPixelColor(max(bandLength + stripLen - i, halfStripLen), pixels[i].Color(w, w, w));

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
    int bandLength = stripLen;
    for (float t=0; t<=stripLen+bandLength+1; t+=animationSpeed) {
      int i = stripLen - (int)t;
      uint32_t loopStartTime = millis();
      int v = getFadeIn(t, animationSpeed);
      int w = getFadeOut(t, animationSpeed);
      for (int liana=0; liana<4; liana++) {
        pixels[liana].setPixelColor(stripLen - i, pixels[i].Color(v,v,v));
        pixels[liana].setPixelColor(stripLen - max(i + bandLength, 0), pixels[i].Color(w,w,w));
        pixels[liana].show();
      }
      while((millis()-loopStartTime) < animationDeltaTime){
        //wait
      }  
    }
  }
  
  // -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  
  // displays a blind going up animation on the LED strip
  void goUpAnimation(float animationSpeed) {
    int bandLength = stripLen;
    for (float t=0; t<=stripLen+bandLength+1; t+=animationSpeed) {
      int i = (int)t;
      uint32_t loopStartTime = millis();
      int v = getFadeIn(t, animationSpeed);
      int w = getFadeOut(t, animationSpeed);
      for (int liana=0; liana<4; liana++) {
        pixels[liana].setPixelColor(stripLen - i, pixels[i].Color(v,v,v));
        pixels[liana].setPixelColor(stripLen - max(i - bandLength, 0), pixels[i].Color(w,w,w));
        pixels[liana].show();
      }
      while((millis()-loopStartTime) < animationDeltaTime){
        //wait
      }  
    }
  }

  
};

Animation animation;
