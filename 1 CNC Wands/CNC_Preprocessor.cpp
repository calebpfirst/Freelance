/*
@Author: 		Caleb Price
@File Details:  CNC Pre-processor
@Date: 			6/29/2020
*/

#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <ctype.h>
#include <sstream>

using namespace std;


/* CONSTANT VALUES */
//const char* FILENAME		= "test.tap";			// change this to whatever your filename is next to the .exe
const char* OUTPUT_FILENAME = "output.tap";			// change this to whatever you would like the output filename to be.
const string StrLeadIn = "LEADIN";
const string StrLeadOut = "LEADOUT";

/* TYPE DEFINITIONS */
struct LineValues
{
	double y, z;
	bool hasy, hasz;
};

/* GLOBAL VARIABLES */
static vector<string> VectRawFile;

/* FUNCTION PROTOTYPES */
double RoundToThree(double value);
int GetLeadinRowIdx();
int GetLeadoutRowIdx();
bool IsLeadinRowIdx(int index);
bool IsLeadoutRowIdx(int index);
bool replace(std::string& str, const std::string& from, const std::string& to);
bool contains(string str, const string& from);
bool is_number(const char c);
void SetLineValues(int line_index, const LineValues line);
void GetLineValues(int line_index, LineValues &line, int charOffset = 0);
void LoadFile(const char* filename);
void LeadInLogic();
void LeadOutLogic();
void SaveOutputFile(const char* filename);

/* MAIN CODE */
int main()
{
	// 1) load file into memory.
	string input = "";
	cout << "Please local filename followed by Enter (no spaces): ";
	cin >> input;
	cout << "Loading file...\n";
	LoadFile(input.c_str());
	// 2) Apply the LeadIns.
	cout << "Applying LEADIN logic...\n";
	LeadInLogic();
	// 3) Apply the LeadOuts.
	cout << "Applying LEADOUT logic...\n";
	LeadOutLogic();
	// 4) Save output into a new file.
	cout << "Saving file...\n";
	SaveOutputFile(OUTPUT_FILENAME);
	cout << "...done\n";
	getchar();

	return 0;
}

/* FUNCTION DEFINITIONS */
void LeadOutLogic()
{
	const int LEADOUT_OFFSET = StrLeadOut.size();
	int leadOutIdx = GetLeadoutRowIdx();
	double Zorg = 0.0f;
	double Yorg = 0.0f;
	double Ys = 0.0f;
	double Zs = 0.0f;
	double Yp = 0.0f;
	bool hasLeadout = false;
	if (leadOutIdx >= 0)
	{
		for (int i = leadOutIdx; i >= 0; i--)
		{
			// go by each row...
			LineValues line = { 0 };
			// this is only to get the initial 'leadout' values..
			if (IsLeadoutRowIdx(i))
			{
				// initialization index for leadin
				GetLineValues(i, line, LEADOUT_OFFSET);
				Zorg = line.z;
				Yorg = line.y;
				leadOutIdx = i;
				Ys = 0.0f;
				Zs = 0.0f;
				Yp = 0.0f;
				hasLeadout = true;
			}
			else if (IsLeadinRowIdx(i))
			{
				// skip the line that says 'leadin'
			}
			else if (hasLeadout)
			{
				// everything else should go through this code path
				GetLineValues(i, line);
				if (line.hasy)
				{
					if (Ys == 0.0f)
					{
						// first value for Y.
						Ys = line.y;
					}
					else
					{
						Yp = line.y;
					}
					if ((Yp > 0) && (Ys > 0))
						if ((Yp - Yorg) >= (Ys))
							hasLeadout = false;
				}
				if (line.hasz)
				{
					if ((Ys == 0.0f) || (Yp == 0.0f))
					{
						// we haven't found a valid Y value yet.
						line.z = RoundToThree(line.z + Zorg);
					}
					else
					{
						// we do have a initial Y value...we should do the complex calculation for Z.
						line.z = RoundToThree(line.z + (Zorg * ((Yorg - (Yp - Ys)) / Yorg)));
					}
				}

			}
			//cout << "\nx=" << line.x << "y=" << line.y << "z=" << line.z;
			SetLineValues(i, line);
		}
	}
}
double RoundToThree(double value)
{
	return static_cast<double>(static_cast<int>(value*1000.0) / 1000.0);
}
void LeadInLogic()
{
	const int LEADIN_OFFSET = StrLeadIn.size();
	double Zorg = 0.0f;
	double Yorg = 0.0f;
	double Ys = 0.0f;
	double Zs = 0.0f;
	double Yp = 0.0f;
	int leadInIdx = GetLeadinRowIdx();
	bool hasLeadin = false;
	if (leadInIdx >= 0)
	{
		for (int i = leadInIdx; i < VectRawFile.size(); i++)
		{
			// go by each row...
			LineValues line = { 0 };
			// this is only to get the initial 'leadin' values..
			if (IsLeadinRowIdx(i))
			{
				// initialization index for leadin
				GetLineValues(i, line, LEADIN_OFFSET);
				Zorg = line.z;
				Yorg = line.y;
				leadInIdx = i;
				Ys = 0.0f;
				Zs = 0.0f;
				Yp = 0.0f;
				hasLeadin = true;
			}
			else if (IsLeadoutRowIdx(i))
			{
				// skip the line that says 'leadout'
			}
			else if (hasLeadin)
			{
				// everything else should go through this code path
				GetLineValues(i, line);
				if (line.hasy)
				{
					if (Ys == 0.0f)
					{
						// first value for Y.
						Ys = line.y;
					}
					else
					{
						Yp = line.y;
					}
					if ((Yp > 0)&&(Ys > 0))
						if (Yp <= (Ys - Yorg))
							hasLeadin = false;
				}
				if (line.hasz)
				{
					if ((Ys == 0.0f) || (Yp == 0.0f))
					{
						// we haven't found a valid Y value yet.
						line.z = RoundToThree(line.z + Zorg);
					}
					else
					{
						// we do have a initial Y value...we should do the complex calculation for Z.
						line.z = RoundToThree(line.z + (Zorg * ((Yorg - (Ys - Yp)) / Yorg)));
					}
				}

			}
			//cout << "\nx=" << line.x << "y=" << line.y << "z=" << line.z;
			SetLineValues(i, line);
		}
	}
}

