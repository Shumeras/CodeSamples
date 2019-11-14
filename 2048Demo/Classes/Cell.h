#pragma once
#include "cocos2d.h"

using namespace cocos2d;

class Cell
{
public:
	int _number;
	Node* _sprite;


	bool createdThisMove = true;
	
	Cell(int _number, Vec2& pos, Node* parent);

	virtual ~Cell();

private:
};