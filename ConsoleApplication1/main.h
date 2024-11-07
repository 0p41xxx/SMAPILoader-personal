#pragma once
#include <iostream>


class main {
public:
	void* operator new(std::size_t size) {
		std::cout << "Allocating " << size << " bytes of memory for MyClass\n";
		void* p = ::operator new(size);
		return p;
	}

	// ��˹����������Ŵ�ͧ operator delete ���ͷӡ�ä׹˹��¤�����
	void operator delete(void* p) {
		std::cout << "Freeing memory of MyClass\n";
		::operator delete(p);
	}

	int data;
};

