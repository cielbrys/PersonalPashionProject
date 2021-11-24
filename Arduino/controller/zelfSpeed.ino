#include <SoftwareSerial.h>
#include <SerialCommand.h>
SerialCommand sCmd;
int hallSensorPin = 3;     
int ledPin =  13;    
int state = 0;
int counter = 0;
int prevState = 0;
float prevMillis = 0.00;
bool firstTime = true;
float kmh = 0.00;
float rpm = 0.00;

const int WHEEL_DIAMETER = 12; // please enter the wheel diameter in inches (or centimeters if USE_MPH is set to false)
const int RPM_SAMPLE_PERIOD = 1; // RPM is calculated based on the last 10 seconds of data

void setup() {
  Serial.begin(9600);
  pinMode(ledPin, OUTPUT);      
  pinMode(hallSensorPin, INPUT);
  while (!Serial);
  sCmd.addCommand("PING", pingHandler);
}

void loop(){
   state = digitalRead(hallSensorPin);
   if (prevState == 1 && state == 0){
      counter ++;

    }

    if(floor((millis()-prevMillis)/1000)>=RPM_SAMPLE_PERIOD)
    {
            prevMillis = millis();
            counter = 0;
    }

     if(abs((((float)(millis()-prevMillis)/1000)/60)) > 0.01)
        {
            rpm = (counter/2) / (((float)(millis()-prevMillis)/1000)/60);
            kmh = rpm * WHEEL_DIAMETER * 3.14 * 60 / 63360;
        }
        if(kmh > 0){
          digitalWrite(ledPin, HIGH);
          Serial.println(kmh);
          Serial.flush();
        } else {
          digitalWrite(ledPin, LOW);
        }

   prevState = state;
}

void pingHandler (const char *command) {
    Serial.println("PONG");
}

void echoHandler () {
  char *arg;
  arg = sCmd.next();
  if (arg != NULL)
    Serial.println(arg);
}