void LoadFile(const char* filename)
{
	if (VectRawFile.size() > 0)
		VectRawFile.clear();
	fstream file;
	file.open(filename);
	if (file.is_open())
	{
		// keep reading until we reach the end of the file...
		string strLine = "";
		while (getline(file, strLine))
		{
			// read file line by line
			VectRawFile.push_back(strLine);
		}
	}
	file.close();
}

int GetLeadinRowIdx()
{
	if (VectRawFile.size() > 0)
	{
		for (int i = 0; i < VectRawFile.size(); ++i)
		{
			if (VectRawFile[i].size() >= 6)
			{
				string strRow = VectRawFile[i];
				if (contains(strRow, StrLeadIn))
				{

					return i;
				}
			}
		}
	}

	return -1;
}

int GetLeadoutRowIdx()
{
	if (VectRawFile.size() > 0)
	{
		for (int i = VectRawFile.size() - 1; i >= 0; --i)
		{
			if (VectRawFile[i].size() >= 6)
			{
				string strRow = VectRawFile[i];
				if (contains(strRow, StrLeadOut))
				{

					return i;
				}
			}
		}
	}

	return -1;
}

bool IsLeadinRowIdx(int index)
{
	if (VectRawFile.size() > index)
	{
		if (VectRawFile[index].size() >= 6)
		{
			string strRow = VectRawFile[index];
			if (contains(strRow, StrLeadIn))
			{

				return true;
			}
		}
	}

	return false;
}

bool IsLeadoutRowIdx(int index)
{
	if (VectRawFile.size() > index)
	{
		if (VectRawFile[index].size() >= 6)
		{
			string strRow = VectRawFile[index];
			if (contains(strRow, StrLeadOut))
			{

				return true;
			}
		}
	}

	return false;
}

bool is_number(const char c)
{
	string s(1, c);
	std::string::const_iterator it = s.begin();
	while (it != s.end() && isdigit(*it)) ++it;
	return !s.empty() && it == s.end();
}
bool replace(std::string& str, const std::string& from, const std::string& to) {
	size_t start_pos = str.find(from);
	if (start_pos == std::string::npos)
		return false;
	str.replace(start_pos, from.length(), to);
	return true;
}

bool contains(string str, const string& from)
{
	int start = str.size();
	replace(str, from, "");
	int end = str.size();

	return (start != end);
}

