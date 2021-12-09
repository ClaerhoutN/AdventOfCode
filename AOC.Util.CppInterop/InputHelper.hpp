#pragma once
#include <iostream>
#include <vector>
using namespace System;

namespace AOCUtilCppInterop {
	public ref class InputHelper
	{
	public:
		std::vector<std::string> GetInputLines(const char* url, const char* const lineSeparatorRegex);
	};
}
