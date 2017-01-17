void postureErrorHandler(const char *eventName, const char *data)
{
  if (data != NULL)
  {
    //Outputs data received on USB serial connection from the Kinect
    Serial.println(" ");
    Serial.print("INCOMING FROM KINECT: ");
    Serial.println(data);

    //Serial1 is RX/TX pins on Particle, this sends the received parsed bit vector to the arduino
    Serial1.write(data);
 }

  else
    //If the event is not triggered, then don't send anything to the Arduino
    Serial.println("NULL");
}

//Test method used to test data transfer from Photon to Arduino
/*
int sendData(String data) {
  if (data != NULL)
  {
    //Outputs data received on USB serial connection from the Kinect
    Serial.println(" ");
    Serial.print("Test Data: ");
    Serial.print(data);
    Serial.println(" ");

    //Serial1 is RX/TX pins on Particle, this sends the received parsed bit vector to the arduino
    Serial1.print(data);
	return -1;
  }
  return -1;
}*/




void setup() {
    Particle.subscribe("shoulder",postureErrorHandler,MY_DEVICES);
	//Particle.function("sendData", sendData);
	Serial.begin(9600);
	Serial.print("Particle is working and listening for events");
}

void loop() {
	if(Serial1.available())
		Serial.write(Serial1.read());
}
