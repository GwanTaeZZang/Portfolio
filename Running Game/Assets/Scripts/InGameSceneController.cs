using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameSceneController : MonoBehaviour
{
    private const int FLOOR_CAPICITER = 10;

    [SerializeField] private Transform playerObj;
    [SerializeField] private Transform floorParent;

    private List<Floor> floorList = new List<Floor>();
    //private Floor startFloor;
    private Floor lastFloor;
    private int curFloorIdx;
    private int frontFloorIdx;
    private Player player;
    private SpriteRenderer leftFloorPart;
    private SpriteRenderer middleFloorPart;
    private SpriteRenderer rightFloorPart;

    private void Awake()
    {
        leftFloorPart = Resources.Load<SpriteRenderer>("Prefab/Floor/Single Left");
        middleFloorPart = Resources.Load<SpriteRenderer>("Prefab/Floor/Single Middle");
        rightFloorPart = Resources.Load<SpriteRenderer>("Prefab/Floor/Single Right");
        player = new Player(playerObj);
        //startFloor = new Floor(leftFloorPart, middleFloorPart, rightFloorPart, floorParent, true);
    }

    private void Start()
    {
        CreateFloor();
        SetFloorPosition();
    }

    private void Update()
    {
        //startFloor.MoveFloor();
        MoveFloors();

        if (Input.GetKeyDown("space") && !player.GetisJump())
        {
            player.Jump();
        }
        else if (Input.GetKeyDown("space") && player.GetisJump())
        {
            player.DoubleJump();
        }
        player.Gravity();
        UpdateCurrentFloor();
        CheckCollisionFloor();
    }

    private void CreateFloor()
    {
        // Start Floor
        floorList.Add(new Floor(leftFloorPart, middleFloorPart, rightFloorPart, floorParent, true));

        for(int i =0; i < FLOOR_CAPICITER; i++)
        {
            floorList.Add(new Floor(leftFloorPart, middleFloorPart, rightFloorPart, floorParent));
        }
    }
    private void SetFloorPosition()
    {
        int count = floorList.Count;
        for(int i =0; i < count; i++)
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
                SetRandomFloorPos(floorList[i]);
            }
            lastFloor = floorList[i];
        }
    }
    private void MoveFloors()
    {
        int count = floorList.Count;
        for (int i = 0; i < count; i++)
        {
            if (!floorList[i].GetFloorVisible())
            {
                RePosFloor(floorList[i]);

                floorList[i].SetFloorVisible(true);
                lastFloor = floorList[i];
            }
            floorList[i].MoveFloor();
        }
    }
    private void RePosFloor(Floor _floor)
    {
        float randomWidth = Random.Range(0, 3);
        _floor.SetFloorSize(new Vector2((int)randomWidth, 1));

        SetRandomFloorPos(_floor);
    }

    private void SetRandomFloorPos(Floor _floor)
    {
        float randomY = Random.Range(-2, 2);
        _floor.SetFloorPosition(new Vector2(lastFloor.GetXPos() + (lastFloor.GetFloorWidth() * 0.5f) + 2 + (_floor.GetFloorWidth() * 0.5f), randomY));

    }

    private void UpdateCurrentFloor()
    {
        Floor curFloor = floorList[curFloorIdx];
        if (player.GetPlayerPos().x -0.5f > curFloor.GetXPos() + (curFloor.GetFloorWidth()*0.5))
        {
            // 현재 플레이어 바로 밑 발판 사라짐
            //Debug.Log(curFloorIdx + "발판 사라짐 ");
            frontFloorIdx++;
            curFloorIdx = frontFloorIdx % 11;
        }
    }

    private void CheckCollisionFloor()
    {
        AABB curFloor = floorList[curFloorIdx].GetAABB();
        if(curFloor.pos.x - curFloor.width * 0.5f < player.GetPlayerPos().x + 0.5f &&
            curFloor.pos.x + curFloor.width * 0.5f > player.GetPlayerPos().x - 0.5f &&
            curFloor.pos.y - curFloor.height * 0.5f < player.GetPlayerPos().y + 0.5f &&
            curFloor.pos.y + curFloor.height * 0.5f > player.GetPlayerPos().y - 0.5f)
        {
            //Debug.Log("충돌했쪄" + curFloor.pos.y + curFloor.height);
            player.SetGroundPosY(curFloor.pos.y + curFloor.height);
            player.SetIsGround(true);
            player.PlayerPosYInterpolation(curFloor.pos.y + curFloor.height);
        }
        else
        {
            player.SetGroundPosY(-30f);
            player.SetIsGround(false);
        }
    }
}
