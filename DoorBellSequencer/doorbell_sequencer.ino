/* Doorbell Sequencer */
/* 2 input pins */
/* 4 output pins */

// these are all of our outputs, 'enum' is like saying, you can't change the value later....
enum class TYPE_OUTPUTS
{
  OUTPUT_RELAY1 = 4,
  OUTPUT_RELAY2 = 5,
  OUTPUT_RELAY3 = 6,
  OUTPUT_RELAY4 = 7
};
// these are all of our inputs
enum class TYPE_INPUTS
{
  INPUT_BTN_FRONT = 8,
  INPUT_BTN_BACK = 9
};
// use this to define how long the arduino should wait in between chimes in milliseconds.
#define SEQUENCE_DELAY_MS (int)800
// use this to define how many relays that we have.
#define SEQUENCE_SIZE     (int)4
// this is an array of chimes that the arduino will perform in order for the front door.
TYPE_OUTPUTS sequence1[SEQUENCE_SIZE] = { TYPE_OUTPUTS::OUTPUT_RELAY1, 
                                          TYPE_OUTPUTS::OUTPUT_RELAY2, 
                                          TYPE_OUTPUTS::OUTPUT_RELAY3, 
                                          TYPE_OUTPUTS::OUTPUT_RELAY4 };
// this is also an array of chimes that the arduino will perform in order but for the back door.
TYPE_OUTPUTS sequence2[SEQUENCE_SIZE] = { TYPE_OUTPUTS::OUTPUT_RELAY4, 
                                          TYPE_OUTPUTS::OUTPUT_RELAY3, 
                                          TYPE_OUTPUTS::OUTPUT_RELAY2, 
                                          TYPE_OUTPUTS::OUTPUT_RELAY1 };

// this is a pointer, we use this to cut down on code..if the back door is pressed,
// it is set to the back door sequence, if the front door is pressed, it is set to the back door.
TYPE_OUTPUTS sequence_ptr[SEQUENCE_SIZE] = {};
// The sequence index is the current relay the arduino is processing.
int sequence_idx = 0;
// the sequence triggered flag is true whenever the arduino should start processing,
// relays, it is false whenever it should be reading button presses.
bool sequence_triggered = false;
// this 'SetRelay' function will set all relays to off except the one we want,
// this is so they don't all chime at once. 
static void SetRelaysOff()
{
  for (int i = (int)TYPE_OUTPUTS::OUTPUT_RELAY1; i <= (int)TYPE_OUTPUTS::OUTPUT_RELAY4; ++i)
  {
      digitalWrite(i, LOW);
      delay(5);
  }
}       
static void SetRelay(TYPE_OUTPUTS relay)
{
  digitalWrite((int)relay, HIGH);
  delay(5);
}
// check if the front button is pressed
static bool GetFrontBtnPressed()
{
  int buttonstate = digitalRead((int)TYPE_INPUTS::INPUT_BTN_FRONT);

  return buttonstate == HIGH;
}
// check if the back button is pressed
static bool GetBackBtnPressed()
{
  int buttonstate = digitalRead((int)TYPE_INPUTS::INPUT_BTN_BACK);

  return buttonstate == HIGH;
}
// initialize our pin inputs/outputs
void setup() {
  // put your setup code here, to run once:
  pinMode((int)TYPE_OUTPUTS::OUTPUT_RELAY1, OUTPUT);
  pinMode((int)TYPE_OUTPUTS::OUTPUT_RELAY2, OUTPUT);
  pinMode((int)TYPE_OUTPUTS::OUTPUT_RELAY3, OUTPUT);
  pinMode((int)TYPE_OUTPUTS::OUTPUT_RELAY4, OUTPUT);
  pinMode((int)TYPE_INPUTS::INPUT_BTN_FRONT, INPUT);
  pinMode((int)TYPE_INPUTS::INPUT_BTN_BACK, INPUT);
  Serial.begin(9600);
  SetRelaysOff();
}
// this function keeps looping forever
void loop() {
  // put your main code here, to run repeatedly:
  // check for button press, don't do anything until there is one
  if (!sequence_triggered)
  {
    // check for button presses if there hasn't been
    if (GetFrontBtnPressed())
    {
      // front button pressed..
      memcpy(&sequence_ptr[0], &sequence1[0], sizeof(TYPE_OUTPUTS) * SEQUENCE_SIZE);
      sequence_triggered = true;
      Serial.println("Front Door Pressed\n");
    }
    if (GetBackBtnPressed())
    {
      memcpy(&sequence_ptr[0], &sequence2[0], sizeof(TYPE_OUTPUTS) * SEQUENCE_SIZE);
      sequence_triggered = true;
      Serial.println("Back Door Pressed\n");
    }
    // adding a generic delay in there can help the arduino not get bogged down.
    delay(1);
  }
  else
  {
    // if there has been a button press, we need to execute it
    SetRelay(sequence_ptr[sequence_idx]);
    if (++sequence_idx >= SEQUENCE_SIZE)
    {
        sequence_idx = 0;
        sequence_triggered = false;
        // let dereference our pointer variable (for safety) so that we can set it again later.
    }
    // delay by the amount specified at the top of the file.
    delay(SEQUENCE_DELAY_MS);
    SetRelaysOff();
  }
}
