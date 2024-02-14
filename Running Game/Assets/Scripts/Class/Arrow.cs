using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Obstacle
{
    private SpriteRenderer arrowRenderer;
    private Vector2 arrowPos;

    public override void Initialized(SpriteRenderer _obstacle, Transform _parent, float _rePosX)
    {
        arrowRenderer = GameObject.Instantiate<SpriteRenderer>(_obstacle, _parent);
        obstacle = arrowRenderer.transform;
        //jumpDinoPos = obstacle.position;
        arrowPos = new Vector2(CREATE_POS_X, CREATE_POS_Y);
        obstacle.position = arrowPos;

        speed = arrowRenderer.sortingOrder;
        repositionX = _rePosX;

        SetVisible(false);
    }

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

        SetVisible(true);
        return true;
    }
}
