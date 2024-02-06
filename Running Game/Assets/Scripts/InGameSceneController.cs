using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameSceneController : MonoBehaviour
{
    private const int CAPICITER = 10;
    private const int PLAYER_WIDTH = 1;
    private const int PLAYER_HEIGHT = 1;
    private const float HALF = 0.5f;
    private const int FLOOR_MIN_WIDTH = 0;
    private const int FLOOR_MAX_WIDTH = 4;
    private const int FLOOR_MIN_Y = -2;
    private const int FLOOR_MAX_Y = 2;
    private const int FLOOR_BETWEEN_MIN = 2;
    private const int FLOOR_BETWEEN_MAX = 5;
    private const float CORRECTION_VALUE = 0.45f;
    private const float NONE_GROUND_VALUE = -30f;

    [SerializeField] private Transform playerObj;
    [SerializeField] private Transform floorParent;
    [SerializeField] private Transform cactusParent;

    private List<Floor> floorList = new List<Floor>();
    private List<Cactus> cactusList = new List<Cactus>();

    private int floorListCount;
    private int cactusListCount;
    //private Floor startFloor;
    private Floor lastFloor;
    private int curCactusIdx;
    private int curFloorIdx;
    private int frontFloorIdx;
    private Player player;
    private SpriteRenderer leftFloorPart;
    private SpriteRenderer middleFloorPart;
    private SpriteRenderer rightFloorPart;
    private SpriteRenderer cactus;


    private void Awake()
    {
        leftFloorPart = Resources.Load<SpriteRenderer>("Prefab/Floor/Single Left");
        middleFloorPart = Resources.Load<SpriteRenderer>("Prefab/Floor/Single Middle");
        rightFloorPart = Resources.Load<SpriteRenderer>("Prefab/Floor/Single Right");
        cactus = Resources.Load<SpriteRenderer>("Prefab/Cactus/Cactus");


        player = new Player(playerObj);
        //startFloor = new Floor(leftFloorPart, middleFloorPart, rightFloorPart, floorParent, true);
    }

    private void Start()
    {
        CreateFloor();
        CreateCactus();
        SetFloorPosition();
    }

    private void Update()
    {
        MoveFloors();
        MoveCactus();

        player.MovePlayer();

        UpdateCurrentFloor();
        CheckCollisionFloor();
    }

    private void CreateFloor()
    {
        // Start Floor
        floorList.Add(new Floor(leftFloorPart, middleFloorPart, rightFloorPart, floorParent, true));

        for(int i =0; i < CAPICITER; i++)
        {
            floorList.Add(new Floor(leftFloorPart, middleFloorPart, rightFloorPart, floorParent));
        }

        floorListCount = floorList.Count;
    }

    private void CreateCactus()
    {
        for(int i = 0; i < CAPICITER; i++)
        {
            cactusList.Add(new Cactus(cactus, cactusParent));
        }
    }

    private void SetFloorPosition()
    {
        for(int i =0; i < floorListCount; i++)
        {
            if( i == 0)
            {
                // Start Floor
                floorList[i].SetFloorPosition(new Vector2(0, -1));
                frontFloorIdx = i;
                curFloorIdx = i;
            }
            else
            {
                ReSizeFloor(floorList[i]);
                SetRandomFloorPos(floorList[i]);
            }
            lastFloor = floorList[i];
        }
    }

    private void MoveFloors()
    {
        for (int i = 0; i < floorListCount; i++)
        {
            floorList[i].MoveFloor();

            if (!floorList[i].IsVisible())
            {
                ReSizeFloor(floorList[i]);
                SetRandomFloorPos(floorList[i]);
                floorList[i].SetFloorVisible(true);
                lastFloor = floorList[i];
            }
        }
    }

    private void MoveCactus()
    {
        for(int i =0; i < CAPICITER; i++)
        {
            cactusList[i].MoveCactus();
        }
    }

    private void ReSizeFloor(Floor _floor)
    {
        _floor.SetFloorSize(new Vector2(GetRandomValue(FLOOR_MIN_WIDTH, FLOOR_MAX_WIDTH), 1));
    }

    private void SetRandomFloorPos(Floor _floor)
    {
        _floor.SetFloorPosition(
            new Vector2(
                lastFloor.GetXPos() + (lastFloor.GetFloorWidth() * HALF) + GetRandomValue(FLOOR_BETWEEN_MIN, FLOOR_BETWEEN_MAX) + (_floor.GetFloorWidth() * HALF),
                GetRandomValue(FLOOR_MIN_Y, FLOOR_MAX_Y)
                )
            );


        // Cactus Position Setting
        AABB aabb = _floor.GetAABB();
        if(aabb.width > 3)
        {
            cactusList[curCactusIdx].SetPosition(_floor);
            curCactusIdx++;
            curCactusIdx = curCactusIdx % CAPICITER;
        }
    }

    private void UpdateCurrentFloor()
    {
        Floor curFloor = floorList[curFloorIdx];
        if (player.GetPlayerPos().x - HALF > curFloor.GetXPos() + (curFloor.GetFloorWidth()* HALF))
        {
            frontFloorIdx++;
            curFloorIdx = frontFloorIdx % floorListCount;
        }
    }

    private void CheckCollisionFloor()
    {
        AABB curFloor = floorList[curFloorIdx].GetAABB();
        float curFloorPosX = curFloor.pos.x;
        float curFloorPosY = curFloor.pos.y;
        float curFloorWidth = curFloor.width;
        float curFloorHeight = curFloor.height;

        if (curFloorPosX - curFloorWidth * HALF              < player.GetPlayerPos().x + PLAYER_WIDTH * HALF &&
            curFloorPosX + curFloorWidth * HALF              > player.GetPlayerPos().x - PLAYER_WIDTH * HALF &&
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

    private int GetRandomValue(int _min, int max)
    {
        return Random.Range(_min, max);
    }
}
