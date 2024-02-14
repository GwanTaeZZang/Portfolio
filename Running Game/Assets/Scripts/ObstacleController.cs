using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController
{
    private const int CAPACITY = 10;
    private const float CORRECTION_HALF = 0.4f;
    private const float HALF = 0.5f;
    private const int INIT_CACTUS_IDX = 0;
    private const int INIT_JUMPDINO_IDX = 10;
    private const int INIT_ARROW_IDX = 20;

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
    private float repositionX;

    private Camera camera;

    public ObstacleController(Transform _parent, Player _player)
    {
        camera = Camera.main;

        cactus = Resources.Load<SpriteRenderer>("Prefab/Obstacle/Cactus/Cactus");
        dino = Resources.Load<SpriteRenderer>("Prefab/Obstacle/Dino/Dino");
        arrow = Resources.Load<SpriteRenderer>("Prefab/Obstacle/Arrow/Arrow");

        obstacleList = new List<Obstacle>();
        player = _player;
        repositionX = camera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;

        CreateObstacle<Cactus>(cactus, _parent, repositionX);
        CreateObstacle<JumpDino>(dino, _parent, repositionX);
        CreateObstacle<Arrow>(arrow, _parent, repositionX);

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











    private void SetRandomCactusPos(Floor _floor)
    {
        // Cactus Position Setting
        AABB aabb = _floor.GetAABB();
        if (aabb.width > 3)
        {
            bool isSet = obstacleList[setCactusIdx].SetPosition(_floor);
            if (isSet)
            {
                //Debug.Log("세팅된 선인장 인덱스" + setCactusIdx  + "   선인장의 넓이 및 인덱스 : " + aabb.width  + aabb.pos);

                setCactusIdx++;
                setCactusIdx = setCactusIdx % CAPACITY;
            }
        }
    }

    private void SetRandomJumpDinoPos(Floor _floor)
    {
        if (_floor.GetBetween() > 3)
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

        if (obstaclePosX - obstacleWidth * CORRECTION_HALF < player.GetPlayerPos().x + CORRECTION_HALF &&
            obstaclePosX + obstacleWidth * CORRECTION_HALF > player.GetPlayerPos().x - CORRECTION_HALF &&
            obstaclePosY - obstacleHeight * CORRECTION_HALF < player.GetPlayerPos().y + CORRECTION_HALF &&
            obstaclePosY + obstacleHeight * CORRECTION_HALF > player.GetPlayerPos().y - CORRECTION_HALF)
        {
            Debug.Log("Collision~~~~~~~~~~~");
        }

    }


    //private void CheckCollisionJumpDino()
    //{
    //    AABB curJumpDino = obstacleList[collisionJumpDinoIdx].GetAABB();
    //    float jumpDinoPosX = curJumpDino.pos.x;
    //    float jumpDinoPosY = curJumpDino.pos.y;
    //    float jumpDinoWidth = curJumpDino.width;
    //    float jumpDinoHeight = curJumpDino.height;

    //    if (jumpDinoPosX - jumpDinoWidth * CORRECTION_HALF < player.GetPlayerPos().x + CORRECTION_HALF &&
    //        jumpDinoPosX + jumpDinoWidth * CORRECTION_HALF > player.GetPlayerPos().x - CORRECTION_HALF &&
    //        jumpDinoPosY - jumpDinoHeight * CORRECTION_HALF < player.GetPlayerPos().y + CORRECTION_HALF &&
    //        jumpDinoPosY + jumpDinoHeight * CORRECTION_HALF > player.GetPlayerPos().y - CORRECTION_HALF)
    //    {
    //        Debug.Log("JumpDino Collision~~~~~~~~~~~");

    //    }
    //}
}
