#include <SoftwareSerial.h>

SoftwareSerial ser(2,3);

void setup() {
  Serial.begin(9600);
  Serial.println("SERIAL: BEGIN");

  ser.begin(9600);
  ser.println("SOFTWARE SERIAL: BEGIN");
}


void loop() {
  if(ser.available())
    Serial.write(ser.read());
}
