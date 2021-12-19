#include <stringUtilities.hpp>
#include <fileUtilities.h>
#include <iostream>

using namespace std;

inline int getFromFilledBits(int n)
{
	int j = 0;
	for (int i = 0; i < n; ++i)
		j = (j << 1) + 1;
	return j;
}

//bitOffset = 0-based
inline int readNBits(string& const input, int bitOffset, int bits)
{
	int lowerBit = bitOffset / 4 * 4;
	int upperBit = (bitOffset + bits - 1) / 4 * 4 + 4;
	string substr = input.substr(lowerBit / 4, (upperBit - lowerBit) / 4);
	int i = stoi(substr, nullptr, 16);
	int right = upperBit - (bitOffset + bits);
	return (i >> right) & getFromFilledBits(bits);
}
unsigned long long readPacket(string& const input, int& bitOffset);
unsigned long long aggregatePacketExpressionResults(unsigned long long r1, unsigned long long r2, int typeID)
{
	switch (typeID)
	{
	case 0:
		return r1 + r2;
	case 1:
		return r1 * r2;
	case 2:
		return min(r1, r2);
	case 3:
		return max(r1, r2);
	case 5:
		return r1 > r2 ? 1 : 0;
	case 6:
		return r1 < r2 ? 1 : 0;
	case 7:
		return r1 == r2 ? 1 : 0;
	}
}
//the next 15 bits are a number that represents the total length in bits of the sub-packets contained by this packet.
inline unsigned long long readOperator_length(string& const input, int& bitOffset, int typeID)
{
	int subPacketsLength = readNBits(input, bitOffset, 15);
	bitOffset += 15;
	int localBitOffset = bitOffset;

	unsigned long long packetExpressionResult = readPacket(input, localBitOffset);
	while (localBitOffset - bitOffset < subPacketsLength)
		packetExpressionResult = aggregatePacketExpressionResults(packetExpressionResult, readPacket(input, localBitOffset), typeID);

	bitOffset = localBitOffset;
	return packetExpressionResult;
}
//the next 11 bits are a number that represents the number of sub-packets immediately contained by this packet.
inline unsigned long long readOperator_number(string& const input, int& bitOffset, int typeID)
{
	int subPacketCount = readNBits(input, bitOffset, 11);
	bitOffset += 11;

	unsigned long long packetExpressionResult = readPacket(input, bitOffset);
	for (int i = 1; i < subPacketCount; ++i)
		packetExpressionResult = aggregatePacketExpressionResults(packetExpressionResult, readPacket(input, bitOffset), typeID);

	return packetExpressionResult;
}
inline unsigned long long readOperatorPacket(string& const input, int& bitOffset, int typeID) {
	unsigned long long packetExpressionResult = 0;
	int lengthTypeId = readNBits(input, bitOffset, 1);
	++bitOffset;
	switch (lengthTypeId)
	{
	case 0:
		packetExpressionResult = readOperator_length(input, bitOffset, typeID);
		break;
	case 1:
		packetExpressionResult = readOperator_number(input, bitOffset, typeID);
		break;
	}
	return packetExpressionResult;
}
inline unsigned long long readLiteralValue(string& const input, int& bitOffset)
{
	int isNotLastGroupDigit;
	int bitsRead = 0;
	unsigned long long packetExpressionResult = 0;
	do
	{
		isNotLastGroupDigit = readNBits(input, bitOffset + bitsRead, 1);
		packetExpressionResult = (packetExpressionResult << 4) + readNBits(input, bitOffset + bitsRead + 1, 4);
		bitsRead += 5;
	} 	while (isNotLastGroupDigit);
	bitOffset += bitsRead;
	return packetExpressionResult;
}
unsigned long long readPacket(string& const input, int& bitOffset)
{
	unsigned long long packetExpressionResult = 0;
	readNBits(input, bitOffset, 3);
	int packetTypeId = readNBits(input, bitOffset + 3, 3);
	int localBitOffset = bitOffset + 6;
	switch (packetTypeId)
	{
	case 4:
		packetExpressionResult = readLiteralValue(input, localBitOffset);
		break;
	default:
		packetExpressionResult = readOperatorPacket(input, localBitOffset, packetTypeId);
		break;
	}
	bitOffset = localBitOffset;
	return packetExpressionResult;
}
unsigned long long readPacket(string& const input)
{
	int bitOffset = 0;
	return readPacket(input, bitOffset);
}

int main() {
	string line = readFile("../inputFiles/2021_16.txt");
	unsigned long long packetExpressionResult = readPacket(line);
	cout << packetExpressionResult;
	return 0;
}