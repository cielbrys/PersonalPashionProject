#include <SoftwareSerial.h>
#include <SerialCommand.h>
#include <SimpleTimer.h>

SerialCommand sCmd;
int hallSensorPin = A5;    
float state = 0;
int counter = 0;
float prevState = 0;
float prevMillis = 0.00;
bool firstTime = true;
int left = 0;
int right = 0;
int up = 0;
int down = 0;
int mid = 0;

float kmh = 0.00;
float rpm = 0.00;

int middleButtonLed = 3;
int checkMiddle = 0;

int blueButtonLed = 2;
int yellowButtonLed = 4;

int buttnLeftPin = 11;
int buttnLeft = 0;
int prevButtnLeft;

int buttnRightPin = 9;
int buttnRight = 0;
int prevButtnRight;

int buttnUpPin = 7;
int buttnUp = 0;
int prevButtnUp;

int buttnDownPin = 12;
int buttnDown = 0;
int prevButtnDown;

int buttnMiddlePin = 10;
int buttnMiddle = 0;
int prevButtnMiddle;

bool notStarted = true;

int tutorial = 0;

const int WHEEL_DIAMETER = 9; 
const int RPM_SAMPLE_PERIOD = 1;

SimpleTimer flashTimer;
SimpleTimer flashTimer2;
int timerInterval = 500;

SimpleTimer speedTimer;

void setup() {
  Serial.begin(9600);  
  pinMode(hallSensorPin, INPUT);
  
  pinMode(buttnLeftPin, INPUT);
  pinMode(buttnRightPin, INPUT);
  pinMode(buttnUpPin, INPUT);
  pinMode(buttnDownPin, INPUT);
  pinMode(buttnMiddlePin, INPUT);

  pinMode (middleButtonLed, OUTPUT);
  
  pinMode (blueButtonLed, OUTPUT);
  pinMode (yellowButtonLed, OUTPUT);
  
  flashTimer.setInterval(timerInterval);
  flashTimer2.setInterval(timerInterval*2);

  speedTimer.setInterval(200);
  
  while (!Serial){
  sCmd.addCommand("PING", pingHandler);
  sCmd.addCommand("blinkMid", blinkingMidLed);
  }
}

