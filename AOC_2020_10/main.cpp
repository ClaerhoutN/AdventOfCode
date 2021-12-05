#include "input.h"
#include <iostream>
#include <algorithm>
#include <unordered_map>
#include <stringUtilities.h>
using namespace std;

//recursion for no good reason :)
void addJoltageDiffInChain(vector<int>& const joltages, int fromIndex, vector<int>& const joltageDifferences)
{
	if (fromIndex +1 >= joltages.size()) return;
	joltageDifferences[fromIndex] = joltages[fromIndex + 1] - joltages[fromIndex];
	addJoltageDiffInChain(joltages, fromIndex + 1, joltageDifferences);
}

unsigned long long countAdapterChains(vector<int>& const joltages, int fromIndex, int lastJoltage)
{
	static unordered_map<int, unsigned long long> cacheByLastJoltage;
	if (fromIndex == joltages.size() - 1)
		return 1;

	unsigned long long count = 0;
	for (int jIndex = fromIndex; jIndex < joltages.size(); ++jIndex)
	{
		if (joltages[jIndex] - lastJoltage <= 3)
		{
			if (cacheByLastJoltage.find(joltages[jIndex]) != cacheByLastJoltage.end())
				count += cacheByLastJoltage[joltages[jIndex]];
			else if (jIndex == joltages.size() - 1)
				++count;
			else
				count += countAdapterChains(joltages, jIndex + 1, joltages[jIndex]);
		}
		else
			break;
	}
	cacheByLastJoltage[lastJoltage] = count;
	return count;
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
	addJoltageDiffInChain(joltages, 0, joltageDifferences);
	int count1j = count_if(joltageDifferences.begin(), joltageDifferences.end(), [](int j){return j == 1;});
	int count3j = count_if(joltageDifferences.begin(), joltageDifferences.end(), [](int j){return j == 3;});

	unsigned long long adapterChainCnt = countAdapterChains(joltages, 1, 0);

	cout << "part 1: " << count1j * count3j << endl;
	cout << "part 2: " << adapterChainCnt << endl;
	cin.get();
	return 0;
}