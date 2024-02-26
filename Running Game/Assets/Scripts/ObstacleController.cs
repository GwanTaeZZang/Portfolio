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

    public delegate void SetCoinDelegate(Floor _floor, Obstacle _obstacle);
    public SetCoinDelegate setCoinEvent;

    private List<Obstacle> obstacleList;
    private SpriteRenderer cactus;
    private SpriteRenderer dino;
    private SpriteRenderer arrow;
    private Player player;

    private int count;
    private int setCactusIdx;
    private int setJumpDinoIdx;
    private int setArrowIdx;
    private int collisionCactusIdx;
    private int collisionJumpDinoIdx;
    private int collisionArrowIdx;
    //private int frontCactusIdx;
    //private float repositionX;

    //private Camera camera;

    public ObstacleController(Transform _parent, Player _player , float _reposX , SetCoinDelegate _event)
    {
        //camera = Camera.main;

        cactus = Resources.Load<SpriteRenderer>("Prefab/Obstacle/Cactus/Cactus");
        dino = Resources.Load<SpriteRenderer>("Prefab/Obstacle/Dino/Dino");
        arrow = Resources.Load<SpriteRenderer>("Prefab/Obstacle/Arrow/Arrow");

        setCoinEvent = _event;

        obstacleList = new List<Obstacle>();
        player = _player;
        //repositionX = camera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;

        CreateObstacle<Cactus>(cactus, _parent, _reposX);
        CreateObstacle<JumpDino>(dino, _parent, _reposX);
        CreateObstacle<Arrow>(arrow, _parent, _reposX);

        count = obstacleList.Count;

        setCactusIdx = INIT_CACTUS_IDX;
        setJumpDinoIdx = INIT_JUMPDINO_IDX;
        setArrowIdx = INIT_ARROW_IDX;
        collisionCactusIdx = INIT_CACTUS_IDX;
        collisionJumpDinoIdx = INIT_JUMPDINO_IDX;
        collisionArrowIdx = INIT_ARROW_IDX;

        //Debug.Log(count);
    }

    public void ObstacleUpdate()
    {
        MoveObstacle();
        UpdateCurrentCollisionCactus();
        CheckCollisionObstacle(collisionCactusIdx);

        UpdateCurrentCollisionJumpDino();
        CheckCollisionObstacle(collisionJumpDinoIdx);

        UpdateCurrentCollisionArrow();
        CheckCollisionObstacle(collisionArrowIdx);


        //CheckCollisionJumpDino();
    }
    public void SetRandomObstaclePos(Floor _floor)
    {
        SetRandomCactusPos(_floor);
        SetRandomJumpDinoPos(_floor);
        SetRandomArrowPos(_floor);
    }

    private void CreateObstacle<T>(SpriteRenderer _obstacle, Transform _parent, float _reposX) where T : Obstacle, new()
    {
        for (int i = 0; i < CAPACITY; i++)
        {
            T obstacle = new T();
            obstacle.Initialized(_obstacle, _parent, _reposX);
            obstacleList.Add(obstacle);
        }
        //frontCactusIdx = 0;
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
                //setCoinEvent?.Invoke(_floor, obstacle);

                //Debug.Log("세팅된 선인장 인덱스" + setCactusIdx  + "   선인장의 넓이 및 인덱스 : " + aabb.width  + aabb.pos);

                setCactusIdx++;
                setCactusIdx = setCactusIdx % CAPACITY;
            }
            else
            {
                return;
            }
        }
        setCoinEvent?.Invoke(_floor, obstacle);

    }
    private void SetRandomJumpDinoPos(Floor _floor)
    {
        if (_floor.GetBetween() < 3.5f)
        {
            //Debug.Log(_floor.GetBetween());

            bool isSet = obstacleList[setJumpDinoIdx].SetPosition(_floor);
            if (isSet)
            {
                //Debug.Log("세팅된 공룡 인덱스" + setDinoIdx);

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
    private void UpdateCurrentCollisionCactus()
    {
        Obstacle curCactus = obstacleList[collisionCactusIdx];
        if (player.GetPlayerPos().x - HALF > curCactus.GetPos().x + (curCactus.GetWidth() * HALF))
        {
            collisionCactusIdx++;
            collisionCactusIdx = collisionCactusIdx % CAPACITY;
        }
    }

    private void UpdateCurrentCollisionJumpDino()
    {
        Obstacle curJumpDino = obstacleList[collisionJumpDinoIdx];
        if (player.GetPlayerPos().x - HALF > curJumpDino.GetPos().x + (curJumpDino.GetWidth() * HALF))
        {
            collisionJumpDinoIdx++;
            collisionJumpDinoIdx = (collisionJumpDinoIdx % CAPACITY) + CAPACITY;

        }

    }

    private void UpdateCurrentCollisionArrow()
    {
        Obstacle curArrow = obstacleList[collisionArrowIdx];
        if (player.GetPlayerPos().x - HALF > curArrow.GetPos().x + (curArrow.GetWidth() * HALF))
        {
            collisionArrowIdx++;
            collisionArrowIdx = (collisionArrowIdx % CAPACITY) + INIT_ARROW_IDX;

        }
    }

    private void CheckCollisionObstacle(int _obstacleIdx)
    {
        AABB curObstacle = obstacleList[_obstacleIdx].GetAABB();
        float obstaclePosX = curObstacle.pos.x;
        float obstaclePosY = curObstacle.pos.y;
        float obstacleWidth = curObstacle.width;
        float obstacleHeight = curObstacle.height;

        //if(_obstacleIdx > 19)
        //{
        //    Debug.Log(obstacleHeight);
        //}

        if (obstaclePosX - obstacleWidth * HALF < player.GetPlayerPos().x + CORRECTION_HALF &&
            obstaclePosX + obstacleWidth * HALF > player.GetPlayerPos().x - CORRECTION_HALF &&
            obstaclePosY - obstacleHeight * HALF < player.GetPlayerPos().y + CORRECTION_HALF &&
            obstaclePosY + obstacleHeight * HALF > player.GetPlayerPos().y - CORRECTION_HALF)
        {
            Debug.Log("Collision~~~~~~~~~~~");
        }

    }

}