void SetLineValues(int line_index, const LineValues line)
{
	if (line_index < VectRawFile.size())
	{
		string strLine = VectRawFile[line_index];
		for (int i = 0; i < strLine.size(); i++)
		{
			string strValue = "";
			bool haveHeader = false;
			// looking for header mode...
			if (!contains(strLine, StrLeadIn) && !contains(strLine, StrLeadOut))
			{
				for (int j = i; j < strLine.size(); j++)
				{
					// we have the header, now we need the value...
					const char header = strLine[i];
					if (((header == 'Y')
						|| (header == 'Z')) && (!haveHeader))
					{
						haveHeader = true;
					}
					else if (haveHeader)
					{
						if ((is_number(strLine[j]) || (strLine[j] == '-') || (strLine[j] == '.')) && (j < strLine.size()))
						{
							strValue += strLine[j];
						}
						else
						{
							// replace old value with our new value...
							if ((((header == 'Y') && (line.hasy))
								|| ((header == 'Z') && (line.hasz))))
							{
								ostringstream ss;
								if (header == 'Y')
									ss << line.y;
								else
									ss << line.z;
								string newString(ss.str());
								newString = header + newString;
								strValue = header + strValue;
								replace(VectRawFile[line_index], strValue, newString);
							}
							strValue = "";
							haveHeader = false;
							i = j + 1;
						}
						if (j == strLine.size() - 1)
						{
							// replace old value with our new value...
							if ((((header == 'Y') && (line.hasy))
								|| ((header == 'Z') && (line.hasz))))
							{
								ostringstream ss;
								if (header == 'Y')
									ss << line.y;
								else
									ss << line.z;
								string newString(ss.str());
								newString = header + newString;
								strValue = header + strValue;
								replace(VectRawFile[line_index], strValue, newString);
							}
							strValue = "";
							haveHeader = false;
							i = j + 1;
						}
					}
				}
			}
		}
	}
}

void SaveOutputFile(const char* filename)
{
	ofstream file(filename);
	if (file.is_open())
	{
		// write all content of the vector...
		for (int line = 0; line < VectRawFile.size(); line++)
		{
			file << VectRawFile[line] << "\n";
		}
	}
	file.close();
}

void GetLineValues(int line_index, LineValues &line, int charOffset)
{
	line.hasy = false;
	line.hasz = false;
	if (line_index < VectRawFile.size())
	{
		string strLine = VectRawFile[line_index];
		// is this a first line?
		if (charOffset == 0)
		{
			// we have no business changing leadin or leadout values..
			if (contains(strLine, StrLeadIn) || contains(strLine, StrLeadOut))
				return;
		}
		for (int i = charOffset; i < strLine.size(); i++)
		{
			string strValue = "";
			bool haveHeader = false;
			char header = ' ';
			// looking for header mode...
			for (int j = i; j < strLine.size(); j++)
			{
				// we have the header, now we need the value...
				if (!haveHeader)
					header = strLine[j];
				if (((header == 'Y')
					|| (header == 'Z')) && (!haveHeader))
				{
					haveHeader = true;
				}
				else if (haveHeader)
				{
					if ((is_number(strLine[j]) || (strLine[j] == '-') || (strLine[j] == '.')) && (j < strLine.size()))
					{
						strValue += strLine[j];
					}
					else
					{
						// set number to corresponding structure value.
						if (header == 'Y')
						{
							sscanf(strValue.c_str(), "%lf", &line.y);
							line.hasy = true;
						}
						else if (header == 'Z')
						{
							sscanf(strValue.c_str(), "%lf", &line.z);
							if (line.z != 0.0)
							    line.hasz = true;
							else
								line.z = 0.0;
						}
						strValue = "";
						haveHeader = false;
						i = j + 1;
					}
					if (j == strLine.size() - 1)
					{
						// set number to corresponding structure value.
						if (header == 'Y')
						{
							sscanf(strValue.c_str(), "%lf", &line.y);
							line.hasy = true;
						}
						else if (header == 'Z')
						{
							sscanf(strValue.c_str(), "%lf", &line.z);
							if (line.z != 0.0)
							    line.hasz = true;
							else
								line.z = 0.0;
						}
						strValue = "";
						haveHeader = false;
						i = j + 1;
					}
				}
			}
		}
	}
}

