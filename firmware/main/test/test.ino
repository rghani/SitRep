#include <SoftwareSerial.h>
String inputString;
bool stringComplete = false;

SoftwareSerial ser(2,3);

void setup() {
  Serial.begin(9600);
  Serial.println("SERIAL: BEGIN");

  ser.begin(9600);
  ser.println("SOFTWARE SERIAL: BEGIN");
  ser.listen();
  if(ser.isListening())
    Serial.println("Software Serial Instance is Listening");
}


void loop() {
//  if(ser.available())
//    Serial.write(ser.read());
    if(stringComplete){
      //Region where you can do stuff with the newly arrived data
      Serial.println(inputString);
      inputString = "";
      
      
      stringComplete = false;
    }
}


/*
  SerialEvent occurs whenever a new data comes in the
 hardware serial RX.  This routine is run between each
 time loop() runs, so using delay inside loop can delay
 response.  Multiple bytes of data may be available.
 */
void serialEvent(){
    while (ser.available()) {
    // get the new byte:
    char inChar = (char)ser.read();
    // add it to the inputString:
    inputString += inChar;
    // if the incoming character is a newline, set a flag
    // so the main loop can do something about it:
    if (inChar == '\n') {
      stringComplete = true;
    }
  }
}
