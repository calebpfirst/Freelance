// LogfileParser.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#define _CRT_SECURE_NO_WARNINGS

#include <iostream>
#include <string>
#include <fstream>
#include <windows.h>
#include <vector>

using namespace std;


string lastFileContents = "";

const string settingsFile = "C:/Temp/settings.cfg";
string logFile = "";
string exeFile = "";
bool showWindow = true;
vector<string> lstExpressions;

std::vector<string> Split(string str, const char delimiter)
{
    vector<string> output;
    string pointer = "";
    for (int i = 0; i < str.size(); ++i)
    {
        if (str[i] == delimiter)
        {
            if (pointer.size() > 0)
            {
                output.push_back(pointer);
                pointer = "";
            }
        }
        else if ((str[i] != '\n') &&
            (str[i] != '\r') &&
            (str[i] != '\t'))
        {
            pointer += str[i];
        }
    }
    if (pointer.size() > 0)
        output.push_back(pointer);


    return output;
}

bool to_bool(string value)
{
    return (value == "true") ? true : false;
}

bool Initialize()
{
    string str = "";
    streampos size;
    char* memblock;
    ifstream file(settingsFile, ios::in | ios::binary | ios::ate);
    if (file.is_open())
    {
        size = file.tellg();
        memblock = new char[size];
        file.seekg(0, ios::beg);
        file.read(memblock, size);
        file.close();
        for (int i = 0; i < size; ++i)
            str += memblock[i];
        delete[] memblock;

        vector<string> lines = Split(str, ';');
        vector<string> keywords = Split(lines[0], '=');
        lstExpressions = Split(keywords[1], ',');
        vector<string> delimTmp = Split(lines[1], '=');
        logFile = delimTmp[1];
        delimTmp = Split(lines[2], '=');
        exeFile = delimTmp[1];
        delimTmp = Split(lines[3], '=');

        showWindow = to_bool(delimTmp[1]);

        return true;
    }

    return false;
}

bool Contains(string str, string expr)
{
    return str.find(expr) != string::npos;
}

void RunExecutable(string str)
{
    system(str.c_str());
}

bool FileChanged(string str)
{
    return str.size() != lastFileContents.size();
}

void ReadFile()
{
    streampos size;
    string memblock = "";
    ifstream file(logFile);
    if (file.is_open())
    {
        string line = "";
        while (getline(file, line))
            memblock += line;
        file.close();
        if (FileChanged(memblock))
        {
            lastFileContents = memblock;
            for (int i = 0; i < lstExpressions.size(); ++i)
            {
                if (Contains(lastFileContents, lstExpressions[i]))
                {
                    // launch executable..
                    RunExecutable(exeFile);
                    break;
                }
            }
        }
    }
}

int main()
{
    HWND hWnd = GetConsoleWindow();
    ShowWindow(hWnd, SW_HIDE);
    if (Initialize())
    {
        if (showWindow)
            ShowWindow(hWnd, SW_SHOW);
        cout << "Initialized.\n";
        cout << "Listening to log file[" << logFile << "].\n";
        cout << "Ready to execute[" << logFile << "].\n";
        while (true)
        {
            ReadFile();
            Sleep(1000);
        }
    }
    else
    {

        ShowWindow(hWnd, SW_SHOW);
        cout << "Error: File in incorrect format.\n";
        cout << "File Format Example: \n";
        cout << "keyword=test,hello,goodbye;\nlogfile=c:/;\nexecutable=c:/;\n";
        cout << "show_window=false;";
        char tmp;
        cin >> tmp;
    }
}

// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
