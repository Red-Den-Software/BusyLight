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
// pixelss you might need to change the third parameter -- see the
// strandtest example for more information on possible values.
Adafruit_NeoPixel pixels(NUMPIXELS, PIN, NEO_GRB + NEO_KHZ800);
uint32_t green = pixels.Color(0,255,0);
uint32_t red = pixels.Color(255,0,0);
uint32_t blue = pixels.Color(0,0,255);
uint32_t yellow = pixels.Color(255,255,0);
uint32_t purple = pixels.Color(128,0,128);
uint8_t data[10] = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
unsigned long previousMillis = 0; // variable to store the last time the function ran
const long interval = 5000;       // interval at which to run the function (milliseconds)
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
  pixels.begin(); // INITIALIZE NeoPixel pixels object (REQUIRED)
  Serial.begin(9600);
  pixels.show();
  

}

void rotatePixels(uint32_t color, int wait) {
  int count = pixels.numPixels();
  byte s = Serial.read();
  if (s = 0x03 || 0x55){
    while (true) {
    for (int i = 0; i < count; i++) {
   // Set all pixel colors to 'off'
    pixels.clear();
    // Turn on the current pixel and a few trailing ones for a "comet" trail effect
    pixels.setPixelColor(i, color);
     pixels.setPixelColor((i - 1 + count) % count, color);
            pixels.setPixelColor((i - 2 + count) % count, color);
    
    pixels.show();
    delay(wait);
  }
  }
}
  
}
}
void solidblue(){
  pixels.fill(blue,0,255);
  pixels.show();
}
void solidred(){
  pixels.fill(red,0,255);
  pixels.show();
}
void solidyellow(){
  pixels.fill(yellow,0,255);
  pixels.show();
}
void solidpurple(){
  pixels.fill(purple,0,255);
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
void spiralGreen() {
    rotatePixels(green, 50);
}

void spiralRed() {
    rotatePixels(red, 50);
}

void spiralBlue() {
    rotatePixels(blue, 50);
}

void spiralYellow() {
    rotatePixels(yellow, 50);
}

void spiralPurple() {
    rotatePixels(purple, 50);
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

void spiralLight(){
  rotatePixels(pixels.Color(255,255,0), 50);
  pixels.show();
}
void (*solidlight[])() = {
    solidgreen,    // 0x01
    solidred,      // 0x02
    spiralLight,   // 0x04
    solidblue,     // 0x05
    solidyellow,   // 0x06
    solidpurple    // 0x07
};

uint32_t Wheel(byte WheelPos) {
    WheelPos = 255 - WheelPos;
    if (WheelPos < 85) {
        return pixels.Color(255 - WheelPos * 3, 0, WheelPos * 3);
    }
    if (WheelPos < 170) {
        WheelPos -= 85;
        return pixels.Color(0, WheelPos * 3, 255 - WheelPos * 3);
    }
    WheelPos -= 170;
    return pixels.Color(WheelPos * 3, 255 - WheelPos * 3, 0);
};

void (*spiralLightFunc[15])() = {};
void loop() {
   // Only read if a byte is available
    spiralLightFunc[0x0A] = spiralGreen;
    spiralLightFunc[0x0B] = spiralRed;
    spiralLightFunc[0x0C] = spiralBlue;
    spiralLightFunc[0x0D] = spiralYellow;
    spiralLightFunc[0x0E] = spiralPurple;
   unsigned long currentMillis = millis();

    if (currentMillis - previousMillis >= interval)
    {
        previousMillis = currentMillis;
        Serial.write(0x3F);
    }
    
  if (Serial.available() > 0) {
    byte b = Serial.read();
    if (b == 0x55) {
        Serial.write(0xAA);
    }
    else if (b == 0x03) {
        while (Serial.available() == 0);

        currentBrightness = Serial.read();

        if (currentBrightness < 1)
            currentBrightness = 1;

        pixels.setBrightness(currentBrightness);
        pixels.show();
    }
    else if (b >= 0x01 && b <= 0x07) {

        if (solidlight[b - 1] != nullptr) {
            solidlight[b - 1]();
        }
    }
	else if (b < 15 && spiralLightFunc[b] != nullptr) {
    spiralLightFunc[b]();
}
  }
  
}


