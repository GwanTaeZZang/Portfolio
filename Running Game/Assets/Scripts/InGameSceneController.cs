using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameSceneController : MonoBehaviour
{
    private const int CAPACITY = 10;
    private const int PLAYER_WIDTH = 1;
    private const int PLAYER_HEIGHT = 1;
    private const float HALF = 0.5f;
    private const int FLOOR_MIN_WIDTH = 4;
    private const int FLOOR_MAX_WIDTH = 9;
    private const int FLOOR_MIN_Y = -2;
    private const int FLOOR_MAX_Y = 2;
    private const int FLOOR_BETWEEN_MIN = 2;
    private const int FLOOR_BETWEEN_MAX = 5;
    private const float CORRECTION_VALUE = 0.45f;
    private const float NONE_GROUND_VALUE = -30f;
    private const float CORRECTION_HALF = 0.4f;

    [SerializeField] private Transform playerObj;
    [SerializeField] private Transform floorParent;
    [SerializeField] private Transform obstacleParent;
    [SerializeField] private Transform coinParent;

    private List<Floor> floorList = new List<Floor>(); //.. TODO :: FloorController
    //private List<Cactus> cactusList = new List<Cactus>(); //.. TODO :: ObstacleController
    private ObstacleController obstacleCtrl;
    private CoinController coinCtrl;

    private int floorListCount;
    //private Floor startFloor;
    private Floor lastFloor;

    //private int setCactusIdx;
    //private int collisionCactusIdx;
    private int collisionFloorIdx;
    private int frontFloorIdx;
    //private int frontCactusIdx;

    private Player player;
    private SpriteRenderer leftFloorPart;
    private SpriteRenderer middleFloorPart;
    private SpriteRenderer rightFloorPart;
    //private SpriteRenderer cactus;

    private float repositionX;

    private void Awake()
    {
        repositionX = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;

        leftFloorPart = Resources.Load<SpriteRenderer>("Prefab/Floor/Single Left");
        middleFloorPart = Resources.Load<SpriteRenderer>("Prefab/Floor/Single Middle");
        rightFloorPart = Resources.Load<SpriteRenderer>("Prefab/Floor/Single Right");
        //cactus = Resources.Load<SpriteRenderer>("Prefab/Obstacle/Cactus/Cactus");

        player = new Player(playerObj);
        //startFloor = new Floor(leftFloorPart, middleFloorPart, rightFloorPart, floorParent, true);

        obstacleCtrl = new ObstacleController(obstacleParent, player, repositionX);
        coinCtrl = new CoinController(coinParent, player, repositionX);
    }

    private void Start()
    {
        CreateFloor();
        //CreateCactus();
        SetFloorPosition();
    }

    private void Update()
    {
        MoveFloors();
        //MoveCactus();

        player.MovePlayer();

        UpdateCurrentCollisionFloor();
        CheckCollisionFloor();

        //UpdateCurrentCollisionCactus();
        //CheckCollisionCactus();

        obstacleCtrl.ObstacleUpdate();
    }

    private void CreateFloor()
    {
        // Start Floor
        floorList.Add(new Floor(leftFloorPart, middleFloorPart, rightFloorPart, floorParent, true));

        for(int i =0; i < CAPACITY -1; i++)
        {
            floorList.Add(new Floor(leftFloorPart, middleFloorPart, rightFloorPart, floorParent));
        }

        floorListCount = floorList.Count;
    }

    //private void CreateCactus()
    //{
    //    for(int i = 0; i < CAPACITY; i++)
    //    {
    //        cactusList.Add(new Cactus(cactus, cactusParent));
    //    }
    //    frontCactusIdx = 0;
    //    collisionCactusIdx = 0;
    //}

    private void SetFloorPosition()
    {
        for(int i =0; i < floorListCount; i++)
        {
            Floor floor = floorList[i];
            if ( i == 0)
            {
                // Start Floor
                floor.SetFloorPosition(0,-1);
                frontFloorIdx = i;
                collisionFloorIdx = i;
            }
            else
            {
                ReSizeFloor(floor);
                SetRandomFloorPos(floor);
            }
            lastFloor = floor;
        }
    }

    private void MoveFloors()
    {
        for (int i = 0; i < floorListCount; i++)
        {
            Floor floor = floorList[i];
            floor.MoveFloor();

            if (!floor.IsVisible())
            {
                ReSizeFloor(floor);
                SetRandomFloorPos(floor);
                floor.SetFloorVisible(true);
                lastFloor = floor;
            }
        }
    }

    //private void MoveCactus()
    //{
    //    for(int i =0; i < CAPACITY; i++)
    //    {
    //        cactusList[i].MoveCactus();
    //    }
    //}

    private void ReSizeFloor(Floor _floor)
    {
        _floor.SetFloorSize(new Vector2(GetRandomValue(FLOOR_MIN_WIDTH, FLOOR_MAX_WIDTH), 1));
    }

    private void SetRandomFloorPos(Floor _floor)
    {
        float posX = lastFloor.GetXPos() + (lastFloor.GetFloorWidth() * HALF);
        float betweenX = GetRandomValue(FLOOR_BETWEEN_MIN, FLOOR_BETWEEN_MAX);
        float randomBetweenX = betweenX + (_floor.GetFloorWidth() * HALF);
        float randomY = GetRandomValue(FLOOR_MIN_Y, FLOOR_MAX_Y);
        _floor.SetFloorPosition(posX + randomBetweenX, randomY);
        _floor.SetBetween(betweenX);


        //obstacleCtrl.SetRandomCactusPos(_floor);
        //obstacleCtrl.SetRandomJumpDinoPos(_floor);
        //obstacleCtrl.SetRandomArrowPos(_floor);
        obstacleCtrl.SetRandomObstaclePos(_floor);
        //SetRandomCactusPos(_floor);
    }

    private void UpdateCurrentCollisionFloor()
    {
        Floor curFloor = floorList[collisionFloorIdx];
        if (player.GetPlayerPos().x - HALF > curFloor.GetXPos() + (curFloor.GetFloorWidth()* HALF))
        {
            frontFloorIdx++;
            collisionFloorIdx = frontFloorIdx % floorListCount;
        }
    }

    private void CheckCollisionFloor()
    {
        AABB curFloor = floorList[collisionFloorIdx].GetAABB();
        float curFloorPosX = curFloor.pos.x;
        float curFloorPosY = curFloor.pos.y;
        float curFloorWidth = curFloor.width;
        float curFloorHeight = curFloor.height;

        if (curFloorPosX - curFloorWidth * HALF              < player.GetPlayerPos().x + PLAYER_WIDTH * CORRECTION_HALF &&
            curFloorPosX + curFloorWidth * HALF              > player.GetPlayerPos().x - PLAYER_WIDTH * CORRECTION_HALF &&
            curFloorPosY + curFloorHeight * CORRECTION_VALUE < player.GetPlayerPos().y - PLAYER_HEIGHT * CORRECTION_VALUE &&
            curFloorPosY + curFloorHeight * HALF            >= player.GetPlayerPos().y - PLAYER_HEIGHT * HALF)
        {
            player.SetGroundPosY(curFloorPosY + curFloorHeight);
            player.SetIsGround(true);
            player.PlayerPosYInterpolation(curFloorPosY + curFloorHeight);
        }
        else
        {
            player.SetGroundPosY(NONE_GROUND_VALUE);
            player.SetIsGround(false);
        }
    }

    //private void SetRandomCactusPos(Floor _floor)
    //{
    //    // Cactus Position Setting
    //    AABB aabb = _floor.GetAABB();
    //    if (aabb.width > 3)
    //    {
    //        bool isSet = cactusList[setCactusIdx].SetPosition(_floor);
    //        if (isSet)
    //        {
    //            setCactusIdx++;
    //            setCactusIdx = setCactusIdx % CAPACITY;
    //        }
    //    }
    //}

    //private void UpdateCurrentCollisionCactus()
    //{
    //    Cactus curCactus = cactusList[collisionCactusIdx];
    //    if (player.GetPlayerPos().x - HALF > curCactus.GetPos().x + (curCactus.GetCactusWidth() * HALF))
    //    {
    //        frontCactusIdx++;
    //        collisionCactusIdx = frontCactusIdx % CAPACITY;
    //    }

    //}

    //private void CheckCollisionCactus()
    //{
    //    AABB curCactus = cactusList[collisionCactusIdx].GetCactusAABB();
    //    float cactusPosX = curCactus.pos.x;
    //    float cactusPosY = curCactus.pos.y;
    //    float cactusWidth = curCactus.width;
    //    float cactusHeight = curCactus.height;

    //    if(cactusPosX - cactusWidth * CORRECTION_HALF < player.GetPlayerPos().x + PLAYER_WIDTH * CORRECTION_HALF &&
    //        cactusPosX + cactusWidth * CORRECTION_HALF > player.GetPlayerPos().x - PLAYER_WIDTH * CORRECTION_HALF &&
    //        cactusPosY - cactusHeight * CORRECTION_HALF < player.GetPlayerPos().y + PLAYER_HEIGHT * CORRECTION_HALF &&
    //        cactusPosY + cactusHeight * CORRECTION_HALF > player.GetPlayerPos().y - PLAYER_HEIGHT * CORRECTION_HALF)
    //    {
    //        Debug.Log("Collision~~~~~~~~~~~");
    //    }

    //}

    private int GetRandomValue(int _min, int max)
    {
        return Random.Range(_min, max);
    }
}
