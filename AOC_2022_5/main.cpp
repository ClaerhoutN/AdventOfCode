#include <stringUtilities.hpp>
#include <fileUtilities.h>
#include <vector>
#include <iostream>
#include <array>
#include <deque>
#include <stack>
#include <algorithm>

using namespace std;

int main() {
	auto lines = split(readFile("../inputFiles/2022_5.txt"), '\n');
	int lineIndex = 0;
	array<deque<string>, 9> crateQueues;
	for (auto& line : lines)
	{
		if (lineIndex <= 7)
		{
			line = line + " ";
			for (int crateQueueIndex = 0; crateQueueIndex < crateQueues.size(); ++crateQueueIndex)
			{
			   string crate = line.substr(crateQueueIndex * 4, 4);
			   if(crate.find(' ') != 0)
				   crateQueues[crateQueueIndex].push_front(crate);
			}
		}
		else if (lineIndex >= 10)
		{
			auto splittedLine = split(line, ' ');
			int count = stoi(splittedLine[1]);
			int from = stoi(splittedLine[3]) - 1;
			int to = stoi(splittedLine[5]) - 1;

			//part 1
			/*for (int crateMoveIteration = 0; crateMoveIteration < count; ++crateMoveIteration)
			{
				auto crate = crateQueues[from].back(); crateQueues[from].pop_back();
				crateQueues[to].push_back(crate);
			}*/
			//part 2
			stack<string> crateStackToMove;
			for (int crateMoveIteration = 0; crateMoveIteration < count; ++crateMoveIteration)
			{
				auto crate = crateQueues[from].back(); crateQueues[from].pop_back();
				crateStackToMove.push(crate);
			}
			for (int cratemoveIteration = 0; cratemoveIteration < count; ++cratemoveIteration)
			{
				auto crate = crateStackToMove.top(); crateStackToMove.pop();
				crateQueues[to].push_back(crate);
			}
		}
		++lineIndex;
	}
	cout << crateQueues[0].back() << crateQueues[1].back() << crateQueues[2].back() << crateQueues[3].back() << crateQueues[4].back() << crateQueues[5].back() << crateQueues[6].back() << crateQueues[7].back() << crateQueues[8].back();
}