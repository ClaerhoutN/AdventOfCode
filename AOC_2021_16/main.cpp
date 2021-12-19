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
	string substr = input.substr(lowerBit/4, (upperBit - lowerBit)/4);
	int i = stoi(substr, nullptr, 16);
	int right = upperBit - (bitOffset + bits);
	return (i >> right) & getFromFilledBits(bits);
}
int readPacket(string& const input, int& bitOffset, bool pad);
//the next 15 bits are a number that represents the total length in bits of the sub-packets contained by this packet.
inline int readOperator_length(string& const input, int& bitOffset)
{
	int packetVersionSum = 0;
	int subPacketsLength = readNBits(input, bitOffset, 15);
	bitOffset += 15;
	int localBitOffset = bitOffset;
	while (localBitOffset - bitOffset < subPacketsLength)
		packetVersionSum += readPacket(input, localBitOffset, false);
	bitOffset = localBitOffset;
	return packetVersionSum;
}
//the next 11 bits are a number that represents the number of sub-packets immediately contained by this packet.
inline int readOperator_number(string& const input, int& bitOffset)
{
	int packetVersionSum = 0;
	int subPacketCount = readNBits(input, bitOffset, 11);
	bitOffset += 11;
	for(int i = 0; i < subPacketCount; ++i)
		packetVersionSum += readPacket(input, bitOffset, false);
	return packetVersionSum;
}
inline int readOperatorPacket(string& const input, int& bitOffset) {
	int packetVersionSum = 0;
	int lengthTypeId = readNBits(input, bitOffset, 1);
	++bitOffset;
	switch (lengthTypeId)
	{
	case 0:
		packetVersionSum += readOperator_length(input, bitOffset);
		break;
	case 1:
		packetVersionSum += readOperator_number(input, bitOffset);
		break;
	}
	return packetVersionSum;
}
inline void readLiteralValue(string& const input, int& bitOffset) 
{
	int isNotLastGroupDigit;
	int bitsRead = 0;
	do
	{
		isNotLastGroupDigit = readNBits(input, bitOffset + bitsRead, 1);
		readNBits(input, bitOffset + bitsRead + 1, 4);
		bitsRead += 5;
	}
	while (isNotLastGroupDigit);
	bitOffset += bitsRead;
}
int readPacket(string& const input, int& bitOffset, bool pad) 
{
	int packetVersionSum = 0;
	packetVersionSum += readNBits(input, bitOffset, 3);
	int packetTypeId = readNBits(input, bitOffset + 3, 3);
	int localBitOffset = bitOffset + 6;
	switch (packetTypeId)
	{
		case 4:
			readLiteralValue(input, localBitOffset);
			break;
		default:
			packetVersionSum += readOperatorPacket(input, localBitOffset);
			break;
	}
	if (pad && (localBitOffset - bitOffset) % 4 != 0)
		localBitOffset += 4 - ((localBitOffset - bitOffset) % 4);
	bitOffset = localBitOffset;
	return packetVersionSum;
}
int readPacket(string& const input)
{
	int bitOffset = 0;
	return readPacket(input, bitOffset, true);
}

int main() {
	string line = readFile("../inputFiles/2021_16.txt");
	//string line = "620080001611562C8802118E34";
	int packetVersionSum = readPacket(line);
	cout << packetVersionSum;
	return 0;
}