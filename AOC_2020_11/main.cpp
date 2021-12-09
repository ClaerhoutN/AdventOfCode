#include "input.hpp"
#include <iostream>
#include <stringUtilities.hpp>
#include <Windows.h>
using namespace std;

int gridWidth;
int gridHeight;

bool _t(char c, int& count) {
	if(c == '#')
		++count;
	if (c == '#' || c == 'L')
	{
		return true;
	}
	return false;
}

int countOccupiedAdjacentInSight(int row, int col,
	const char* const* const referenceGrid, int range = -1)
{
	int count = 0;
	bool tl = false, bl = false, l = false, tr = false, br = false, r = false, t = false, b = false;
	int i = 1;
	while (i <= (range != -1 ? range : max(gridWidth, gridHeight)) && (!tl || !bl || !l || !tr || !br || !r || !t || !b))
	{
		if (col >= i)
		{
			if (!tl && row >= i) //tl
				tl = _t(referenceGrid[row - i][col - i], count);
			if (!bl && row < gridHeight - i) //bl
				bl = _t(referenceGrid[row + i][col - i], count);
			if (!l) //l
				l = _t(referenceGrid[row][col - i], count);
		}
		if (col < gridWidth - i)
		{
			if (!tr && row >= i) //tr
				tr = _t(referenceGrid[row - i][col + i], count);
			if (!br && row < gridHeight - i) //br
				br = _t(referenceGrid[row + i][col + i], count);
			if (!r) //r
				r = _t(referenceGrid[row][col + i], count);
		}
		if (!t && row >= i) //t
			t = _t(referenceGrid[row - i][col], count);
		if (!b && row < gridHeight - i) //b
			b = _t(referenceGrid[row + i][col], count);

		++i;
	}
	return count;

}

int countOccupiedAdjacent(int row, int col,
	const char* const* const referenceGrid)
{
	return countOccupiedAdjacentInSight(row, col, referenceGrid, -1);
}
bool shouldOccupy(
	int row, int col, 
	const char* const* const referenceGrid) {
	return countOccupiedAdjacent(row, col, referenceGrid) == 0;
}
bool shouldMove(
	int row, int col, 
	const char* const* const referenceGrid) {
	return countOccupiedAdjacent(row, col, referenceGrid) >= 5;
}
void deleteGrid(const char * const * const grid)
{
	for (int i = 0; i < gridHeight; ++i)
	{
		delete[] grid[i];
	}
	delete[] grid;
}
char * const * const copyGrid(const char * const * const grid)
{
	char** const newGrid = new char* [gridHeight];
	for (int i = 0; i < gridHeight; ++i)
	{
		newGrid[i] = new char[gridWidth];
		memcpy(&newGrid[i][0], &grid[i][0], gridWidth);
	}
	return newGrid;
}

void printGrid(const char* const* const grid, int callCount, int changeCount) {
	for (int row = 0; row < gridHeight; ++row)
	{
		for (int col = 0; col < gridWidth; ++col)
		{
			cout << grid[row][col];
		}
		cout << endl;
	}
	cout << "---rendered " << callCount << "nd/th call to executeRound with " << changeCount << " changes---";
	cout << endl << endl;
}

bool executeRound(char * const * const grid)
{
	bool changed = false;
	const char * const * const referenceGrid = copyGrid(grid);
	int changeCount = 0;
	for (int row = 0; row < gridHeight; ++row)
	{
		for (int col = 0; col < gridWidth; ++col)
		{
			if (grid[row][col] == '#' && shouldMove(row, col, referenceGrid))
			{
				grid[row][col] = 'L';
				changed = true;
				++changeCount;
			}
			else if (grid[row][col] == 'L' && shouldOccupy(row, col, referenceGrid))
			{
				grid[row][col] = '#';
				changed = true;
				++changeCount;
			}
		}
	}
	deleteGrid(referenceGrid);

	static unsigned long long callCnt = 1;
	printGrid(grid, callCnt, changeCount);
	++callCnt;

	return changed;
}

int main() {
	auto lines = split(input, '\n');
	gridWidth = static_cast<int>(lines[0].length());
	gridHeight = static_cast<int>(lines.size());
	char** const grid = new char*[gridHeight];
	for (int i = 0; i < gridHeight; ++i)
	{
		grid[i] = const_cast<char*>(lines[i].c_str());
	}

	while (executeRound(grid));

	int occupiedCount = 0;
	for (int row = 0; row < gridHeight; ++row)
	{
		for (int col = 0; col < gridWidth; ++col)
		{
			if (grid[row][col] == '#')
				++occupiedCount;
		}
	}

	cout << occupiedCount << endl;
	cin.get();
	delete[] grid;
	return 0;
}