using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSceneController : MonoBehaviour
{
    [SerializeField] private Button startGameBtn;
    [SerializeField] private Button shopBtn;

    private void Awake()
    {
        startGameBtn.onClick.AddListener(OnClickStartGameBtn);
        shopBtn.onClick.AddListener(OnClickShopBtn);

        //ItemManager.getInstance.Initialize();
    }

    private void OnClickStartGameBtn()
    {
        Debug.Log("start Game");
        SceneManager.LoadScene("InGameScene");
    }
    private void OnClickShopBtn()
    {
        Debug.Log("shop btn Click");
        UIManager.getInstance.Show<ShopCanvasController>("Prefab/Canvas/ShopCanvas");
    }
}
