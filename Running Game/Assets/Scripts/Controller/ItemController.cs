using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController
{
    private const int CAPACITY = 10;
    private const int INIT_HEART_IDX = CAPACITY * 0;
    private const int INIT_MAGNET_IDX = CAPACITY * 1;
    private const float HALF = 0.5f;
    private const int SET_RANDOM_ITEM_MIN = 0;
    private const int SET_RANDOM_ITEM_MAX = 10;
    private const int SET_RANDOM_ITEM_TYPE_MIN = 0;
    private const int SET_RANDOM_ITEM_TYPE_MAX = 3;
    private const float CORRECTION_HALF = 0.3f;



    private List<Item> itemList;
    private Player player;

    private SpriteRenderer heartItmeSpriteRenderer;
    private SpriteRenderer magnetItmeSpriteRenderer;
    private int setHeartIdx = INIT_HEART_IDX;
    private int setMagnetIdx = INIT_MAGNET_IDX;
    private int itemListCount;

    public ItemController(Transform _parent, Player _player, float _reposX, float _inScenePosX)
    {
        itemList = new List<Item>();
        player = _player;

        heartItmeSpriteRenderer = Resources.Load<SpriteRenderer>("Prefab/Item/Heart");
        magnetItmeSpriteRenderer = Resources.Load<SpriteRenderer>("Prefab/Item/Magnet");

        CreateItem<HeartItem>(heartItmeSpriteRenderer, _parent, _reposX, _inScenePosX);
        CreateItem<MagnetItem>(magnetItmeSpriteRenderer, _parent, _reposX, _inScenePosX);

        itemListCount = itemList.Count;
    }

    private void CreateItem<T>(SpriteRenderer _item, Transform _parent, float _reposX, float _inScenePosX) where T : Item, new()
    {
        for(int i =0; i < CAPACITY; i++)
        {
            T item = new T();
            item.Initialized(_item, _parent, _reposX, _inScenePosX);
            itemList.Add(item);
        }
    }

    public void FixedItemUpdate()
    {
        MoveItem();
        UpdateCollisionItem();
    }

    public void SetRandomItemPos(Floor _floor)
    {

        //SetRandomHeartPos(_floor);

        int percentage = GetRandomValue(SET_RANDOM_ITEM_MIN, SET_RANDOM_ITEM_MAX);

        if (percentage == 0)
        {
            int type = GetRandomValue(SET_RANDOM_ITEM_TYPE_MIN, SET_RANDOM_ITEM_TYPE_MAX);
            SetRandomHeartPos(_floor);
            SetRandomMagnetPos(_floor);
        }
    }

    private void SetRandomHeartPos(Floor _floor)
    {
        itemList[setHeartIdx].SetPosition(_floor);
        setHeartIdx++;
        setHeartIdx = (setHeartIdx % CAPACITY) + INIT_HEART_IDX;
    }
    private void SetRandomMagnetPos(Floor _floor)
    {
        itemList[setMagnetIdx].SetPosition(_floor);
        setMagnetIdx++;
        setMagnetIdx = (setMagnetIdx % CAPACITY) + INIT_MAGNET_IDX;
    }



    private void MoveItem()
    {
        for(int i =0; i < itemListCount; i++)
        {
            itemList[i].Move();
        }
    }

    private void UpdateCollisionItem()
    {
        for (int i = 0; i < itemListCount; i++)
        {
            Item item = itemList[i];

            if (item.IsInScene())
            {
                AABB itemAABB = item.GetAABB();
                float PosX = itemAABB.pos.x;
                float PosY = itemAABB.pos.y;
                float Width = itemAABB.width;
                float Height = itemAABB.height;

                Vector2 playerPos = player.GetPlayerPos();

                if (PosX - Width * HALF < playerPos.x + HALF &&
                    PosX + Width * HALF > playerPos.x - HALF &&
                    PosY - Height * HALF < playerPos.y + HALF &&
                    PosY + Height * HALF > playerPos.y - HALF)
                {
                    item.SetIsCollision(true);
                    item.SetVisible(false);
                    item.SetIsInScene(false);

                    item.CollisionItem(player);
                }
            }
        }
    }


    public int GetRandomValue(int _min, int _max)
    {
        return Random.Range(_min, _max);
    }

}