void loop(){
   state = digitalRead(hallSensorPin);
   buttnLeft = digitalRead(buttnLeftPin);
   buttnRight = digitalRead(buttnRightPin);
   buttnUp = digitalRead(buttnUpPin);
   buttnDown = digitalRead(buttnDownPin);
   buttnMiddle = digitalRead(buttnMiddlePin);

   if (state == LOW && prevState == HIGH){
      counter ++;
    }

   if(buttnLeft == LOW && prevButtnLeft == HIGH){
     left = 1;
        Serial.print(kmh);
        Serial.print(",");
        Serial.print(left);
        Serial.print(",");
        Serial.print(right);
        Serial.print(",");
        Serial.print(up);
        Serial.print(",");
        Serial.print(down);
        Serial.print(",");
        Serial.print(mid);
        Serial.println();
      if(tutorial == 1){
        tutorial ++;
          digitalWrite(yellowButtonLed, LOW);
      }
 
    } else{
      left = 0;
      if(tutorial == 4){
      digitalWrite(yellowButtonLed, HIGH);
      }
    }
    if (buttnRight == LOW && prevButtnRight == HIGH){
     right = 1;
        Serial.print(kmh);
        Serial.print(",");
        Serial.print(left);
        Serial.print(",");
        Serial.print(right);
        Serial.print(",");
        Serial.print(up);
        Serial.print(",");
        Serial.print(down);
        Serial.print(",");
        Serial.print(mid);
        Serial.println();
        if(tutorial == 1){
          tutorial ++;
          digitalWrite(yellowButtonLed, LOW);
      }

    } else {
      right = 0;
      if(tutorial == 4){
      digitalWrite(yellowButtonLed, HIGH);
      }
    }
    if (buttnUp == LOW && prevButtnUp == HIGH){
     up = 1;
        Serial.print(kmh);
        Serial.print(",");
        Serial.print(left);
        Serial.print(",");
        Serial.print(right);
        Serial.print(",");
        Serial.print(up);
        Serial.print(",");
        Serial.print(down);
        Serial.print(",");
        Serial.print(mid);
        Serial.println();

        if(tutorial == 2){
          tutorial ++;
          digitalWrite(blueButtonLed, LOW);
      }
    } else {
      up = 0;
      if(tutorial == 4){
      digitalWrite(blueButtonLed, HIGH);
      }

    }
   if (buttnDown == LOW && prevButtnDown == HIGH){
      down = 1;
        Serial.print(kmh);
        Serial.print(",");
        Serial.print(left);
        Serial.print(",");
        Serial.print(right);
        Serial.print(",");
        Serial.print(up);
        Serial.print(",");
        Serial.print(down);
        Serial.print(",");
        Serial.print(mid);
        Serial.println();

        if(tutorial == 2){
          tutorial ++;
          digitalWrite(blueButtonLed, LOW);
      }
   } else {
     down = 0;
     if(tutorial == 4){
     digitalWrite(blueButtonLed, HIGH);
     }
     }

     if (buttnMiddle == LOW && prevButtnMiddle == HIGH){
      mid = 1;
        Serial.print(kmh);
        Serial.print(",");
        Serial.print(left);
        Serial.print(",");
        Serial.print(right);
        Serial.print(",");
        Serial.print(up);
        Serial.print(",");
        Serial.print(down);
        Serial.print(",");
        Serial.print(mid);
        Serial.println();
        if(notStarted){
          notStarted = false;
          tutorial ++;
          digitalWrite(middleButtonLed, LOW);
        }
        
        if(tutorial == 3){
          tutorial ++;
         digitalWrite(middleButtonLed, HIGH);
      }
     } else {
       mid = 0;
       if(tutorial == 4){
       digitalWrite(middleButtonLed, HIGH);
       }

       }

       prevButtnLeft = buttnLeft;
       prevButtnRight = buttnRight;
       prevButtnUp = buttnUp;
       prevButtnDown = buttnDown;
       prevButtnMiddle = buttnMiddle;
   
  
   prevState = state;

    if(floor((millis()-prevMillis)/1000)>=RPM_SAMPLE_PERIOD)
    {
            prevMillis = millis();
            counter = 0;
    }

     if(speedTimer.isReady())
        {
            speedTimer.reset();
            float mps = ((WHEEL_DIAMETER * 3.14) * counter)/0.2;
            kmh = mps/10;
            
        }
        if(kmh > 0){
         // digitalWrite(ledPin, HIGH);
          Serial.print(kmh);
          Serial.print(",");
          Serial.print(left);
          Serial.print(",");
          Serial.print(right);
          Serial.print(",");
          Serial.print(up);
          Serial.print(",");
          Serial.print(down);
          Serial.print(",");
          Serial.print(mid);
          Serial.println();
        } else {
//          Serial.print(0);
//          Serial.print(",");
//          Serial.print(left);
//          Serial.print(",");
//          Serial.print(right);
//          Serial.print(",");
//          Serial.print(up);
//          Serial.print(",");
//          Serial.print(down);
//          Serial.print(",");
//          Serial.print(mid);
//          Serial.println();
         
        }
       

    if(notStarted){
      blinkingLed(middleButtonLed);
    }

    if(tutorial == 1){
          blinkingLed(yellowButtonLed);

    } else if (tutorial == 2){
      blinkingLed(blueButtonLed);
    } else if(tutorial == 3){
      blinkingLed(middleButtonLed);
    }
   
}

void blinkingLed (int ButtonLedPin) {
      if(flashTimer.isReady()){
        digitalWrite(ButtonLedPin, HIGH);
        flashTimer.reset(); 
      } else if (flashTimer2.isReady()) {
        digitalWrite(ButtonLedPin, LOW);
        flashTimer2.reset(); 
    }
}

void blinkingMidLed () {
    notStarted = true;
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
