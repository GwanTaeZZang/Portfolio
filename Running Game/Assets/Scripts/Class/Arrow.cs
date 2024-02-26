using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Obstacle
{
    private const int COUNT = 5;
    private const float ARROW_HEIGHT_INTERVAL = 0.4f;

    private Transform arrowPatternTransform;
    private SpriteRenderer arrowRenderer;
    private List<SpriteRenderer> arrowList = new List<SpriteRenderer>();
    private Vector2 arrowPos;

    private float arrowHeight;

    public override void Initialized(SpriteRenderer _obstacle, Transform _parent, float _rePosX)
    {
        //arrowRenderer = GameObject.Instantiate<SpriteRenderer>(_obstacle, _parent);
        //obstacle = arrowRenderer.transform;
        CreateArrowPattern(_obstacle, _parent);
        SetArrowPatternPosition(COUNT);
        obstacle = arrowPatternTransform;

        //jumpDinoPos = obstacle.position;
        arrowPos = new Vector2(CREATE_POS_X, CREATE_POS_Y);
        obstacle.position = arrowPos;

        speed = arrowRenderer.sortingOrder;
        repositionX = _rePosX;

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
        //SetVirtualPivot(COUNT);
    }

    private void RandomArrowPatternCount()
    {
        int count = GetRandomValue(1, 6);
        for(int i =0; i < COUNT; i++)
        {
            if(i < count)
            {
                arrowList[i].gameObject.SetActive(true);
            }
            else
            {
                arrowList[i].gameObject.SetActive(false);
            }
        }
        SetArrowPatternPosition(count);
        //SetVirtualPivot(count);
    }

    //private void SetVirtualPivot(int _count)
    //{
    //    height = ARROW_HEIGHT_INTERVAL * _count;
    //    float centerPivotY = -(height * HALF);

    //    Vector2 pos = arrowPatternTransform.position;
    //    pos.y += centerPivotY;
    //    arrowPatternTransform.position = pos;

    //    Debug.Log(arrowPatternTransform.position);

    //    //Debug.DrawLine(new Vector3(arrowPatternTransform.position.x - width * HALF, arrowPatternTransform.position.y - height * HALF), new Vector3(arrowPatternTransform.position.x - width * HALF, arrowPatternTransform.position.y - height * HALF), Color.blue);
    //}




    public override void Move()
    {
        if (arrowPos.x + width * HALF < repositionX)
        {
            SetVisible(false);
        }
        else
        {
            //Vector2 pos = obstacle.position;
            arrowPos.x += Time.deltaTime * speed * -HALF * 2;
            obstacle.position = arrowPos;
        }







        Debug.DrawLine(new Vector3(obstacle.position.x, obstacle.position.y - height * HALF), new Vector3(obstacle.position.x, obstacle.position.y + height * HALF), Color.red);


        Debug.DrawLine(new Vector3(obstacle.position.x - width * HALF, obstacle.position.y - height * 0.3f),
            new Vector3(obstacle.position.x - width * HALF, obstacle.position.y + height * 0.3f), Color.red);
        Debug.DrawLine(new Vector3(obstacle.position.x + width * HALF, obstacle.position.y - height * HALF),
            new Vector3(obstacle.position.x + width * HALF, obstacle.position.y + height * HALF), Color.red);
    }

    public override bool SetPosition(Floor _floor)
    {
        if(GetRandomValue(0,2) == 0)
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
