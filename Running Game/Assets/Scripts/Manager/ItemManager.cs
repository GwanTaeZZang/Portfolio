using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemModel
{
    public ITEM_TYPE type;
    public int level;
    public int price;
    public float effectAmount;
}
public enum ITEM_TYPE
{
    heart = 0,
    magnet = 1,
    count,
}
public class ItemManager : Singleton<ItemManager>
{
    private List<ItemModel> itemList;

    public override bool Initialize()
    {
        itemList = new List<ItemModel>();
        for(int i =0; i < (int)ITEM_TYPE.count; i++)
        {
            ItemModel model = new ItemModel();
            model.type = (ITEM_TYPE)i;
            model.level = 1;
            model.price = 500;
            model.effectAmount = 1;
            itemList.Add(model);
        }

        Debug.Log("아이템 매니저 초기화 완료 ");
        return base.Initialize();
    }

    public ItemModel ItemLevelUp(ITEM_TYPE _tpye)
    {
        ItemModel model = itemList[(int)_tpye];
        model.level++;

        return model;
    }

    public ItemModel GetItemModel(ITEM_TYPE _type)
    {
        return itemList[(int)_type];
    }
}
