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
    [SerializeField] private Text coineAmountText;


    protected override void Initialized()
    {
        closeBtn.onClick.AddListener(OnClickCloseBtn);
        ButtonEventBinding();

        UpdateItemElement(ItemManager.getInstance.GetItemModel(ITEM_TYPE.heart), ITEM_TYPE.heart);
        UpdateItemElement(ItemManager.getInstance.GetItemModel(ITEM_TYPE.magnet), ITEM_TYPE.magnet);
        UpdateItemElement(ItemManager.getInstance.GetItemModel(ITEM_TYPE.invincible), ITEM_TYPE.invincible);
    }

    public override void Show()
    {
        base.Show();

        UpdateCoinAmount();
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
        ItemModel model = ItemManager.getInstance.GetItemModel(_type);
        if(ScoreManager.getInstance.DecreaseCoin(model.level * model.price))
        {
            ItemManager.getInstance.ItemLevelUp(_type);
            UpdateItemElement(model, _type);
            UpdateCoinAmount();
        }
        else
        {
            Debug.Log("돈이 없다 ");
        }
    }


    private void UpdateItemElement(ItemModel _model, ITEM_TYPE _type)
    {
        int idx = (int)_type;
        itemLvAmountTextArr[idx].text = _model.level.ToString();
        itemLvPriceAmountTextArr[idx].text = (_model.price * _model.level).ToString();
    }

    private void UpdateCoinAmount()
    {
        coineAmountText.text = ScoreManager.getInstance.GetCoin().ToString();
    }
}
