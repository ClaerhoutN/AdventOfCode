#include <iostream>
#include <stringUtilities.hpp>
#include <fileUtilities.h>
#include <tuple>
using namespace std;

int main() {

	vector<tuple<string, int, bool>> commands;
	for (string& instruction : split(readFile("../inputFiles/2020_8.txt"), '\n'))
	{
		string name = instruction.substr(0, 3);
		int amount = stoi(instruction.substr(4));
		commands.push_back(make_tuple(name, amount, false));
	}

	tuple<string, int, bool>* instructionPtr = &commands[0];
	int acc = 0;
	do
	{
		string name = get<0>(*instructionPtr);
		int amount = get<1>(*instructionPtr);
		get<2>(*instructionPtr) = true;
		if (name == "acc")
			acc += amount;
		if (name == "jmp")
			instructionPtr += amount;
		else
			++instructionPtr;
	} while (!get<2>(*instructionPtr));

	cout << acc << endl;
	cin.get();
	return 0;
}