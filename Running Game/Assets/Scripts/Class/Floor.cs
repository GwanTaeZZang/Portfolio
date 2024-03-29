using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct AABB
{
    public Vector2 pos;
    public float width;
    public float height;
}

public class Floor
{
    private const float START_FLOOR_SIZE = 20f;
    private const float HALF = 0.5f;
    private const int FLOOR_PART_LEFT = 0;
    private const int FLOOR_PART_MIDDLE = 1;
    private const int FLOOR_PART_RIGHT = 2;
    private const float FLOOR_CORRECTION_HEIGHT = 0.5f;

    private Transform floor;

    private SpriteRenderer leftSingleFloor;
    private SpriteRenderer middleSingleFloor;
    private SpriteRenderer rightSingleFloor;

    private float width;
    private int moveSpeed;
    private bool isStartFloor;
    private float repositionX;
    private float betweenX;

    private Vector2 curPos = Vector2.zero;
    private Rect floorRect;

    //.. TODO :: Parameter 가 너무 많음, 복합 자료형 이용해서 줄일 수 있도록  // correction
    public Floor(SpriteRenderer[] _floorPartArr, Transform _floorGroup , float _reposX, bool _isStartFloor = false)
    {

        floor = new GameObject("Floor").transform;
        floor.position = Vector3.zero;
        floor.SetParent(_floorGroup);

        curPos = floor.position;

        floorRect = new Rect(0, 0, 0, 0);

        leftSingleFloor = GameObject.Instantiate<SpriteRenderer>(_floorPartArr[FLOOR_PART_LEFT], floor);
        middleSingleFloor = GameObject.Instantiate<SpriteRenderer>(_floorPartArr[FLOOR_PART_MIDDLE], floor);
        rightSingleFloor = GameObject.Instantiate<SpriteRenderer>(_floorPartArr[FLOOR_PART_RIGHT], floor);

        moveSpeed = leftSingleFloor.sortingOrder;
        isStartFloor = _isStartFloor;
        repositionX = _reposX;

        Initialized();
    }

    private void Initialized()
    {
        if (isStartFloor)
        {
            middleSingleFloor.size = new Vector2(START_FLOOR_SIZE, 1);
        }
        else
        {
            //float randomWidth = Random.Range(0, 5);
            middleSingleFloor.size = new Vector2(1, 1);
        }

        ResetFloorPartsPos();
        SetFloorWidth();
    }

    public void MoveFloor()
    {

        if (curPos.x + width * HALF < repositionX)
        {
            SetFloorVisible(false);
        }
        else
        {
            curPos.x += Time.deltaTime * moveSpeed * -HALF;
            floor.position = curPos;
        }

    }

    public void SetFloorPosition(int _x, float _y)
    {
        //Debug.Log(_x);
        curPos.x = _x;
        curPos.y = _y;
        floor.position = curPos;
    }
    public void SetBetween(float _between)
    {
        betweenX = _between;
    }

    public void SetFloorSize(Vector2 _size)
    {
        middleSingleFloor.size = _size;
        ResetFloorPartsPos();
        SetFloorWidth();
    }

    public void SetFloorVisible(bool _visible)
    {
        floor.gameObject.SetActive(_visible);
    }

    public bool IsVisible()
    {
        return floor.gameObject.activeSelf;
    }

    public float GetFloorWidth()
    {
        return width;
    }

    public float GetXPos()
    {
        return curPos.x;
    }
    public float GetBetween()
    {
        return betweenX;
    }

    public AABB GetAABB()
    {
        AABB aabb;
        aabb.pos = curPos;
        aabb.width = width;
        aabb.height = 1; // 지금은 높이가 무조건 1 임시 데이터
        return aabb;
    }
    public Rect GetRect()
    {
        floorRect.Set(curPos.x - width * HALF ,curPos.y + 1 * HALF ,width , FLOOR_CORRECTION_HEIGHT);
        return floorRect;
    }

    private void ResetFloorPartsPos()
    {
        leftSingleFloor.transform.localPosition = new Vector2(-((middleSingleFloor.size.x * HALF) + (leftSingleFloor.size.x * HALF)), 0);
        rightSingleFloor.transform.localPosition = new Vector2((middleSingleFloor.size.x * HALF) + (leftSingleFloor.size.x * HALF), 0);
    }

    private void SetFloorWidth()
    {
        width = middleSingleFloor.size.x + leftSingleFloor.size.x + rightSingleFloor.size.x;
    }

}
