#pragma once
#include <string>
#include <fstream>
std::string readFile(const char* const filename) {
	std::ifstream ifs { filename };
	
	//std::string allText;
	//fs >> allText;

	std::string allText{ (std::istreambuf_iterator<char>(ifs)), std::istreambuf_iterator<char>() };

	return allText;
}
//alternative:
//std::string read_file(const char* const path) {
//    constexpr auto read_size = std::size_t(4096);
//    auto stream = std::ifstream(path);
//    stream.exceptions(std::ios_base::badbit);
//
//    if (not stream) {
//        throw std::ios_base::failure("file does not exist");
//    }
//
//    auto out = std::string();
//    auto buf = std::string(read_size, '\0');
//    while (stream.read(&buf[0], read_size)) {
//        out.append(buf, 0, stream.gcount());
//    }
//    out.append(buf, 0, stream.gcount());
//    return out;
//}