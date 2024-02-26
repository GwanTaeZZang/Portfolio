using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameSceneController : MonoBehaviour
{
    //private const int CAPACITY = 10;
    //private const int PLAYER_WIDTH = 1;
    //private const int PLAYER_HEIGHT = 1;
    //private const float HALF = 0.5f;
    //private const int FLOOR_MIN_WIDTH = 4;
    //private const int FLOOR_MAX_WIDTH = 9;
    //private const int FLOOR_MIN_Y = -2;
    //private const int FLOOR_MAX_Y = 2;
    //private const int FLOOR_BETWEEN_MIN = 2;
    //private const int FLOOR_BETWEEN_MAX = 5;
    //private const float CORRECTION_VALUE = 0.45f;
    //private const float NONE_GROUND_VALUE = -30f;
    //private const float CORRECTION_HALF = 0.4f;

    [SerializeField] private Transform playerObj;
    [SerializeField] private Transform floorParent;
    [SerializeField] private Transform obstacleParent;
    [SerializeField] private Transform coinParent;
    [SerializeField] private Text scoreAmount;

    //private List<Floor> floorList = new List<Floor>(); //.. TODO :: FloorController
    private FloorController floorCtrl;
    private ObstacleController obstacleCtrl;
    private CoinController coinCtrl;

    //private int floorListCount;
    //private Floor lastFloor;

    //private int collisionFloorIdx;
    //private int frontFloorIdx;

    private Player player;
    //private SpriteRenderer leftFloorPart;
    //private SpriteRenderer middleFloorPart;
    //private SpriteRenderer rightFloorPart;

    private float repositionX;

    private void Awake()
    {
        repositionX = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;

        //leftFloorPart = Resources.Load<SpriteRenderer>("Prefab/Floor/Single Left");
        //middleFloorPart = Resources.Load<SpriteRenderer>("Prefab/Floor/Single Middle");
        //rightFloorPart = Resources.Load<SpriteRenderer>("Prefab/Floor/Single Right");

        player = new Player(playerObj);


        coinCtrl = new CoinController(coinParent, player, repositionX);

        obstacleCtrl = new ObstacleController(obstacleParent, player, repositionX, SetCoinEvnent);

        floorCtrl = new FloorController(floorParent, player, repositionX, SetObstacleEvent);

        coinCtrl.scoreEvnet = UpdateScore;
    }

    private void Start()
    {
        //CreateFloor();
        //SetFloorPosition();
    }

    private void Update()
    {
        player.MovePlayer();
    }

    private void FixedUpdate()
    {
        floorCtrl.UpdateFloor();
        obstacleCtrl.ObstacleUpdate();
        coinCtrl.UpdateCoin();
        player.Gravity();
    }

    private void SetObstacleEvent(Floor _floor)
    {
        obstacleCtrl.SetRandomObstaclePos(_floor);
    }

    private void SetCoinEvnent(Floor _floor, Obstacle _obstacle)
    {
        coinCtrl.SetCoinPosition(_floor, _obstacle);
    }

    private void UpdateScore(int _score)
    {
        scoreAmount.text = _score.ToString();
    }

    //private void CreateFloor()
    //{
    //    // Start Floor
    //    floorList.Add(new Floor(leftFloorPart, middleFloorPart, rightFloorPart, floorParent, true));

    //    for(int i =0; i < CAPACITY -1; i++)
    //    {
    //        floorList.Add(new Floor(leftFloorPart, middleFloorPart, rightFloorPart, floorParent));
    //    }

    //    floorListCount = floorList.Count;
    //}


    //private void SetFloorPosition()
    //{
    //    for(int i =0; i < floorListCount; i++)
    //    {
    //        Floor floor = floorList[i];
    //        if ( i == 0)
    //        {
    //            // Start Floor
    //            floor.SetFloorPosition(0,-1);
    //            frontFloorIdx = i;
    //            collisionFloorIdx = i;
    //        }
    //        else
    //        {
    //            ReSizeFloor(floor);
    //            SetRandomFloorPos(floor);
    //        }
    //        lastFloor = floor;
    //    }
    //}

    //private void MoveFloors()
    //{
    //    for (int i = 0; i < floorListCount; i++)
    //    {
    //        Floor floor = floorList[i];
    //        floor.MoveFloor();

    //        if (!floor.IsVisible())
    //        {
    //            ReSizeFloor(floor);
    //            SetRandomFloorPos(floor);
    //            floor.SetFloorVisible(true);
    //            lastFloor = floor;
    //        }
    //    }
    //}


    //private void ReSizeFloor(Floor _floor)
    //{
    //    _floor.SetFloorSize(new Vector2(GetRandomValue(FLOOR_MIN_WIDTH, FLOOR_MAX_WIDTH), 1));
    //}

    //private void SetRandomFloorPos(Floor _floor)
    //{
    //    float posX = lastFloor.GetXPos() + (lastFloor.GetFloorWidth() * HALF);
    //    float betweenX = GetRandomValue(FLOOR_BETWEEN_MIN, FLOOR_BETWEEN_MAX);
    //    float randomBetweenX = betweenX + (_floor.GetFloorWidth() * HALF);
    //    float randomY = GetRandomValue(FLOOR_MIN_Y, FLOOR_MAX_Y);
    //    _floor.SetFloorPosition(posX + randomBetweenX, randomY);
    //    _floor.SetBetween(betweenX);


    //    obstacleCtrl.SetRandomObstaclePos(_floor);
    //    coinCtrl.SetCoinPosition(_floor);
    //}

    //private void UpdateCurrentCollisionFloor()
    //{
    //    Floor curFloor = floorList[collisionFloorIdx];
    //    if (player.GetPlayerPos().x - HALF > curFloor.GetXPos() + (curFloor.GetFloorWidth()* HALF))
    //    {
    //        frontFloorIdx++;
    //        collisionFloorIdx = frontFloorIdx % floorListCount;
    //    }
    //}

    //private void CheckCollisionFloor()
    //{
    //    AABB curFloor = floorList[collisionFloorIdx].GetAABB();
    //    float curFloorPosX = curFloor.pos.x;
    //    float curFloorPosY = curFloor.pos.y;
    //    float curFloorWidth = curFloor.width;
    //    float curFloorHeight = curFloor.height;

    //    if (curFloorPosX - curFloorWidth * HALF              < player.GetPlayerPos().x + PLAYER_WIDTH * CORRECTION_HALF &&
    //        curFloorPosX + curFloorWidth * HALF              > player.GetPlayerPos().x - PLAYER_WIDTH * CORRECTION_HALF &&
    //        curFloorPosY + curFloorHeight * CORRECTION_VALUE < player.GetPlayerPos().y - PLAYER_HEIGHT * CORRECTION_VALUE &&
    //        curFloorPosY + curFloorHeight * HALF            >= player.GetPlayerPos().y - PLAYER_HEIGHT * HALF)
    //    {
    //        player.SetGroundPosY(curFloorPosY + curFloorHeight);
    //        player.SetIsGround(true);
    //        player.PlayerPosYInterpolation(curFloorPosY + curFloorHeight);
    //    }
    //    else
    //    {
    //        player.SetGroundPosY(NONE_GROUND_VALUE);
    //        player.SetIsGround(false);
    //    }
    //}


    //private int GetRandomValue(int _min, int max)
    //{
    //    return Random.Range(_min, max);
    
}
