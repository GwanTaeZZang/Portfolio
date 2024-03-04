using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpDino : Obstacle
{
    private const float JUMP_POWER = 0.026f;
    private const float GRAVITY = 0.015f;
    private const int REJUMP_POS_Y = -20;

    private SpriteRenderer jumpDinoRenderer;

    private float curJumpPower;
    private bool isJump;
    private Vector2 jumpDinoPos;
    

    public override void Initialized(SpriteRenderer _obstacle, Transform _parent, float _rePosX, float _inScenePosX)
    {
        jumpDinoRenderer = GameObject.Instantiate<SpriteRenderer>(_obstacle, _parent);
        obstacle = jumpDinoRenderer.transform;
        //jumpDinoPos = obstacle.position;
        jumpDinoPos = new Vector2(CREATE_POS_X, CREATE_POS_Y);
        obstacle.position = jumpDinoPos;

        speed = jumpDinoRenderer.sortingOrder;
        repositionX = _rePosX;
        inScenePosX = _inScenePosX;
        isCollision = false;


        SetVisible(false);
    }

    public override void Move()
    {

        if (jumpDinoPos.x + width * HALF < repositionX)
        {
            SetVisible(false);
            isInScene = false;
            isCollision = false;
        }
        else
        {
            //Vector2 pos = obstacle.position;
            jumpDinoPos.x += Time.deltaTime * speed * -HALF;
            obstacle.position = jumpDinoPos;
        }

        if (obstacle.position.x < inScenePosX && !isCollision)
        {
            isInScene = true;
        }


        Gravity();
    }

    public override bool SetPosition(Floor _floor)
    {
        width = jumpDinoRenderer.bounds.size.x;
        height = jumpDinoRenderer.bounds.size.y;

        //Debug.Log("스프라이트 : " + cactusRenderer.sprite + "  높이 : " + height + "  길이 : " + width);

        AABB aabb = _floor.GetAABB();

        jumpDinoPos = obstacle.position;

        float posX = aabb.pos.x - (aabb.width * 0.5f) - (_floor.GetBetween() * 0.5f);

        jumpDinoPos.x = posX;
        jumpDinoPos.y = GetRandomValue(-20 , -15);

        obstacle.position = jumpDinoPos;

        SetVisible(true);
        return true;
    }

    private void Gravity()
    {
        if (isJump)
        {
            curJumpPower -= Time.deltaTime * GRAVITY;
            jumpDinoPos.y += curJumpPower;

            if(REJUMP_POS_Y > jumpDinoPos.y)
            {
                isJump = false;
                jumpDinoPos.y = REJUMP_POS_Y;
                curJumpPower = 0;
            }
        }
        else
        {
            Jump();
        }
    }

    private void Jump()
    {
        curJumpPower = JUMP_POWER;
        isJump = true;
    }
}
