#include "DemoScene.h"
#include "SimpleAudioEngine.h"

using namespace cocos2d;

Scene* DemoScene::createScene()
{
    return DemoScene::create();
}

static void problemLoading(const char* filename)
{
    printf("Error while loading: %s\n", filename);
    printf("Depending on how you compiled you might have to add 'Resources/' in front of filenames in DemoSceneScene.cpp\n");
}

bool DemoScene::init()
{
    if ( !Scene::init() )
    {
        return false;
    }

    auto visibleSize = Director::getInstance()->getVisibleSize();
    Vec2 origin = Director::getInstance()->getVisibleOrigin();
	
	gameLayer = LayerColor::create(Color4B::GRAY);
	this->addChild(gameLayer);
	
	scoreLabel = Label::create("Score: "+ std::to_string(score), "Arial", 28);
	scoreLabel->setAnchorPoint(Vec2(0,0));
	scoreLabel->setPosition(Vec2(10, 360));
	gameLayer->addChild(scoreLabel, 1);

	Label* instructions = Label::create("Controlls: \n\nArrow keys - move the cells in the grid; \n\nR - reset; \n\nEsc - Close game.","Arial", 22,Size(150,390));
	instructions->setAnchorPoint(Vec2(0,0));
	instructions->setPosition(Vec2(250, 0));
	gameLayer->addChild(instructions, 1);

	/*
	  ->5<-
		|<-------------218------------->|
	 (5,310)						(223,310)	
	_	 _______ _______ _______ _______		5 + (56)*pos.x , 260 - ( 56*pos.y)
	^	|		|		|		|		|		
	|	|<-50-->|	 -->6<--	|		|       x0 = 5;		y0 = 260;
	|	|_______|_______|_______|_______|		x1 = 61;	y1 = 204;
	|	|   ^   |	|	|		|		|		x2 = 117;	y2 = 148;
	|	|	50	|	v	|		|		|		x3 = 173;	y3 = 92;
   218	|___v___|___6___|_______|_______|
	|	|		|	^	|		|		|		Adding 2 to outer edges so they don't touch cells;
	|	|		|	|	|		|		|	
	|	|_______|_______|_______|_______|
	|	|		|		|		|		|
	v	|		|		|		|		|
	_	|_______|_______|_______|_______|
	  (5,92)						(223,92)
	*/

	Color4F lineColor = Color4F::BLACK;
	Color4F boxColor = Color4F(1.0f, 0.8f, 0.5f, 1.0f);
	DrawNode* bE = DrawNode::create(); 
	//Draw grid box
	bE->drawSolidRect(Vec2(3,90), Vec2(226,312), boxColor);
	//Draw score box
	bE->drawSolidRect(scoreLabel->getPosition()-Vec2(5,10), scoreLabel->getPosition()+Vec2(215, 40), boxColor);
	//Draw instruction box
	bE->drawSolidRect(instructions->getPosition() + Vec2(-5, 10), instructions->getPosition() + instructions->getContentSize() + Vec2(-10, 2), boxColor);
	//Vertical lines
	bE->drawLine(Vec2(3, 312), Vec2(3, 90), lineColor);
	bE->drawLine(Vec2(58, 312), Vec2(58, 90), lineColor);
	bE->drawLine(Vec2(114, 312), Vec2(114, 90), lineColor);
	bE->drawLine(Vec2(170, 312), Vec2(170, 90), lineColor);
	bE->drawLine(Vec2(226, 312), Vec2(226, 90), lineColor);
	//Horizontal lines
	bE->drawLine(Vec2(3, 312), Vec2(226, 312), lineColor);
	bE->drawLine(Vec2(3, 257), Vec2(226, 257), lineColor);
	bE->drawLine(Vec2(3, 201), Vec2(226, 201), lineColor);
	bE->drawLine(Vec2(3, 145), Vec2(226, 145), lineColor);
	bE->drawLine(Vec2(3, 90), Vec2(226, 90), lineColor);
	gameLayer->addChild(bE, 0);
	

	auto keyboardInputListener = cocos2d::EventListenerKeyboard::create();
	keyboardInputListener->onKeyPressed =
		[&](EventKeyboard::KeyCode key, Event * e)
		{
			if (!stoped) 
			{
				if (key == EventKeyboard::KeyCode::KEY_UP_ARROW) 
				{
					move(MoveDirection::UP);
				}
				else if (key == EventKeyboard::KeyCode::KEY_DOWN_ARROW)
				{
					move(MoveDirection::DOWN);
				}
				else if (key == EventKeyboard::KeyCode::KEY_LEFT_ARROW)
				{
					move(MoveDirection::LEFT);
				}
				else if (key == EventKeyboard::KeyCode::KEY_RIGHT_ARROW)
				{
					move(MoveDirection::RIGHT);
				}
				else if (key == EventKeyboard::KeyCode::KEY_R)
				{
					reset();
				}
				else if (key == EventKeyboard::KeyCode::KEY_ESCAPE)
				{
					Director::getInstance()->end();
				}
			}
		};
	_eventDispatcher->addEventListenerWithSceneGraphPriority(keyboardInputListener, this);
	spawnRandomCell();
    return true;
}

