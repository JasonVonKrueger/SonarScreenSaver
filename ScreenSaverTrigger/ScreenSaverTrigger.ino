// ---------------------------------------------------------------------------
// Thing to trigger screen saver on a Windows computer if the operator walks away
// ---------------------------------------------------------------------------

#include <NewPing.h>

#define TRIGGER_PIN  9  
#define ECHO_PIN     8  
#define MAX_SONAR_DISTANCE 500 
#define DELAY 500
#define SECONDS_TO_TRIGGER 3

NewPing sonar(TRIGGER_PIN, ECHO_PIN, MAX_SONAR_DISTANCE);

int _counter = 0;
int _max_ping_distance = 30;

void setup() {
  Serial.begin(9600);     
}

void loop() {
  delay(DELAY);
  unsigned int uS = sonar.ping(); 
  int dist = uS / US_ROUNDTRIP_IN;

  // let the user set _max_ping_distance
  if (Serial.available() > 0) {
          String incomingByte = Serial.readString();
          _max_ping_distance = incomingByte.toInt();
  }  
  
  // if dist has been more than _max_ping_distance for 3 seconds, trigger the screen saver
  if (dist >= _max_ping_distance)
    _counter++;
  else
    _counter = 0;

  // delay is 500ms so multiply times 2 for seconds
  if (_counter >= (SECONDS_TO_TRIGGER * 2))
    Serial.println("TRIG");

}
