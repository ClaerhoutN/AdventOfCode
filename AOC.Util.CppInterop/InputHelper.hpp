#pragma once
//#include <msclr\marshal.h>
#include <string>
#include <vector>

using namespace System;
using namespace System::Threading::Tasks;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;

using namespace AOC;

std::vector<std::string> GetInputLines(const char* url, const char* const lineSeparatorRegex)
{
	String^ cliUrl = gcnew String(url);
	String^ cliLineSeparatorRegex = gcnew String(lineSeparatorRegex);
	Task<IReadOnlyList<String^>^>^ inputLinesTask = AOC::Util::InputHelper::GetInputLines<String^>(cliUrl, "", cliLineSeparatorRegex);
	auto result = inputLinesTask->Result;

	std::vector<std::string> lines;
	for (int i = 0; i < result->Count; ++i)
	{
		String^ line = result->default[i];
		IntPtr ip = Marshal::StringToHGlobalAnsi(line);
		const char* str = static_cast<const char*>(ip.ToPointer());
		std::string stdLine{ str };
		Marshal::FreeHGlobal(ip);
		lines.push_back(stdLine);
	}
	return lines;
}
