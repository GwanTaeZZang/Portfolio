using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Obstacle
{
    private const int COUNT = 5;
    private const float ARROW_HEIGHT_INTERVAL = 0.4f;
    private const int ARROW_PATTERN_COUNT_MIN = 1;
    private const int ARROW_PATTERN_COUNT_MAX = 6;
    private const int CREATE_ARROW_PERCENTAGE_MIN = 0;
    private const int CREATE_ARROW_PERCENTAGE_MAX = 2;

    private Transform arrowPatternTransform;
    private SpriteRenderer arrowRenderer;
    private List<SpriteRenderer> arrowList = new List<SpriteRenderer>();
    private Vector2 arrowPos;

    private float arrowHeight;

    public override void Initialized(SpriteRenderer _obstacle, Transform _parent, float _rePosX, float _inScenePosX)
    {
        CreateArrowPattern(_obstacle, _parent);
        SetArrowPatternPosition(COUNT);
        obstacle = arrowPatternTransform;

        arrowPos = new Vector2(CREATE_POS_X, CREATE_POS_Y);
        obstacle.position = arrowPos;

        speed = arrowRenderer.sortingOrder;
        repositionX = _rePosX;
        inScenePosX = _inScenePosX;
        isCollision = false;


        width = arrowRenderer.bounds.size.x;
        arrowHeight = arrowRenderer.bounds.size.y;

        SetVisible(false);
    }

    private void CreateArrowPattern(SpriteRenderer _obstacle, Transform _parent)
    {
        GameObject arrowPattern = new GameObject();
        arrowPattern.name = "ArrowPattern";
        arrowPatternTransform = arrowPattern.transform;

        for(int i = 0; i < COUNT; i++)
        {
            SpriteRenderer arrow = GameObject.Instantiate<SpriteRenderer>(_obstacle, arrowPatternTransform);
            arrowList.Add(arrow);

            if (i == 1)
            {
                arrowRenderer = arrow;
            }
        }
        arrowPattern.transform.SetParent(_parent);
    }

    private void SetArrowPatternPosition(int _count)
    {
        height = ARROW_HEIGHT_INTERVAL * _count;
        float startPosY = arrowPatternTransform.position.y + (height * HALF) - (arrowHeight * HALF);

        for (int i = 0; i < COUNT; i++)
        {
            var arrow = arrowList[i];
            Vector2 arrowPos = arrow.transform.position;

            arrowPos.y = -(i * ARROW_HEIGHT_INTERVAL) + startPosY;
            arrow.transform.position = arrowPos;
        }
    }

    private void RandomArrowPatternCount()
    {
        //.. TODO :: 리터럴 상수 // correction
        int count = GetRandomValue(ARROW_PATTERN_COUNT_MIN, ARROW_PATTERN_COUNT_MAX);
        for(int i =0; i < COUNT; i++)
        {
            //.. TODO :: 한줄로 변경가능  // correction
            arrowList[i].gameObject.SetActive(i < count ? true : false);
            //if(i < count)
            //{
            //    arrowList[i].gameObject.SetActive(true);
            //}
            //else
            //{
            //    arrowList[i].gameObject.SetActive(false);
            //}

        }
        SetArrowPatternPosition(count);
    }



    //public override void Move()
    //{
    //    if (arrowPos.x + width * HALF < repositionX)
    //    {
    //        SetVisible(false);
    //        isInScene = false;
    //        isCollision = false;

    //    }
    //    else
    //    {
    //        arrowPos.x += Time.deltaTime * speed * -HALF * 2;
    //        obstacle.position = arrowPos;
    //    }

    //    if (obstacle.position.x < inScenePosX && !isCollision)
    //    {
    //        isInScene = true;
    //    }

    //}

    public override bool SetPosition(Floor _floor)
    {
        //.. TODO :: 리터럴 상수  // correction
        if(GetRandomValue(CREATE_ARROW_PERCENTAGE_MIN, CREATE_ARROW_PERCENTAGE_MAX) == 0)
        {
            return false;
        }
        AABB aabb = _floor.GetAABB();

        arrowPos.x = aabb.pos.x;
        arrowPos.y = GetRandomValue(aabb.pos.y, aabb.pos.y + 4);

        obstacle.position = arrowPos;

        RandomArrowPatternCount();

        SetVisible(true);
        return true;
    }
}
