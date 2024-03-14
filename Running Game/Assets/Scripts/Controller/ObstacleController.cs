using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController
{
    private const int CAPACITY = 10;
    private const float CORRECTION_HALF = 0.3f;
    private const float HALF = 0.5f;
    private const int INIT_CACTUS_IDX = CAPACITY * 0;
    private const int INIT_JUMPDINO_IDX = CAPACITY * 1;
    private const int INIT_ARROW_IDX = CAPACITY * 2;

    public delegate void SetObstacleDelegate(Floor _floor, Obstacle _obstacle);
    public SetObstacleDelegate setObstacleEvent;

    //public delegate void OnCollisionDelegate();
    //public OnCollisionDelegate onCollisionEvent;


    private List<Obstacle> obstacleList;
    private SpriteRenderer cactus;
    private SpriteRenderer dino;
    private SpriteRenderer arrow;
    private Player player;

    private int count;
    private int setCactusIdx;
    private int setJumpDinoIdx;
    private int setArrowIdx;

    public ObstacleController(Transform _parent, Player _player , float _reposX , float _inScenePosX, SetObstacleDelegate _event)
    {
        cactus = Resources.Load<SpriteRenderer>("Prefab/Obstacle/Cactus/Cactus");
        dino = Resources.Load<SpriteRenderer>("Prefab/Obstacle/Dino/Dino");
        arrow = Resources.Load<SpriteRenderer>("Prefab/Obstacle/Arrow/Arrow");

        setObstacleEvent = _event;

        obstacleList = new List<Obstacle>();
        player = _player;

        CreateObstacle<Cactus>(cactus, _parent, _reposX, _inScenePosX);
        CreateObstacle<JumpDino>(dino, _parent, _reposX, _inScenePosX);
        CreateObstacle<Arrow>(arrow, _parent, _reposX, _inScenePosX);

        count = obstacleList.Count;

        setCactusIdx = INIT_CACTUS_IDX;
        setJumpDinoIdx = INIT_JUMPDINO_IDX;
        setArrowIdx = INIT_ARROW_IDX;

        //Debug.Log(count);
    }

    public void FixedObstacleUpdate()
    {
        MoveObstacle();
        UpdateCollisionObstacle();

    }
    public void SetRandomObstaclePos(Floor _floor)
    {
        SetRandomCactusPos(_floor);
        SetRandomJumpDinoPos(_floor);
        SetRandomArrowPos(_floor);
    }

    private void CreateObstacle<T>(SpriteRenderer _obstacle, Transform _parent, float _reposX, float _inScenePosX) where T : Obstacle, new()
    {
        for (int i = 0; i < CAPACITY; i++)
        {
            T obstacle = new T();
            obstacle.Initialized(_obstacle, _parent, _reposX, _inScenePosX);
            obstacleList.Add(obstacle);
        }
    }

    private void MoveObstacle()
    {
        for (int i = 0; i < count; i++)
        {
            obstacleList[i].Move();
        }
    }










    // Oabstacles reposition Method
    private void SetRandomCactusPos(Floor _floor)
    {
        // Cactus Position Setting
        AABB aabb = _floor.GetAABB();
        Obstacle obstacle = obstacleList[setCactusIdx];
        if (aabb.width > 6)
        {
            bool isSet = obstacle.SetPosition(_floor);
            if (isSet)
            {
                setCactusIdx++;
                setCactusIdx = setCactusIdx % CAPACITY;
            }
            else
            {
                return;
            }
        }
        setObstacleEvent?.Invoke(_floor, obstacle);

    }
    private void SetRandomJumpDinoPos(Floor _floor)
    {
        if (_floor.GetBetween() < 3.5f)
        {
            bool isSet = obstacleList[setJumpDinoIdx].SetPosition(_floor);
            if (isSet)
            {
                setJumpDinoIdx++;
                setJumpDinoIdx = (setJumpDinoIdx % CAPACITY) + INIT_JUMPDINO_IDX;
            }
        }
    }
    private void SetRandomArrowPos(Floor _floor)
    {
        bool isSet = obstacleList[setArrowIdx].SetPosition(_floor);
        if (isSet)
        {
            setArrowIdx++;
            setArrowIdx = (setArrowIdx % CAPACITY) + INIT_ARROW_IDX;
        }
    }








    // Obstacles Collision Method
    private void UpdateCollisionObstacle()
    {
        int count = obstacleList.Count;
        for(int i =0; i < count; i++)
        {
            Obstacle curObstacle = obstacleList[i];

            if (curObstacle.IsInScene() && player.IsHit())
            {
                AABB curObstacleAABB = curObstacle.GetAABB();
                float obstaclePosX = curObstacleAABB.pos.x;
                float obstaclePosY = curObstacleAABB.pos.y;
                float obstacleWidth = curObstacleAABB.width;
                float obstacleHeight = curObstacleAABB.height;

                if (obstaclePosX - obstacleWidth * HALF < player.GetPlayerPos().x + CORRECTION_HALF &&
                    obstaclePosX + obstacleWidth * HALF > player.GetPlayerPos().x - CORRECTION_HALF &&
                    obstaclePosY - obstacleHeight * HALF < player.GetPlayerPos().y + CORRECTION_HALF &&
                    obstaclePosY + obstacleHeight * HALF > player.GetPlayerPos().y - CORRECTION_HALF)
                {
                    Debug.Log("Collision~~~~~~~~~~~");
                    curObstacle.SetIsCollision(true);
                    curObstacle.SetIsInScene(false);

                    player.SetHp(-1);
                    player.OffHit();
                    //onCollisionEvent?.Invoke();
                }

            }


        }
    }

}
