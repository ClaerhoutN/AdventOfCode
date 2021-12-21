#include <stringUtilities.hpp>
#include <fileUtilities.h>
#include <iostream>
#include <bitset>
#include <array>
#include <algorithm>

using namespace std;

template <size_t size>
size_t getImageEnhancementBitsetIndex(const vector<bitset<size>>& const image, int x, int y, int iteration)
{
	size_t index = 0;
	for (int i = -1; i <= 1; ++i) //y
	{
		for (int j = -1; j <= 1; ++j) //x
		{
			index = index << 1;
			if (y + i >= 0 && y + i < image.size() && x + j >= 0 && x + j < size)
				index |= static_cast<int>(image[y + i][x + j]);
			else if (iteration % 2 != 0)
				index |= 1;
		}
	}
	return index;
}

template <size_t size>
void print(vector<bitset<size>>& const image)
{
	for (bitset<size>& line : image)
	{
		string t = line.to_string('.', '#');
		reverse(t.begin(), t.end());
		cout << t << endl;
	}
	cout << endl;
}

template <size_t size>
vector<bitset<size+2>> enhanceImage(const vector<bitset<size>>& const image, const bitset<512>& const imgEnhancementBitset, int iteration)
{
	vector <bitset<size+2>> newImage(image.size() + 2);
	for (int y = 0; y <= size+1; ++y)
	{
		bitset<size + 2> newBitset;
		for (int x = 0; x <= size+1; ++x)
		{
			size_t bitsetIndex = getImageEnhancementBitsetIndex(image, x-1, y-1, iteration);
			newBitset.set(x, imgEnhancementBitset[bitsetIndex]);
		}
		newImage[y] = std::move(newBitset);
	}
	print(newImage);
	return newImage;
}

template <size_t size>
int getLitPixelCount(const vector<bitset<size>>& const image)
{
	int count = 0;
	for (auto& pixelLine : image)
	{
		count += pixelLine.count();
	}
	return count;
}


int main() {
	auto lines = split(readFile("../inputFiles/2021_20.txt"), '\n');

	constexpr int size = 100;

	reverse(lines[0].begin(), lines[0].end());
	const bitset<512> imgEnhancementBitset{ lines[0], 0, string::npos, '.', '#' };
	vector <bitset<size>> image;
	image.reserve(size);
	for (auto lineIt = lines.begin() + 2; lineIt != lines.end(); ++lineIt)
	{
		reverse(lineIt->begin(), lineIt->end());
		image.push_back(bitset<size>{ *lineIt, 0, string::npos, '.', '#' });
	}

	int iteration = 0;
	auto newImg = enhanceImage(image, imgEnhancementBitset, iteration++);
	auto newImg2 = enhanceImage(newImg, imgEnhancementBitset, iteration++);
	cout << getLitPixelCount(newImg2);

	return 0;
}