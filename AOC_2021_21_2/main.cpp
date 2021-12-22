#include <stringUtilities.hpp>
#include <fileUtilities.h>
#include <iostream>
#include <unordered_map>
#include <tuple>

using namespace std;
/*p1Pos, p2Pos, p1Score, p2Score*/
unordered_map<unsigned long long, std::pair<unsigned long long, unsigned long long>> cachedUniverses1;
unordered_map<unsigned long long, std::pair<unsigned long long, unsigned long long>> cachedUniverses2;

const char maxScore = 21;
const char maxRollNr = 3;
const char minRollNr = 1;
const char rollsPerTurn = 3;

inline unsigned long long getCacheKey(char p1Pos, char p2Pos, char p1Score, char p2Score) {
	return 
		((((((unsigned long long)p1Pos
			<< sizeof(p1Pos) * CHAR_BIT)
			| (unsigned long long)p2Pos)
			<< sizeof(p2Pos) * CHAR_BIT)
			| (unsigned long long)p1Score)
			<< sizeof(p1Score) * CHAR_BIT)
			| (unsigned long long)p2Score;
}

std::pair<unsigned long long, unsigned long long> play( 
	char playerNr,
	char p1Pos, char p2Pos, 
	char rollCount = 0, char prevRolls = 0,
	char p1Score = 0, char p2Score = 0) 
{
	unsigned long long p1Wins = 0, p2Wins = 0;
	unsigned long long& wins = playerNr ? p2Wins : p1Wins;

	for (char rolled = minRollNr; rolled <= maxRollNr; ++rolled)
	{
		char _p1Pos = p1Pos, _p2Pos = p2Pos, _p1Score = p1Score, _p2Score = p2Score;
		char& pos = playerNr ? _p2Pos : _p1Pos, &score = playerNr ? _p2Score : _p1Score;
		if (rollCount == rollsPerTurn - 1)
		{
			pos = (pos + (rolled + prevRolls) - 1) % 10 + 1;
			score += pos;			
			if (score >= maxScore) ++wins;
			else
			{
				auto key = getCacheKey(_p1Pos, _p2Pos, _p1Score, _p2Score);
				auto& cache = playerNr == 0 ? cachedUniverses1 : cachedUniverses2;
				auto cachedUniverseIt = cache.find(key);
				std::pair<unsigned long long, unsigned long long> winResults;
				if (cachedUniverseIt != cache.end())
					winResults = cachedUniverseIt->second;
				else
				{
					winResults = play((playerNr + 1) % 2, _p1Pos, _p2Pos, 0, 0, _p1Score, _p2Score);
					cache.emplace(key, winResults);
				}
				p1Wins += winResults.first; p2Wins += winResults.second;
			}
		}
		else
		{
			auto played = play(playerNr, p1Pos, p2Pos, rollCount + 1, prevRolls + rolled, _p1Score, _p2Score);
			p1Wins += played.first; p2Wins += played.second;
		}
	}
	return pair<unsigned long long, unsigned long long>(p1Wins, p2Wins);
}

int main() {
	auto winningUniverses = play(0, 9, 10);
	cout << max(winningUniverses.first, winningUniverses.second);
	return 0;
}