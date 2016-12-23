// ---------------------------------------------------------------------------
// Thing to trigger screen saver on a Windows computer if the operator walks away
// ---------------------------------------------------------------------------

#include <NewPing.h>

#define TRIGGER_PIN  9  
#define ECHO_PIN     8  
#define MAX_SONAR_DISTANCE 500 

NewPing sonar(TRIGGER_PIN, ECHO_PIN, MAX_SONAR_DISTANCE);

void setup() {
  Serial.begin(9600); 
}

void loop() {
  delay(1000);
  unsigned int uS = sonar.ping(); 
  int dist = uS / US_ROUNDTRIP_IN;

  Serial.println(dist);
}
