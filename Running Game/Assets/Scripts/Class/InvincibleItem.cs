using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleItem : Item
{
    // 나중에 아이템 매니저가 테이블 로드해서 들고 있을 데이터 초기화 할 때 적용시킬 데이터
    private const int INVINCIBLE_TIME_COEFFICIENT = 5;
    private const int INVINCIBLE_TIME_DEFULT = 5;


    private SpriteRenderer invincibleItemSpriteRenderer;

    public override void Initialized(SpriteRenderer _item, Transform _parent, float _rePosX, float _inScenePosX)
    {
        invincibleItemSpriteRenderer = GameObject.Instantiate<SpriteRenderer>(_item, _parent);
        item = invincibleItemSpriteRenderer.transform;
        itemPos = new Vector2(CREATE_POS_X, CREATE_POS_Y);
        item.position = itemPos;

        speed = invincibleItemSpriteRenderer.sortingOrder;
        repositionX = _rePosX;
        inScenePosX = _inScenePosX;
        isCollision = false;

        SetVisible(false);
    }

    public override void SetPosition(Floor _floor)
    {
        width = invincibleItemSpriteRenderer.bounds.size.x;
        height = invincibleItemSpriteRenderer.bounds.size.y;

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
        ItemModel item = ItemManager.getInstance.GetItemModel(ITEM_TYPE.magnet);
        float time = item.level / INVINCIBLE_TIME_COEFFICIENT + INVINCIBLE_TIME_DEFULT;
        _player.SetInvincibleEffect(time);

        Debug.Log("무적 아이템 먹음 ");
    }

}
