using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopCanvasController : UIBaseController
{
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button[] itemLvUpBtnArr;
    [SerializeField] private Text[] itemLvAmountTextArr;
    [SerializeField] private Text[] itemLvPriceAmountTextArr;
    //[SerializeField] private Button heartItemLvUpBtn;
    //[SerializeField] private Button magnetItemLvUpBtn;


    //[SerializeField] private Text heartItemLvText;
    //[SerializeField] private Text heartItemLvUpPriceText;
    //[SerializeField] private Text magnetItemLvText;
    //[SerializeField] private Text magnetItemLvUpPriceText;


    protected override void Initialized()
    {
        closeBtn.onClick.AddListener(OnClickCloseBtn);
        ButtonEventBinding();
        //heartItemLvUpBtn.onClick.AddListener(OnClickHeartItemLvUpBtn);
        //magnetItemLvUpBtn.onClick.AddListener(OnClickMagnetItemLvUpBtn);

        //UpdateHeartItemElememt(ItemManager.getInstance.GetItemModel(ITEM_TYPE.heart));
        //UpdateMagnetItemElememt(ItemManager.getInstance.GetItemModel(ITEM_TYPE.magnet));

        UpdateItemElement(ItemManager.getInstance.GetItemModel(ITEM_TYPE.heart), ITEM_TYPE.heart);
        UpdateItemElement(ItemManager.getInstance.GetItemModel(ITEM_TYPE.magnet), ITEM_TYPE.magnet);
        UpdateItemElement(ItemManager.getInstance.GetItemModel(ITEM_TYPE.invincible), ITEM_TYPE.invincible);
    }

    private void ButtonEventBinding()
    {
        int count = itemLvUpBtnArr.Length;
        for (int i =0; i < count; i++)
        {
            int type = i;
            itemLvUpBtnArr[i].onClick.AddListener(() => OnClickLvUpBtn((ITEM_TYPE)type));
        }
    }

    private void OnClickCloseBtn()
    {
        UIManager.getInstance.Hide();
    }

    private void OnClickLvUpBtn(ITEM_TYPE _type)
    {
        Debug.Log(_type);
        ItemModel model = ItemManager.getInstance.ItemLevelUp(_type);
        UpdateItemElement(model, _type);
    }

    //private void OnClickHeartItemLvUpBtn()
    //{
    //    ItemModel model = ItemManager.getInstance.ItemLevelUp(ITEM_TYPE.heart);
    //    UpdateHeartItemElememt(model);
    //}
    //private void OnClickMagnetItemLvUpBtn()
    //{
    //    ItemModel model = ItemManager.getInstance.ItemLevelUp(ITEM_TYPE.magnet);
    //    UpdateMagnetItemElememt(model);
    //}

    private void UpdateItemElement(ItemModel _model, ITEM_TYPE _type)
    {
        int idx = (int)_type;
        itemLvAmountTextArr[idx].text = _model.level.ToString();
        itemLvPriceAmountTextArr[idx].text = (_model.price * _model.level).ToString();
    }

    //private void UpdateHeartItemElememt(ItemModel _model)
    //{
    //    heartItemLvText.text = _model.level.ToString();
    //    heartItemLvUpPriceText.text = (_model.price * _model.level).ToString();
    //}
    //private void UpdateMagnetItemElememt(ItemModel _model)
    //{
    //    magnetItemLvText.text = _model.level.ToString();
    //    magnetItemLvUpPriceText.text = (_model.price * _model.level).ToString();
    //}

}
