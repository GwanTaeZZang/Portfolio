using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController
{
    private const int CAPACITY = 10;
    private const float CORRECTION_HALF = 0.4f;
    private const float HALF = 0.5f;


    private List<Obstacle> obstacleList;
    private SpriteRenderer cactus;
    private SpriteRenderer dino;
    private Player player;

    private int count;
    private int setCactusIdx;
    private int setDinoIdx;
    private int collisionCactusIdx;
    private int frontCactusIdx;
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

        setCactusIdx = 0;
        setDinoIdx = 10;
        //Debug.Log(count);
    }

    public void ObstacleUpdate()
    {
        MoveObstacle();
        UpdateCurrentCollisionObstacle();
        CheckCollisionObstacle();
    }

    private void CreateObstacle<T>(SpriteRenderer _obstacle ,Transform _parent, float _reposX) where T : Obstacle, new()
    {
        for (int i = 0; i < CAPACITY; i++)
        {
            T obstacle = new T();
            obstacle.Initialized(_obstacle, _parent, _reposX);
            obstacleList.Add(obstacle);
        }
        frontCactusIdx = 0;
        collisionCactusIdx = 0;
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
        if(_floor.GetBetween() > 3)
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

    private void UpdateCurrentCollisionObstacle()
    {
        Obstacle curCactus = obstacleList[collisionCactusIdx];
        if (player.GetPlayerPos().x - HALF > curCactus.GetPos().x + (curCactus.GetWidth() * HALF))
        {
            frontCactusIdx++;
            collisionCactusIdx = frontCactusIdx % CAPACITY;
        }
    }

    private void CheckCollisionObstacle()
    {
        AABB curCactus = obstacleList[collisionCactusIdx].GetAABB();
        float cactusPosX = curCactus.pos.x;
        float cactusPosY = curCactus.pos.y;
        float cactusWidth = curCactus.width;
        float cactusHeight = curCactus.height;

        if (cactusPosX - cactusWidth * CORRECTION_HALF < player.GetPlayerPos().x + CORRECTION_HALF &&
            cactusPosX + cactusWidth * CORRECTION_HALF > player.GetPlayerPos().x - CORRECTION_HALF &&
            cactusPosY - cactusHeight * CORRECTION_HALF < player.GetPlayerPos().y + CORRECTION_HALF &&
            cactusPosY + cactusHeight * CORRECTION_HALF > player.GetPlayerPos().y - CORRECTION_HALF)
        {
            Debug.Log("Collision~~~~~~~~~~~");
        }

    }


}
