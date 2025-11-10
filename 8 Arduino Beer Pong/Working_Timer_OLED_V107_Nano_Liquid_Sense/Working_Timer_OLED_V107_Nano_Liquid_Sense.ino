#include <StopWatch.h>
#include <U8glib.h>

// create a new variable name (it is an unsigned long but shortened because we will use it a lot)
typedef unsigned long ULONG;
// a class is a variable that we define like a car that has many parts (wheels, doors, etc)
class SysTimer
{
  // we can call functions that are public from outside of the class.
  public:
  // we set default values for our variables in the constructor.
  // A constructor is the same name of the class. It should be called before we use the variable.
  SysTimer(): seconds(0), setseconds(0), delayset(false)
  {
    
  }
  // this is the destructor, we clear our variables after using the variable.
  // Destructors are there to free up memory.
  ~SysTimer()
  {
    setseconds = 0;
    seconds = 0;
    delayset = false;
  }
  // millis() is the current time in seconds from the arduino's clock.
  // Callback() will update the current time variable whenever it is called.
  void Callback()
  {
    seconds = millis();
  }
  // call SetDelay() to set the timespan until we do something.
  void SetDelay(ULONG timeMS)
  {
    delayset = true;
    this->Callback();
    setseconds = seconds + timeMS;
  }
  // this simply tells us if the delay has been set.
  bool DelaySet()
  {
    return delayset;
  }
  // this will tell us if the timespan has elapsed.
  bool DelayUp()
  {
    if (delayset)
    {
      Callback();
      if (seconds >= setseconds)
      {
        delayset = false;
        
        return true;
      }
    }

    return false;
  }
  private:
  ULONG seconds, setseconds;
  bool delayset;
};

// create a new variable of the class that we created above.
SysTimer *Timer = 0;

int startButton = 6;
int liquidPin = 3;
int resetButton = 8;

int greenPin = 4;
int redPin = 5;
int buzzPin = 7;

bool swRun;
float swtime;
int stopDelay;

int prevStartState=LOW;

StopWatch sw_millis;                                               // MILLIS (default)
StopWatch sw_secs(StopWatch::SECONDS);

U8GLIB_SH1106_128X64 u8g(12, 11, 10, 9);                           // CLK = 12, MOSI = 11, CS = 10, DC = 9

void setup() {
  // call the new constructor to set all of the internal variables.
  Timer = new SysTimer();
  Serial.begin(9600);
  pinMode(startButton, INPUT_PULLUP);                                
  pinMode(liquidPin, INPUT_PULLUP);
  pinMode(resetButton, INPUT_PULLUP);
  pinMode(buzzPin, OUTPUT);
  pinMode(greenPin, OUTPUT);
  pinMode(redPin, OUTPUT);
}

void draw(void) {                                                               
  u8g.setRot180();                                                 // flip screen, if required:
  u8g.setFont(u8g_font_fur17);                                    //OLED Set font:                 
  u8g.drawStr(32, 25, "Time:");
 
  if (swtime < 10) {
  u8g.setPrintPos(39, 55);
  }
  else u8g.setPrintPos(32, 55);
  u8g.println(swtime);                                            //OLED elapsed time in seconds:ms format:
    
}

// this is a new global function that I created to replace the 'Delay()' that was used before.
void StopDelay()
{
  // set the timer to run until the stopDelay time has elapsed.
  Timer->SetDelay(stopDelay);
  // Run the LED matrix events and keep checking the time.
  while (!Timer->DelayUp())
  {
    swtime=sw_millis.elapsed() / 1000.0, 2;
    u8g.firstPage();                                                        // Put information on OLED
    do {
      draw();                                                             //Refer to Void draw void
    } while (u8g.nextPage());
  }
}

void loop() {

swtime=sw_millis.elapsed() / 1000.0, 2;
stopDelay=sw_millis.elapsed() /.90 - sw_millis.elapsed();

  
if (digitalRead(startButton) == HIGH && prevStartState == LOW && digitalRead(liquidPin) == LOW) {    //START TIMER
    digitalWrite(greenPin, HIGH);
    digitalWrite(redPin, LOW);
    sw_millis.start();
    swRun=true;
        
}

if (digitalRead(startButton) == LOW || HIGH){
    prevStartState=digitalRead(startButton); 
}

u8g.firstPage();                                                        // Put information on OLED
  do {
    draw();                                                             //Refer to Void draw void
  } while (u8g.nextPage());


if (digitalRead(liquidPin) == HIGH) {                                   //LIQUID at 90%
    //delay(stopDelay);                                                   //stopDelay=sw_millis.elapsed() /.90 - sw_millis.elapsed()
    // added the StopDelay() function in place of 'delay()'
    StopDelay();
    sw_millis.stop();
    digitalWrite(greenPin, LOW);
    digitalWrite(redPin, HIGH);
    swRun=false;
  } 
  

if (digitalRead(resetButton) == LOW) {                                  //RESET TIMER
    sw_millis.reset();
    swRun=false;
    digitalWrite(redPin, LOW);
    digitalWrite(greenPin, LOW);
    tone(buzzPin, 1000, 100); 
    delay(500);
    
}


if (swRun == true){                                               //turn buzzer on if swRun (Stopwatch run) = true;
    tone(buzzPin, 100, 10);   
}



if (digitalRead(startButton) == HIGH && digitalRead(liquidPin) == LOW && swRun == false) {
    
    digitalWrite(redPin, HIGH);
    digitalWrite(greenPin, HIGH);
}
    else if (digitalRead(startButton) == LOW && digitalRead(liquidPin) == LOW && swRun == false) {
      digitalWrite(redPin, LOW);
      digitalWrite(greenPin, LOW);
    }
    

  delay(1);
}
