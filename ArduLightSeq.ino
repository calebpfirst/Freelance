/* 
@Author: Caleb Price
@Company: Boeing
@Date: 11/23/2022
@Description: Simple Arduino circuit for sequencing a powered relay's ON/OFF period
*/

// Globals
// configurable settings
// 2, 5, 10, 20

// constants
#define CTRL_LOOP_PERIOD_MS   (int)1
#define VARIAC_THRES          (int)150
#define TMR_CNT               (int)5
#define TMR_PERIOD            (int)250

struct Timer
{
public:
// interval expressed in ms
void SetInterval(int interval)
{
  this->interval_ = interval;
  this->lastEpoch_ = millis();
}
bool Elapsed()
{
  int currentTimeMS = millis();
  if (currentTimeMS >= (this->lastEpoch_+this->interval_))
  {
    this->lastEpoch_ = currentTimeMS;

    return true;
  }

  return false;
}
private:
int interval_, lastEpoch_;
};

const int sensorPin     = A8;       // select the input pin for the potentiometer
const int relayOut      = 12;
const int rxStatusOut   = 8;
// runtime variables
Timer myTimer;
int myTimerIdx = 1;
int sensorValue = 0;  // variable to store the value coming from the sensor
bool patternChange = false;
bool btnOn         = false;
bool digitalOutOff = false;

void ReadAD()
{
  int tmpSensorValue = analogRead(sensorPin);
  if ((tmpSensorValue <= VARIAC_THRES) &&
      (tmpSensorValue > 0))
  {
    sensorValue = 1;
    if (++myTimerIdx >= TMR_CNT)
    {
      myTimerIdx = 0;
      digitalOutOff = true;
    }
    else
    {
      digitalOutOff = false;
    }
    
    for (int i = 5; i >= 0; --i)
    {
      digitalWrite(rxStatusOut, i%2==0);
      delay(50);
    }
    digitalWrite(rxStatusOut, LOW);
    // flush sensor
    myTimer.SetInterval(TMR_PERIOD*myTimerIdx);
    Serial.println(String(TMR_PERIOD*myTimerIdx));
  }
  else
  {
    sensorValue = 0;
  }
}

void DoRelay()
{
  if (!digitalOutOff)
  {
    if (myTimer.Elapsed())
    {
      btnOn = !btnOn;
      digitalWrite(relayOut, btnOn);
      //Serial.println("\n" + String(btnOn));
    }
  }
  else
  {
    btnOn = false;
  }
}

void CycleTimer()
{
  if (patternChange)
  {
    if (++myTimerIdx >= TMR_CNT)
    {
      myTimerIdx = 0;
    }
    myTimer.SetInterval(TMR_PERIOD*myTimerIdx);
    patternChange = false;
  }
  //Serial.println(String(myTimerIdx) + "\n");
}

void setup() {
  // initialize a sig output to power relay
  pinMode(relayOut, OUTPUT);
  pinMode(rxStatusOut, OUTPUT);
  Serial.begin(9600);
  // initialize our timers
  myTimer.SetInterval(TMR_PERIOD*myTimerIdx);
}

void loop() {
  // put your main code here, to run repeatedly:
  ReadAD();
  //CycleTimer();
  DoRelay();
  delay(CTRL_LOOP_PERIOD_MS);
}
