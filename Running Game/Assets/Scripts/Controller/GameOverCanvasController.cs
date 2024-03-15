using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverCanvasController : UIBaseController
{
    [SerializeField] private Button confirmBtn;
    [SerializeField] private Text scoreAmount;
    [SerializeField] private Text coinAmount;

    protected override void Initialized()
    {
        confirmBtn.onClick.AddListener(OnClickConfirmBtn);
    }

    private void OnClickConfirmBtn()
    {
        Hide();
        SceneManager.LoadScene("MainScene");
        UIManager.getInstance.Clear();

    }

    public void UpdateResult()
    {

    }
}
