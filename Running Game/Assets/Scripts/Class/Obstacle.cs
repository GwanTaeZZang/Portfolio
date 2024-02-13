using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacle
{
    //.. TODO :: move to baseclass
    protected const float HALF = 0.5f;
    protected const int SPRITE_RANDOM_MIN = 0;
    protected const int SPRITE_RANDOM_MAX = 3;
    protected const int CACTUS_CORRECTION_POS_MIN = 2;
    protected const int CACTUS_CORRECTION_POS_MAX = -1;
    protected const int CACTUS_C_FLOOR_MINIMUM = 6;
    protected const int CREATE_POS_X = -5;
    protected const int CREATE_POS_Y = -5;
    //..

    //.. TODO :: move to baseclass
    protected float width;
    protected float height;
    protected float speed;
    protected float repositionX;

    private SpriteRenderer obstacleRenderer;

    protected Transform obstacle;

    public abstract void Move();
    public abstract bool SetPosition(Floor _floor);
    public abstract void Initialized(SpriteRenderer _obstacle, Transform _parent, float _rePosX);

    public AABB GetAABB()
    {
        AABB aabb;
        aabb.pos = obstacle.position;
        aabb.width = width;
        aabb.height = height;

        return aabb;
    }

    public Vector2 GetPos()
    {
        return obstacle.position;
    }

    public float GetWidth()
    {
        return width;
    }

    public void SetVisible(bool _visible)
    {
        obstacle.gameObject.SetActive(_visible);
    }

    public int GetRandomValue(float _min, float _max)
    {
        return (int)Random.Range(_min, _max);
    }
}
