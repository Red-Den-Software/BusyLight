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
byte cancellationToken = 0xFE;
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
  
  delay(10000);

}
struct VersionInfo {
	uint8_t major;
	uint8_t minor;
	uint8_t patch;
    uint8_t git_sha;
};
Version currentVersion = { 1, 0, 0, "1234567" };
enum LedMode {
    MODE_OFF,
    MODE_SOLID,
    MODE_SPIRAL
};

LedMode ledMode = MODE_OFF;

uint32_t activeColor = 0;
int spiralPosition = 0;

unsigned long lastSpiralStep = 0;
const unsigned long spiralInterval = 50;

bool waitingForBrightness = false;

void renderLeds()
{
    pixels.clear();

    if (ledMode == MODE_SOLID)
    {
        pixels.fill(activeColor, 0, NUMPIXELS);
    }
    else if (ledMode == MODE_SPIRAL)
    {
        int count = pixels.numPixels();

        // Preserve your existing three-LED comet trail.
        for (int trail = 0; trail < 3; trail++)
        {
            int index =
                (spiralPosition - trail + count) % count;

            pixels.setPixelColor(index, activeColor);
        }
    }

    pixels.show();
}

void startSolid(uint32_t color)
{
    activeColor = color;
    ledMode = MODE_SOLID;
    renderLeds();
}

void startSpiral(uint32_t color)
{
    activeColor = color;
    ledMode = MODE_SPIRAL;

    // Start a new spiral when a new spiral command is received.
    spiralPosition = 0;
    lastSpiralStep = millis();

    renderLeds();
}

void stopAnimation()
{
    ledMode = MODE_OFF;
    pixels.clear();
    pixels.show();
}

void processSerialCommands()
{
    while (Serial.available() > 0)
    {
        byte command = Serial.read();
        

        // The next byte is the brightness value.
        if (waitingForBrightness)
        {
            currentBrightness = command;

            if (currentBrightness < 1)
                currentBrightness = 1;

            pixels.setBrightness(currentBrightness);
            waitingForBrightness = false;

            // Redraw the CURRENT frame, not a new spiral.
            renderLeds();
            continue;
        }
        void checkIncomingVersion()
        {
            if (!Serial.available())
                return;

            String message = Serial.readStringUntil('\n');

            Version incoming;

            if (!parseVersion(message, incoming))
            {
                Serial.println("Invalid version message");
                return;
            }

            if (isNewerVersion(incoming, currentVersion))
            {
                Serial.println("New version available!");

                // Handle firmware update or version notification here.
            }
            else if (incoming.major == currentVersion.major &&
                incoming.minor == currentVersion.minor &&
                incoming.patch == currentVersion.patch)
            {
                if (incoming.githubSha != currentVersion.githubSha)
                    Serial.println("Same version, different Git commit");
                else
                    Serial.println("Already running this version");
            }
            else
            {
                Serial.println("Incoming version is older");
            }
        }

        switch (command) {
		    case 0x03:
			waitingForBrightness = true;
			break;
            case 0x06F:
			checkIncomingVersion();
			break;
            case 0xFE:
			stopAnimation();

            break;
			default:
				break;
        }
       

        // Static colors
        switch (command)
        {
            case 0x01:
                startSolid(green);
                break;

            case 0x02:
                startSolid(red);
                break;

            case 0x05:
                startSolid(blue);
                break;

            case 0x06:
                startSolid(yellow);
                break;

            case 0x07:
                startSolid(purple);
                break;

            // Spiral colors
            case 0x0A:
                startSpiral(green);
                break;

            case 0x0B:
                startSpiral(red);
                break;

            case 0x0C:
                startSpiral(blue);
                break;

            case 0x0D:
                startSpiral(yellow);
                break;

            case 0x0E:
                startSpiral(purple);
                break;

            default:
                break;
        }
    }
}

void updateSpiral()
{
    if (ledMode != MODE_SPIRAL)
        return;

    unsigned long now = millis();

    if (now - lastSpiralStep >= spiralInterval)
    {
        lastSpiralStep = now;

        spiralPosition =
            (spiralPosition + 1) % NUMPIXELS;

        renderLeds();
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
   // Handle brightness, color, and cancellation commands.
    processSerialCommands();

    // Advance the spiral only when the next frame is due.
    updateSpiral();

    // Preserve your existing heartbeat behavior.
    unsigned long currentMillis = millis();

    if (currentMillis - previousMillis >= interval)
    {
        previousMillis = currentMillis;
        Serial.write(0x3F);
    }
  
}


