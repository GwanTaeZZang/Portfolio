using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//.. TODO :: baseclass -> ObstacleClass
public class Cactus : Obstacle
{
    private SpriteRenderer cactusRenderer;
    private Sprite[] cactusSpriteArr;

    public override void Initialized(SpriteRenderer _obstacle, Transform _parent, float _rePosX, float _inScenePosX)
    {

        cactusRenderer = GameObject.Instantiate<SpriteRenderer>(_obstacle, _parent);
        obstacle = cactusRenderer.transform;
        obstacle.position = new Vector2(CREATE_POS_X, CREATE_POS_Y);

        cactusSpriteArr = new Sprite[] { Resources.Load<Sprite>("Prefab/Sprite/Cactus/Cactus A"),
                                         Resources.Load<Sprite>("Prefab/Sprite/Cactus/Cactus B"),
                                         Resources.Load<Sprite>("Prefab/Sprite/Cactus/Cactus C")};
        speed = cactusRenderer.sortingOrder;
        repositionX = _rePosX;
        inScenePosX = _inScenePosX;
        isCollision = false;


        SetVisible(false);
    }

    public override void Move()
    {
        if (obstacle.position.x + width * HALF < repositionX)
        {
            SetVisible(false);
            isInScene = false;
            isCollision = false;

        }
        else
        {
            Vector2 pos = obstacle.position;
            pos.x += Time.deltaTime * speed * -HALF;
            obstacle.position = pos;
        }

        if (obstacle.position.x < inScenePosX && !isCollision)
        {
            isInScene = true;
        }

    }

    //public void MoveCactus()
    //{
    //    if(obstacle.position.x + width * HALF < repositionX)
    //    {
    //        SetVisible(false);
    //    }
    //    else
    //    {
    //        Vector2 pos = obstacle.position;
    //        pos.x += Time.deltaTime * speed * -HALF;
    //        obstacle.position = pos;
    //    }
    //}

    //public Vector2 GetPos()
    //{
    //    return obstacle.position;
    //}

    //public float GetWidth()
    //{
    //    return width;
    //}

    //public AABB GetCactusAABB()
    //{
    //    AABB aabb;
    //    aabb.pos = obstacle.position;
    //    aabb.width = width;
    //    aabb.height = height;

    //    return aabb;
    //}

    //private void SetWidth(float _width)
    //{
    //    width = _width;
    //}

    //private void SetHeight(float _height)
    //{
    //    height = _height;
    //}


    //public bool SetPosition(Floor _floor)
    //{
    //}

    private void ChangeRandomSprite()
    {
        //int random = Random.Range(0, 4);
        cactusRenderer.sprite = cactusSpriteArr[GetRandomValue(SPRITE_RANDOM_MIN, SPRITE_RANDOM_MAX)];
    }



    public override bool SetPosition(Floor _floor)
    {
        ChangeRandomSprite();
        width = cactusRenderer.bounds.size.x;
        height = cactusRenderer.bounds.size.y;

        //Debug.Log("스프라이트 : " + cactusRenderer.sprite + "  높이 : " + height + "  길이 : " + width);

        AABB aabb = _floor.GetAABB();

        //if (cactusRenderer.sprite.name == "Cactus C" && aabb.width < CACTUS_C_FLOOR_MINIMUM)
        //{
        //    return false;
        //}

        Vector2 pos = obstacle.position;
        float rndMinX = (aabb.pos.x + aabb.width * -HALF) + width * HALF + CACTUS_CORRECTION_POS_MIN;
        float rndMaxX = (aabb.pos.x + aabb.width * HALF) - width * HALF + CACTUS_CORRECTION_POS_MAX;
        pos.x = GetRandomValue(rndMinX, rndMaxX);
        pos.y = (aabb.pos.y + aabb.height * HALF) + height * HALF;
        //Debug.Log("Set Cactus Position = " + pos.x);
        obstacle.position = pos;

        SetVisible(true);
        return true;

    }

}
