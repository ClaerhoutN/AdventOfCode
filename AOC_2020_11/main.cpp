#include "input.h"
#include <iostream>
#include <stringUtilities.h>
#include <tuple>
using namespace std;

size_t gridWidth;
size_t gridHeight;


int countOccupiedAdjacent(int row, int col,
	const char* const* const referenceGrid)
{
	int count = 0;
	if (col != 0)
	{
		if (row != 0 && referenceGrid[row - 1][col - 1] == '#')
			++count;
		if (row != gridHeight - 1 && referenceGrid[row + 1][col - 1] == '#')
			++count;
		if (referenceGrid[row][col - 1] == '#')
			++count;
	}
	if (col != gridWidth - 1)
	{
		if (row != 0 && referenceGrid[row - 1][col + 1] == '#')
			++count;
		if (row != gridHeight - 1 && referenceGrid[row + 1][col + 1] == '#')
			++count;
		if (referenceGrid[row][col + 1] == '#')
			++count;
	}
	if (row != 0 && referenceGrid[row - 1][col] == '#')
		++count;
	if (row != gridHeight - 1 && referenceGrid[row + 1][col] == '#')
		++count;
	return count;

}
bool shouldOccupy(
	int row, int col, 
	const char* const* const referenceGrid) {
	return countOccupiedAdjacent(row, col, referenceGrid) == 0;
}
bool shouldMove(
	int row, int col, 
	const char* const* const referenceGrid) {
	return countOccupiedAdjacent(row, col, referenceGrid) >= 4;
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

bool executeRound(char * const * const grid)
{
	bool changed = false;
	const char * const * const referenceGrid = copyGrid(grid);
	for (int row = 0; row < gridHeight; ++row)
	{
		for (int col = 0; col < gridWidth; ++col)
		{
			if (grid[row][col] == '#' && shouldMove(row, col, referenceGrid))
			{
				grid[row][col] = 'L';
				changed = true;
			}
			else if (grid[row][col] == 'L' && shouldOccupy(row, col, referenceGrid))
			{
				grid[row][col] = '#';
				changed = true;
			}
		}
	}
	deleteGrid(referenceGrid);
	return changed;
}

int main() {
	auto lines = split(input, '\n');
	gridWidth = lines[0].length();
	gridHeight = lines.size();
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