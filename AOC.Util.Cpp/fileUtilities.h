#pragma once
#include <string>
#include <fstream>
std::string readFile(const char* const filename) {
	std::ifstream ifs { filename };
	
	//std::string allText;
	//fs >> allText;

	std::string allText{ (std::istreambuf_iterator<char>(ifs)), std::istreambuf_iterator<char>() };

	return std::move(allText);
}