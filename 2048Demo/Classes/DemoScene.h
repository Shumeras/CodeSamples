#pragma once

#include "cocos2d.h"
#include "Cell.h"

class DemoScene : public cocos2d::Scene
{
public:
	enum MoveDirection
	{
		UP,
		DOWN,
		LEFT,
		RIGHT
	};

    static cocos2d::Scene* createScene();

    virtual bool init();

    void menuCloseCallback(cocos2d::Ref* pSender);
    
    CREATE_FUNC(DemoScene);

private:
	int score;
	Cell* grid[4][4];
	bool stoped = false;
	Layer* gameLayer;
	Label* scoreLabel;

	void move(DemoScene::MoveDirection moveDir);
	int moveCell(Vec2 from, MoveDirection dir);
	void moveCellAnim(Cell * target, Vec2 to);

	void spawnRandomCell();
	void reset();

};



