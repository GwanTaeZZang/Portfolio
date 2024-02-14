using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController
{
    private const int CAPACITY = 10;
    private const float CORRECTION_HALF = 0.4f;
    private const float HALF = 0.5f;
    private const int INIT_CACTUS_DIX = 0;
    private const int INIT_JUMPDINO_DIX = 10;

    private List<Obstacle> obstacleList;
    private SpriteRenderer cactus;
    private SpriteRenderer dino;
    private Player player;

    private int count;
    private int setCactusIdx;
    private int setDinoIdx;
    private int collisionCactusIdx;
    private int collisionJumpDinoIdx;
    //private int frontCactusIdx;
    private float repositionX;

    private Camera camera;

    public ObstacleController(Transform _parent, Player _player)
    {
        camera = Camera.main;

        cactus = Resources.Load<SpriteRenderer>("Prefab/Obstacle/Cactus/Cactus");
        dino = Resources.Load<SpriteRenderer>("Prefab/Obstacle/Dino/Dino");

        obstacleList = new List<Obstacle>();
        player = _player;
        repositionX = camera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;

        CreateObstacle<Cactus>(cactus, _parent, repositionX);
        CreateObstacle<JumpDino>(dino, _parent, repositionX);

        count = obstacleList.Count;

        setCactusIdx = INIT_CACTUS_DIX;
        setDinoIdx = INIT_JUMPDINO_DIX;
        collisionCactusIdx = INIT_CACTUS_DIX;
        collisionJumpDinoIdx = INIT_JUMPDINO_DIX;

        //Debug.Log(count);
    }

    public void ObstacleUpdate()
    {
        MoveObstacle();
        UpdateCurrentCollisionCactus();
        CheckCollisionObstacle(collisionCactusIdx);
        UpdateCurrentCollisionJumpDino();
        CheckCollisionObstacle(collisionJumpDinoIdx);

        //CheckCollisionJumpDino();
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



    public void SetRandomCactusPos(Floor _floor)
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

    public void SetRandomDinoPos(Floor _floor)
    {
        if (_floor.GetBetween() > 3)
        {
            //Debug.Log(_floor.GetBetween());

            bool isSet = obstacleList[setDinoIdx].SetPosition(_floor);
            if (isSet)
            {
                //Debug.Log("세팅된 공룡 인덱스" + setDinoIdx);

                setDinoIdx++;
                setDinoIdx = (setDinoIdx % CAPACITY) + CAPACITY;
            }
        }
    }



    private void MoveObstacle()
    {
        for (int i = 0; i < count; i++)
        {
            obstacleList[i].Move();
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