void DemoScene::move(DemoScene::MoveDirection moveDir)
{
	int scored = 0;
	for (int xAxis = 0; xAxis < 4; xAxis++) 
	{
		for (int yAxis = 0; yAxis < 4; yAxis++)
		{
			if (grid[xAxis][yAxis] != nullptr)
			{
				grid[xAxis][yAxis]->createdThisMove = false;
			}
		}
	}
	switch (moveDir)
	{
		case DemoScene::MoveDirection::UP:
			for (int xAxis = 0; xAxis < 4 ; xAxis++) 
			{
				for (int yAxis = 1; yAxis < 4; yAxis++)
				{
					if (grid[xAxis][yAxis] != nullptr)
					{
						scored += moveCell(Vec2(xAxis, yAxis), moveDir);
					}
				}
			}
		break;
		case DemoScene::MoveDirection::DOWN:
			for (int xAxis = 0; xAxis < 4; xAxis++)
			{
				for (int yAxis = 2; yAxis >= 0; yAxis--)
				{
					if (grid[xAxis][yAxis] != nullptr)
					{
						scored += moveCell(Vec2(xAxis, yAxis), moveDir);
					}
					
				}
			}
		break;
		case DemoScene::MoveDirection::LEFT:
			for (int xAxis = 1; xAxis < 4; xAxis++)
			{
				for (int yAxis = 0; yAxis < 4; yAxis++)
				{
					if (grid[xAxis][yAxis] != nullptr)
					{
						scored += moveCell(Vec2(xAxis, yAxis), moveDir);
					}
				}
			}
		break;
		case DemoScene::MoveDirection::RIGHT:
			for (int xAxis = 2; xAxis >= 0; xAxis--)
			{
				for (int yAxis = 0; yAxis < 4; yAxis++)
				{
					if (grid[xAxis][yAxis] != nullptr)
					{
						scored += moveCell(Vec2(xAxis, yAxis), moveDir);
					}
				}
			}
		break;
	}
	if (scored > 0)
	{
		score += scored;
		spawnRandomCell();
		scoreLabel->setString("Score: " + std::to_string(score));
	}
}

