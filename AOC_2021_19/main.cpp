#include <stringUtilities.hpp>
#include <fileUtilities.h>
#include <iostream>
#include <array>
#include <vector>

using namespace std;

#define _cos(a) (a==0 ? 1 : (a == 180 ? -1 : 0))
#define _sin(a) (a==90 ? 1 : (a == 270 ? -1 : 0))

struct Vector3D { int X; int Y; int Z; };

inline Vector3D TransformVector(const Vector3D& position, const Vector3D& rotation)
{
	int x = position.X, y = position.Y, z = position.Z;
	//rotate over x
	y = y * _cos(rotation.X) - z * _sin(rotation.X);
	z = y * _sin(rotation.X) + z * _sin(rotation.X);
	//rotate over y
	x = x * _cos(rotation.Y) + z * _sin(rotation.Y);
	z = z * _cos(rotation.Y) - x * _sin(rotation.Y);
	//rotate over z
	x = x * _cos(rotation.Z) - y * _sin(rotation.Z);
	y = x * _sin(rotation.Z) + y * _cos(rotation.Z);

	return { x, y, z };
}

bool IsBeaconTheSame(const Vector3D& beacon1, const Vector3D& rotation1, const Vector3D& beacon2, const Vector3D& rotation2, const Vector3D& offset)
{
	//TODO
	return true;
}

template <size_t _Size>
int GetBeaconCount(const array<vector<Vector3D>, _Size>& beaconsPerScanner)
{
	//imagine a left-handed cartesian coordinate system
	const Vector3D rotations[24]{
		Vector3D{0, 0,   0},   Vector3D{90, 0,   0},   Vector3D{180, 0,   0},   Vector3D{270,  0,   0},
		Vector3D{0, 90,  0},   Vector3D{90, 90,  0},   Vector3D{180, 90,  0},   Vector3D{270,  90,  0},
		Vector3D{0, 180, 0},   Vector3D{90, 180, 0},   Vector3D{180, 180, 0},   Vector3D{270,  180, 0},
		Vector3D{0, 270, 0},   Vector3D{90, 270, 0},   Vector3D{180, 270, 0},   Vector3D{270,  270, 0},
		Vector3D{0, 0,   90},  Vector3D{90, 0,   90},  Vector3D{180, 0,   90},  Vector3D{270,  0,   90},
		Vector3D{0, 0,   270}, Vector3D{90, 0,   270}, Vector3D{180, 0,   270}, Vector3D{270,  0,   270}
	};

	for (const vector<Vector3D>& scanner : beaconsPerScanner)
	{
		for (const auto& beacon : scanner)
		{
			for (const auto& rotation : rotations)
			{
				auto transformedBeaconPosition = TransformVector(beacon, rotation);
				for (const vector<Vector3D>& scanner2 : beaconsPerScanner)
				{
					if (&scanner2 == &scanner) continue;
					for (const auto& beacon2 : scanner)
					{
						for (const auto& rotation2 : rotations)
						{
							auto transformedBeaconPosition2 = TransformVector(beacon2, rotation2);
							//todo
						}
					}

				}
			}
		}
	}

	return 0;
}

int main() {
	vector<Vector3D> beacons;
	array<vector<Vector3D>, 27> beaconsPerScanner;
	int scannerIndex = 0, lineIndex = 0;
	for(auto& line : split(readFile("../inputFiles/2021_19.txt"), '\n'))
	{
		if (lineIndex == 0) {
			++lineIndex; continue;
		}
		if (line.find("---") != string::npos)
		{
			beaconsPerScanner[scannerIndex++] = move(beacons);
			beacons = vector<Vector3D>();
		}
		else
		{
			auto coordinates = split(line, ',');
			beacons.push_back(Vector3D{ stoi(coordinates[0]), stoi(coordinates[1]), stoi(coordinates[2]) });
		}
		++lineIndex;
	}
	beaconsPerScanner[scannerIndex] = move(beacons);

	int beaconCount = GetBeaconCount(beaconsPerScanner);

	cout << beaconCount;
	return 0;
}