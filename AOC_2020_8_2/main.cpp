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

	int acc;
	for (auto& command : commands)
	{
		if (get<0>(command) == "acc") continue;
		string oldSwitch = get<0>(command);
		get<0>(command) = (oldSwitch == "nop" ? "jmp" : "nop");
		bool found = false;
		auto* instructionPtr = &commands.front();
		acc = 0;

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
			if (instructionPtr == &commands.back())
				found = true;
		} while (!found 
			&& instructionPtr >= &commands.front() 
			&& instructionPtr <= &commands.back() 
			&& !get<2>(*instructionPtr));
		
		get<0>(command) = oldSwitch;
		for (auto& c : commands)
		{
			get<2>(c) = false;
		}
		if (found)
			break;
	}

	cout << acc << endl;
	cin.get();
	return 0;
}