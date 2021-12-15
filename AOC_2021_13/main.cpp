#include <stringUtilities.hpp>
#include <fileUtilities.h>
#include <vector>
#include <iostream>
#include <algorithm>

using namespace std;

void fold(vector<pair<int, int>>& const coords, int xLine, int yLine)
{
	for (auto coord = coords.begin(); coord != coords.end(); ++coord)
	{
		int x = get<0>(*coord), y = get<1>(*coord);
		if (xLine >= 0 && x > xLine)
			*coord = pair<int, int>(xLine - (x - xLine), y);
		else if (yLine >= 0 && y > yLine)
			*coord = pair<int, int>(x, yLine - (y - yLine));
	}
}

void print(const vector<pair<int, int>>& const coords, int fromX, int fromY, int toX, int toY)
{
	for (int y = fromY; y <= toY; ++y)
	{
		for (int x = fromX; x <= toX; ++x)
		{
			bool found = false;
			for (auto& coord : coords)
			{
				if (x == get<0>(coord) && y == get<1>(coord))
				{
					found = true;
					break;
				}
			}
			if (found)
				cout << "#";
			else
				cout << " ";
		}
		cout << endl;
	}
}

int countDistinctCoordinates(const vector<pair<int, int>>& const coords)
{
	int totalCount = 0;
	vector<pair<int, int>> iteratedCoords;
	for (auto& coord : coords)
	{
		bool found = false;
		for (auto& itCoord : iteratedCoords)
		{
			if (get<0>(coord) == get<0>(itCoord) && get<1>(coord) == get<1>(itCoord))
			{
				found = true;
				break;
			}
		}
		if (!found)
		{
			++totalCount;
			iteratedCoords.push_back(coord);
		}
	}
	return totalCount;
}

int main() {
	auto lines = split(readFile("../inputFiles/2021_13.txt"), '\n');
	vector<pair<int, int>> coords;
	int fromX = 0, fromY = 0, toX = 0, toY = 0;
	for (auto& line : lines)
	{
		if (line == "");
		else if (line.find("fold along ") != string::npos)
		{
			char x_y = line[11];
			int coord = stoi(line.substr(13, line.length()));
			if (x_y == 'x')
			{
				fold(coords, coord, -1);
				fromX = coord - (toX - coord);
				toX = coord - 1;
			}
			else
			{
				fold(coords, -1, coord);
				fromY = coord - (toY - coord);
				toY = coord - 1;
			}
		}
		else
		{
			auto splitted = split(line.c_str(), ',');
			int x = stoi(splitted[0]), y = stoi(splitted[1]);
			coords.push_back(pair<int, int>(x, y));
			toX = max(toX, x); toY = max(toY, y);
		}
	}

	int count = countDistinctCoordinates(coords);
	print(coords, fromX, fromY, toX, toY);

	cout << count;
	return 0;
}