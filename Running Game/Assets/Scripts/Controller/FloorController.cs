using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController
{
    private const int CAPACITY = 10;
    private const int DOUBLE = 2;
    private const int FLOOR_MIN_WIDTH = 2;
    private const int FLOOR_MAX_WIDTH = 5;
    private const float HALF = 0.5f;
    private const int FLOOR_MIN_Y = -2;
    private const int FLOOR_MAX_Y = 2;
    private const int FLOOR_BETWEEN_MIN = 3;
    private const int FLOOR_BETWEEN_MAX = 5;
    private const float CORRECTION_VALUE = 0.45f;
    private const float NONE_GROUND_VALUE = -30f;
    private const float CORRECTION_HALF = 0.4f;
    private const int PLAYER_WIDTH = 1;
    private const int PLAYER_HEIGHT = 1;

    public delegate void SetFloorDelegate(Floor _floor);
    public SetFloorDelegate setFloorEvent;

    private List<Floor> floorList = new List<Floor>();

    private SpriteRenderer[] floorPartsSpriteArr;
    private SpriteRenderer leftFloorPart;
    private SpriteRenderer middleFloorPart;
    private SpriteRenderer rightFloorPart;

    private int floorListCount;
    private Floor lastFloor;
    private int collisionFloorIdx;
    private int frontFloorIdx;
    private Transform floorParent;
    private Player player;
    private float repositionX;



    public FloorController(Transform _parent, Player _player, float _reposX, SetFloorDelegate _event)
    {
        floorPartsSpriteArr = new SpriteRenderer[]
        {
            Resources.Load<SpriteRenderer>("Prefab/Floor/Single Left"),
            Resources.Load<SpriteRenderer>("Prefab/Floor/Single Middle"),
            Resources.Load<SpriteRenderer>("Prefab/Floor/Single Right")
        };
        //leftFloorPart = Resources.Load<SpriteRenderer>("Prefab/Floor/Single Left");
        //middleFloorPart = Resources.Load<SpriteRenderer>("Prefab/Floor/Single Middle");
        //rightFloorPart = Resources.Load<SpriteRenderer>("Prefab/Floor/Single Right");

        setFloorEvent = _event;

        floorParent = _parent;
        player = _player;
        repositionX = _reposX;

        CreateFloor();
        SetFloorPosition();
    }

    public void FixedUpdateFloor()
    {
        UpdateCurrentCollisionFloor();
        CheckCollisionFloor();
        MoveFloors();
    }

    private void CreateFloor()
    {
        //.. TODO :: 이렇게 할거면 CAPACITY - 1 보 1부터 시작 i 조건 비교구문에서 계속 -1 연산 추가됨
        // Start Floor
        floorList.Add(new Floor(floorPartsSpriteArr, floorParent, repositionX, true));

        for (int i = 0; i < CAPACITY - 1; i++)
        {
            floorList.Add(new Floor(floorPartsSpriteArr, floorParent, repositionX));
        }

        floorListCount = floorList.Count;
    }

    private void SetFloorPosition()
    {
        // start floor
        floorList[0].SetFloorPosition(0, -1);
        frontFloorIdx = 0;
        collisionFloorIdx = 0;
        lastFloor = floorList[0];

        for (int i = 1; i < floorListCount; i++)
        {
            Floor floor = floorList[i];
            //if (i == 0)
            //{
            //    // Start Floor
            //    floor.SetFloorPosition(0, -1);
            //    frontFloorIdx = i;
            //    collisionFloorIdx = i;
            //}
            //else
            //{
            //}

            ReSizeFloor(floor);
            SetRandomFloorPos(floor);
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

    private void ReSizeFloor(Floor _floor)
    {
        _floor.SetFloorSize(new Vector2(GetRandomValue(FLOOR_MIN_WIDTH, FLOOR_MAX_WIDTH) * DOUBLE, 1));
    }

    private void SetRandomFloorPos(Floor _floor)
    {
        int posX = (int)lastFloor.GetXPos() + (int)(lastFloor.GetFloorWidth() * HALF);
        int randomBetweenX = GetRandomValue(FLOOR_BETWEEN_MIN, FLOOR_BETWEEN_MAX) + (int)(_floor.GetFloorWidth() * HALF);
        float randomY = GetRandomValue(FLOOR_MIN_Y, FLOOR_MAX_Y);

        _floor.SetFloorPosition(posX + randomBetweenX, randomY);

        //.. TODO :: 수식 정리  // correction
        float newSetFloorLeftX = _floor.GetXPos() - _floor.GetFloorWidth() * HALF;
        float lastFloorRightX = lastFloor.GetXPos() + lastFloor.GetFloorWidth() * HALF;
        float betweenX = newSetFloorLeftX - lastFloorRightX;
        _floor.SetBetween(betweenX);


        setFloorEvent?.Invoke(_floor);
    }

    private void UpdateCurrentCollisionFloor()
    {
        Floor curFloor = floorList[collisionFloorIdx];
        if (player.GetPlayerPos().x - HALF > curFloor.GetXPos() + (curFloor.GetFloorWidth() * HALF))
        {
            frontFloorIdx++;
            collisionFloorIdx = frontFloorIdx % floorListCount;
        }
    }

    private void CheckCollisionFloor()
    {

        //Rect floorRect = floorList[collisionFloorIdx].GetRect();

        //floorRect.DrawDebugLine();

        //Debug.Log(floorRect.y);
        //Debug.Log(floorRect.position.y);
        //Debug.Log(floorRect.height);
        ////Debug.Log(floorRect.Overlaps(player.GetRect()));

        ////player.GetRect().DrawDebugLine();
        //Rect aa = player.GetRect();
        //aa.DrawDebugLine();

        //if (aa.Overlaps(floorRect))
        //{
        //    Debug.Log("dksjflsdjfldsjfldsjkflsdjlfsk");
        //    player.SetGroundPosY(floorRect.position.y + 0.5f);
        //    player.PlayerPosYInterpolation(floorRect.position.y + 0.5f);
        //    player.SetIsGround(true);
        //}
        //else
        //{
        //    Debug.Log("09098080980809809808098098");
        //    player.SetGroundPosY(NONE_GROUND_VALUE);
        //    player.SetIsGround(false);
        //}


        //if (posX - width * HALF < player.GetPlayerPos().x + PLAYER_WIDTH * CORRECTION_HALF &&
        //    posX + width * HALF > player.GetPlayerPos().x - PLAYER_WIDTH * CORRECTION_HALF &&
        //    posY + height * CORRECTION_VALUE < player.GetPlayerPos().y - PLAYER_HEIGHT * CORRECTION_VALUE &&
        //    posY + height * HALF >= player.GetPlayerPos().y - PLAYER_HEIGHT * HALF)
        //{
        //    player.SetGroundPosY(posY + height);
        //    player.SetIsGround(true);
        //    player.PlayerPosYInterpolation(posY + height);
        //}
        //else
        //{
        //    player.SetGroundPosY(NONE_GROUND_VALUE);
        //    player.SetIsGround(false);
        //}







        AABB curFloor = floorList[collisionFloorIdx].GetAABB();
        float curFloorPosX = curFloor.pos.x;
        float curFloorPosY = curFloor.pos.y;
        float curFloorWidth = curFloor.width;
        float curFloorHeight = curFloor.height;

        if (curFloorPosX - curFloorWidth * HALF < player.GetPlayerPos().x + PLAYER_WIDTH * CORRECTION_HALF &&
            curFloorPosX + curFloorWidth * HALF > player.GetPlayerPos().x - PLAYER_WIDTH * CORRECTION_HALF &&
            curFloorPosY + curFloorHeight * CORRECTION_VALUE < player.GetPlayerPos().y - PLAYER_HEIGHT * CORRECTION_VALUE &&
            curFloorPosY + curFloorHeight * HALF >= player.GetPlayerPos().y - PLAYER_HEIGHT * HALF)
        {
            player.SetGroundPosY(curFloorPosY + curFloorHeight);
            player.SetIsGround(true);
            player.PlayerPosYInterpolation(curFloorPosY + curFloorHeight);
        }
        else
        {
            player.SetGroundPosY(NONE_GROUND_VALUE);
            player.SetIsGround(false);
            //player.SetHp((int)NONE_GROUND_VALUE);
        }
    }

    private int GetRandomValue(int _min, int max)
    {
        return Random.Range(_min, max);
    }
}

