using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartItem : Item
{
    // 나중에 아이템 매니저가 테이블 로드해서 들고 있을 데이터 초기화 할 때 적용시킬 데이터
    private const int HEART_COEFFICIENT = 10;

    private SpriteRenderer heartSpriteRenderer;


    public override void Initialized(SpriteRenderer _item, Transform _parent, float _rePosX, float _inScenePosX)
    {
        heartSpriteRenderer = GameObject.Instantiate<SpriteRenderer>(_item, _parent);
        item = heartSpriteRenderer.transform;
        itemPos = new Vector2(CREATE_POS_X, CREATE_POS_Y);
        item.position = itemPos;

        speed = heartSpriteRenderer.sortingOrder;
        repositionX = _rePosX;
        inScenePosX = _inScenePosX;
        isCollision = false;


        SetVisible(false);
    }

    public override void SetPosition(Floor _floor)
    {
        width = heartSpriteRenderer.bounds.size.x;
        height = heartSpriteRenderer.bounds.size.y;


        // 아이템 생성 위치를 바닥 사이로 지정했을 때 로직 
        //float between = _floor.GetBetween();
        //AABB floorAABB = _floor.GetAABB();

        //Vector2 centerPos = floorAABB.pos;
        //centerPos.x = (centerPos.x - floorAABB.width * HALF) - between * HALF;
        //centerPos.y += 3;

        //itemPos = centerPos;

        // 아이템 생성 위치를 블럭 위로 지정했을 때 로직
        AABB floorAABB = _floor.GetAABB();
        float randomPosMin = (floorAABB.pos.x - floorAABB.width * HALF);
        float randomPosMax = (floorAABB.pos.x + floorAABB.width * HALF);

        itemPos.x = GetRandomValue(randomPosMin, randomPosMax);
        itemPos.y = floorAABB.pos.y + ITEM_POSY;

        SetVisible(true);

    }

    public override void CollisionItem(Player _player)
    {
        ItemModel item = ItemManager.getInstance.GetItemModel(ITEM_TYPE.heart);
        int hp = item.level / HEART_COEFFICIENT + 1;
        _player.SetHp(hp);

        Debug.Log("Collision Heart Item");
        Debug.Log("Heart Item Level = " + item.level);
        Debug.Log("HP Up = " + hp);

    }

}
