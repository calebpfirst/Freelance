# -*- coding: utf-8 -*-
"""
Virtual Journal 7/27/2020
Caleb Price

"""

# These are our variables to store input text for various things.
strTextLine = ''
strScore = '1'
strInput = 'y'
# This is the name of the file to output.
# If you want to change where it is located, you can specify "C:/folder/scvoutput.csv"
MYFILENAME = "csvoutout.csv"
# Open the file. Because I specified 'w', it will overwrite the file if it exists and
# it will create the file if it doesn't exist.
file = open(MYFILENAME, "w")
# Literally write the text below into the file as the first line.
file.write("SCORE,COMMENT")
# Closing the file is the best thing to do after you are done with it.
file.close()
# while loops will keep the program running until a condition is met.
# In this case, if the user inputs 'n', it stops running the while loop and the
# subsequent program.
while (strInput != "n"):
    # Output this text.
    print("\n\nHow are you feeling today (1-10)?: ")
    # Read input from the console into the variable 'strScore'.
    strScore = input()
    # If statements are conditional, we want to make sure that the user input a number
    # Thus, we check if it is numeric, if it is, then we continue.
    if (strScore.isnumeric() == True):
        print("\nOkay, now tell me how you are feeling in a sentence.: ")
        strTextLine = input();
        print("\nWould you like to continue(y/n)?: ")
        strInput = input();
        # The important change here is the 'a'. This means to append the text to the end of the file.
        file = open(MYFILENAME, "a")
        file.write("\n" + strScore + "," + strTextLine)
        file.close()
    else:
        # print this output if the user didn't input a valid number.
        print("\nError, please input a valid number")
        
print("\nProgram Over.")