int DemoScene::moveCell(Vec2 from, MoveDirection dir) 
{
	Vec2 target = from;
	if (dir == MoveDirection::UP) 
	{
		for (int y = from.y-1; y >= 0; y--) 
		{
			if (grid[(int)(from.x)][y] == nullptr) 
			{
				target = Vec2(from.x, y);
				continue;
			}
			else if (grid[(int)(from.x)][y]->_number == grid[(int)(from.x)][(int)(from.y)]->_number && !grid[(int)(from.x)][y]->createdThisMove)
			{
				Cell* combined = new Cell(grid[(int)(from.x)][y]->_number + grid[(int)(from.x)][(int)(from.y)]->_number, Vec2(from.x,y), gameLayer);
				moveCellAnim(grid[(int)(from.x)][(int)(from.y)], Vec2(from.x, y));
				delete grid[(int)(from.x)][y];
				delete grid[(int)(from.x)][(int)(from.y)];
				grid[(int)(from.x)][(int)(from.y)] = nullptr;
				grid[(int)(from.x)][y] = &(*combined);
				return combined->_number;
			}
			else 
			{
				break;
			}
		}
		if (target != from) 
		{
			grid[(int)(target.x)][(int)(target.y)] = grid[(int)(from.x)][(int)(from.y)];
			grid[(int)(from.x)][(int)(from.y)] = nullptr;
			moveCellAnim(grid[(int)(target.x)][(int)(target.y)], target);
			return 1;
		}

	}
	else if (dir == MoveDirection::DOWN)
	{
		for (int y = from.y+1; y < 4 ; y++)
		{
			if (grid[(int)(from.x)][y] == nullptr)
			{
				target = Vec2(from.x, y);
				continue;
			}
			else if (grid[(int)(from.x)][y]->_number == grid[(int)(from.x)][(int)(from.y)]->_number && !grid[(int)(from.x)][y]->createdThisMove)
			{
				Cell* combined = new Cell(grid[(int)(from.x)][y]->_number + grid[(int)(from.x)][(int)(from.y)]->_number, Vec2(from.x, y), gameLayer);
				moveCellAnim(grid[(int)(from.x)][(int)(from.y)], Vec2(from.x, y));
				delete grid[(int)(from.x)][y];
				delete grid[(int)(from.x)][(int)(from.y)];
				grid[(int)(from.x)][(int)(from.y)] = nullptr;
				grid[(int)(from.x)][y] = &(*combined);
				return combined->_number;
			}
			else
			{
				break;
			}
		}
		if (target != from)
		{
			grid[(int)(target.x)][(int)(target.y)] = grid[(int)(from.x)][(int)(from.y)];
			grid[(int)(from.x)][(int)(from.y)] = nullptr;
			moveCellAnim(grid[(int)(target.x)][(int)(target.y)], target);
			return 1;
		}

	}
	if (dir == MoveDirection::LEFT)
	{
		for (int x = from.x-1; x >= 0; x--)
		{
			if (grid[(x)][(int)(from.y)] == nullptr)
			{
				target = Vec2(x, from.y);
				continue;
			}
			else if (grid[x][(int)(from.y)]->_number == grid[(int)from.x][(int)(from.y)]->_number && !grid[x][(int)(from.y)]->createdThisMove)
			{
				Cell* combined = new Cell(grid[x][(int)(from.y)]->_number + grid[(int)(from.x)][(int)(from.y)]->_number, Vec2(x, (int)(from.y)), gameLayer);
				moveCellAnim(grid[(int)(from.x)][(int)(from.y)], Vec2(x, (int)(from.y)));
				delete grid[x][(int)(from.y)];
				delete grid[(int)(from.x)][(int)(from.y)];
				grid[x][(int)(from.y)] = &(*combined);
				grid[(int)(from.x)][(int)(from.y)] = nullptr;
				return combined->_number;
			}
			else
			{
				break;
			}
		}
		if (target != from)
		{
			grid[(int)(target.x)][(int)(target.y)] = grid[(int)(from.x)][(int)(from.y)];
			grid[(int)(from.x)][(int)(from.y)] = nullptr;
			moveCellAnim(grid[(int)(target.x)][(int)(target.y)], target);
			return 1;
		}

	}
	if (dir == MoveDirection::RIGHT)
	{
		for (int x = from.x+1; x < 4; x++)
		{
			if (grid[(x)][(int)(from.y)] == nullptr)
			{
				target = Vec2(x, from.y);
				continue;
			}
			else if (grid[x][(int)(from.y)]->_number == grid[(int)from.x][(int)(from.y)]->_number && !grid[x][(int)(from.y)]->createdThisMove)
			{
				Cell* combined = new Cell(grid[x][(int)(from.y)]->_number + grid[(int)(from.x)][(int)(from.y)]->_number, Vec2(x, (int)(from.y)), gameLayer);
				moveCellAnim(grid[(int)(from.x)][(int)(from.y)], Vec2(x, (int)(from.y)));
				delete grid[x][(int)(from.y)];
				delete grid[(int)(from.x)][(int)(from.y)];
				grid[(int)(from.x)][(int)(from.y)] = nullptr;
				grid[x][(int)(from.y)] = &(*combined);
				return combined->_number;
			}
			else
			{
				break;
			}
		}
		if (target != from)
		{
			grid[(int)(target.x)][(int)(target.y)] = grid[(int)(from.x)][(int)(from.y)];
			grid[(int)(from.x)][(int)(from.y)] = nullptr;
			moveCellAnim(grid[(int)(target.x)][(int)(target.y)], target);
			return 1;
		}

	}
	return 0;
}

void DemoScene::moveCellAnim( Cell* target, Vec2 to) 
{
	Vec2 drawTo = Vec2(5 + (56)*to.x, 260 - (56*to.y));
	if (target->_sprite->getNumberOfRunningActions() > 0)
	{
		target->_sprite->getActionManager()->removeActionByTag(1, target->_sprite);
	}
	target->_sprite->runAction(MoveTo::create(0.1f, drawTo))->setTag(1);
}

void DemoScene::spawnRandomCell()
{
	std::vector<Vec2> freeCells;
	for (int xAxis = 0; xAxis < 4; xAxis++)
	{
		for (int yAxis = 0; yAxis < 4; yAxis++)
		{
			if (grid[xAxis][yAxis] == nullptr)
			{
				freeCells.push_back(Vec2(xAxis, yAxis));
			}
		}
	}
	auto selected = freeCells.at(random<int>(0, freeCells.size()-1));
	grid[(int)selected.x][(int)selected.y] = new Cell(2, selected, gameLayer);
}

void DemoScene::reset()
{
	score = 0;
	for (int xAxis = 0; xAxis < 4; xAxis++)
	{
		for (int yAxis = 0; yAxis < 4; yAxis++)
		{
			if (grid[xAxis][yAxis] != nullptr)
			{
				delete grid[xAxis][yAxis];
				grid[xAxis][yAxis] = nullptr;
			}
		}
	}
	spawnRandomCell();
}

void DemoScene::menuCloseCallback(Ref* pSender)
{

    Director::getInstance()->end();

    #if (CC_TARGET_PLATFORM == CC_PLATFORM_IOS)
    exit(0);
#endif


}
