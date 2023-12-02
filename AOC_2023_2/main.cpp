#include <stringUtilities.hpp>
#include <fileUtilities.h>
#include <unordered_map>
#include <string>
#include <vector>
#include <iostream>
#include <algorithm>
using namespace std;

struct Set {
	unordered_map<string, int> cubes;
	void addCube(string const & color, int amount)
	{
		cubes.insert({ color, amount });
	}
	int operator[](const char* color) {
		return cubes[color];
	}
};
struct Game {
	Game(vector<Set> const & sets) : _sets(sets)
	{}
	vector<Set> _sets;
	bool isGamePossible()
	{
		for (auto& set : _sets)
		{
			if (set["red"] > 12 || set["green"] > 13 || set["blue"] > 14)
				return false;
		}
		return true;
	}
	int getPower() 
	{
		int maxRed = 0, maxGreen = 0, maxBlue = 0;
		for (auto& set : _sets)
		{
			maxRed = max(maxRed, set["red"]);
			maxGreen = max(maxGreen, set["green"]);
			maxBlue = max(maxBlue, set["blue"]);
		}
		return maxRed * maxGreen * maxBlue;
	}
};
int main()
{
	auto lines = split(readFile("../inputFiles/2023_2.txt"), '\n');
	vector<Game> games;
	vector<Set> sets;
	for (auto& line : lines)
	{
		sets.clear();
		for (auto& strSet : split(line, ';', line.find(':') + 2))
		{
			Set set;
			for (string& strColor : split(strSet, ','))
			{
				auto t = split(ltrim_copy(strColor), ' ');
				set.addCube(t[1], stoi(t[0]));
			}
			sets.push_back(set);
		}
		games.push_back(Game{ sets });
	}

	int idSum = 0;
	int powerSum = 0;
	for (int i = 0; i < games.size(); ++i)
	{
		if (games[i].isGamePossible())
			idSum += i + 1;
		powerSum += games[i].getPower();
	}
	cout << idSum << endl;
	cout << powerSum << endl;
}