#include "Cell.h"

Cell::Cell(int number, Vec2& pos, Node* parent)
	:_number(number)
{
	Vec2 drawPos = Vec2(5 + (56)*pos.x, 260 - (56*pos.y));
	_sprite = LayerColor::create();
	_sprite->setAnchorPoint(Vec2(0, 0));
	_sprite->setContentSize(Size(50,50));
	_sprite->setPosition(drawPos);
	_sprite->setColor(Color3B(number % 256, (number % 512) / 2, (number % 768) / 3));
	_sprite->setOpacity(255);
	_sprite->setScale(0);
	
	//_sprite->drawRect(drawPos, Vec2(drawPos.x + 50, drawPos.y - 50), ;
	auto label = Label::createWithSystemFont( std::to_string(number), "Arial", 16, Size(50,50));
	label->setAnchorPoint(Vec2(0,0));
	label->setAlignment(TextHAlignment::CENTER, TextVAlignment::CENTER);
	_sprite->addChild(label);
	parent->addChild(_sprite, 1);
	_sprite->runAction(ScaleTo::create(0.1f, 1));
}

Cell::~Cell()
{
	_sprite->runAction(Sequence::createWithTwoActions(DelayTime::create(0.1f), (CallFuncN::create(
		[&](Node* n)
		{
			n->removeFromParent();
		}
	))));
	//_sprite->release();
}


