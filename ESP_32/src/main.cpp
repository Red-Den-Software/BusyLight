#include <Arduino.h>
// NeoPixel Ring simple sketch (c) 2013 Shae Erisson
// Released under the GPLv3 license to match the rest of the
// Adafruit NeoPixel library
#include <Adafruit_NeoPixel.h>
#include <iostream>
// Which pin on the Arduino is connected to the NeoPixels?
#define PIN        D2 // On Trinket or Gemma, suggest changing this to 1
// How many NeoPixels are attached to the Arduino?
#define NUMPIXELS 16 // Popular NeoPixel ring size
// When setting up the NeoPixel library, we tell it how many pixels,
// and which pin to use to send signals. Note that for older NeoPixel
// strips you might need to change the third parameter -- see the
// strandtest example for more information on possible values.
Adafruit_NeoPixel pixels(NUMPIXELS, PIN, NEO_GRB + NEO_KHZ800);
u_int32_t green = pixels.Color(0,255,0);
u_int32_t red = pixels.Color(255,0,0);
u_int32_t blue = pixels.Color(0,0,255);
u_int32_t yellow = pixels.Color(255,255,0);
unsigned long previousMillis = 0; // variable to store the last time the function ran
const long interval = 1000;       // interval at which to run the function (milliseconds)
#define DELAYVAL 500 // Time (in milliseconds) to pause between pixels
int runCount = 0;
const int maxRuns = 2;
int currentBrightness = 0;
void setup() {
  // These lines are specifically to support the Adafruit Trinket 5V 16 MHz.
  // Any other board, you can remove this part (but no harm leaving it):
#if defined(__AVR_ATtiny85__) && (F_CPU == 16000000)
  clock_prescale_set(clock_div_1);
#endif
  // END of Trinket-specific code.
  pixels.begin(); // INITIALIZE NeoPixel strip object (REQUIRED)
  Serial.begin(9600);
  pixels.show();

}
void solidred(){
  pixels.fill(red,0,255);
  pixels.show();
}


void clear(){
  pixels.fill(0,0,0);
  pixels.show();
}
void solidgreen(){
  pixels.fill(green, 0, 255);
  pixels.show();
}
void lights() {
  if (Serial.available() > 0) {
    byte incomingByte = Serial.read();
    if (incomingByte == 0x01) {
      solidgreen();
    }
    else if (incomingByte == 0x02) {
      solidred();
    }
  }
} 
void colorWipe(uint32_t c, uint8_t wait) {
  for(uint16_t i=0; i<pixels.numPixels(); i++) {
    pixels.setPixelColor(i, c);
    pixels.show();
    delay(wait);
  }
}
void spiralLight(){
  colorWipe(pixels.Color(255,255,0), 50);
  pixels.show();
}
void loop() {
   // Only read if a byte is available
  if (Serial.available() > 0) {
    byte b = Serial.read();

    if (b == 0x55) {
      Serial.write(0xAA);  // Probe response
    }
    else if (b == 0x01) {
      solidgreen();
    }
    else if (b == 0x02) {
      solidred();
    }
    else if(b == 0x04){
        spiralLight();
    }
    else if (b == 0x03) {
  while (Serial.available() == 0);

  currentBrightness = Serial.read();

  if (currentBrightness < 1) currentBrightness = 1;

  pixels.setBrightness(currentBrightness);
  pixels.show();
}

  }
}

// Function to send XOFF and XON if the Arduino's input buffer gets full
// This would be implemented as part of the receiving logic to tell the
// other device to stop/start sending to the Arduino.


