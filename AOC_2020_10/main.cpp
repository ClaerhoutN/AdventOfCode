#include "input.h"
#include <iostream>
#include <algorithm>
#include <stringUtilities.h>
using namespace std;

void AddJoltageInChain(vector<int>& const joltages, int fromIndex, vector<int>& const joltageDifferences, bool all = true)
{
	if (fromIndex +1 >= joltages.size()) return;
	if (all)
	{
		joltageDifferences[fromIndex] = joltages[fromIndex + 1] - joltages[fromIndex];
		AddJoltageInChain(joltages, fromIndex + 1, joltageDifferences);
	}
}

int main() {

	vector<int> joltages { 0 };
	for (string& joltage : split(input, '\n'))
	{
		joltages.push_back(stoi(joltage));
	}
	sort(joltages.begin(), joltages.end());
	joltages.push_back(joltages.back() + 3);

	vector<int> joltageDifferences(static_cast<size_t>(joltages.size() - 1));	
	AddJoltageInChain(joltages, 0, joltageDifferences);

	int count1j = count_if(joltageDifferences.begin(), joltageDifferences.end(), [](int j){return j == 1;});
	int count3j = count_if(joltageDifferences.begin(), joltageDifferences.end(), [](int j){return j == 3;});

	cout << count1j * count3j << endl;
	cin.get();
	return 0;
}