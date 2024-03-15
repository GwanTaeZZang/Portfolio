using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    protected const float HALF = 0.5f;
    protected const int CREATE_POS_X = -5;
    protected const int CREATE_POS_Y = -5;
    protected const int SET_ITEM_MIN = 0;
    protected const int SET_ITEM_MAX = 10;
    protected const int ITEM_POSY = 5;

    protected float width;
    protected float height;
    protected float speed;
    protected float repositionX;
    protected float inScenePosX;
    protected bool isInScene;
    protected bool isCollision;

    protected Transform item;
    protected Vector2 itemPos;
    protected SpriteRenderer itemSpriteRenderer;

    public abstract void Initialized(SpriteRenderer _item, Transform _parent, float _rePosX, float _inScenePosX);
    public virtual void SetPosition(Floor _floor)
    {
        width = itemSpriteRenderer.bounds.size.x;
        height = itemSpriteRenderer.bounds.size.y;

        // 아이템 생성 위치를 블럭 위로 지정했을 때 로직
        AABB floorAABB = _floor.GetAABB();
        float randomPosMin = (floorAABB.pos.x - floorAABB.width * HALF);
        float randomPosMax = (floorAABB.pos.x + floorAABB.width * HALF);

        itemPos.x = GetRandomValue(randomPosMin, randomPosMax);
        itemPos.y = floorAABB.pos.y + ITEM_POSY;

        SetVisible(true);

    }
    public abstract void CollisionItem(Player _player);

    public virtual void Move()
    {
        if (itemPos.x + width * HALF < repositionX)
        {
            SetVisible(false);
            isInScene = false;
            isCollision = false;

        }
        else
        {
            itemPos.x += Time.deltaTime * speed * -HALF;
            item.position = itemPos;
        }

        if (itemPos.x < inScenePosX && !isCollision)
        {
            isInScene = true;
        }

    }

    public AABB GetAABB()
    {
        AABB aabb;
        aabb.pos = itemPos;
        aabb.width = width;
        aabb.height = height;

        return aabb;
    }

    public void SetVisible(bool _visible)
    {
        item.gameObject.SetActive(_visible);
    }

    public int GetRandomValue(float _min, float _max)
    {
        return (int)Random.Range(_min, _max);
    }

    public bool IsInScene()
    {
        return isInScene;
    }

    public void SetIsInScene(bool _isInScene)
    {
        isInScene = _isInScene;
    }
    public void SetIsCollision(bool _isCollision)
    {
        isCollision = _isCollision;
    }

}
