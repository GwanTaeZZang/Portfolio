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

    public override void Show()
    {
        base.Show();
        UpdateResult();
    }

    private void OnClickConfirmBtn()
    {
        Hide();
        SceneManager.LoadScene("MainScene");
        UIManager.getInstance.Clear();

    }

    public void UpdateResult()
    {
        scoreAmount.text = ScoreManager.getInstance.GetScore().ToString();
        coinAmount.text = ScoreManager.getInstance.GetResultCoin().ToString();
    }
}
