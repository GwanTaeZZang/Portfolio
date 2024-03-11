using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameSceneController : MonoBehaviour
{

    [SerializeField] private Transform playerObj;
    [SerializeField] private Transform floorParent;
    [SerializeField] private Transform obstacleParent;
    [SerializeField] private Transform coinParent;
    [SerializeField] private Transform itemParent;
    // View
    [SerializeField] private Text scoreAmount;
    [SerializeField] private Image[] heartArr;

    private FloorController floorCtrl;
    private ObstacleController obstacleCtrl;
    private CoinController coinCtrl;
    private ItemController itemCtrl;

    private Player player;

    private float repositionX;
    private float inScenePosX;

    private int curHeartCount;
    private Color offHeartColor;
    private Color onHeartColor;

    
    

    private Camera myCamera;
    private void Awake()
    {

        //.. TODO :: Camera.main 캐싱 습관화  // correction
        myCamera = Camera.main;
        repositionX = myCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        inScenePosX = myCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;

        curHeartCount = heartArr.Length;
        player = new Player(playerObj, curHeartCount);

        coinCtrl = new CoinController(coinParent, player, repositionX, inScenePosX);

        obstacleCtrl = new ObstacleController(obstacleParent, player, repositionX, inScenePosX, SetObstacleEvnent);

        itemCtrl = new ItemController(itemParent, player, repositionX, inScenePosX);

        floorCtrl = new FloorController(floorParent, player, repositionX, SetFloorEvent);

        coinCtrl.onScoreEvnet = OnUpdateScoreText;

        player.onObstacleCollisionEvent = OnUpdateHeartUI;


        ColorUtility.TryParseHtmlString("#FFFFFF", out onHeartColor);
        ColorUtility.TryParseHtmlString("#747474", out offHeartColor);
    }

    private void Update()
    {
        player.UpdatePlayer();
    }

    private void FixedUpdate()
    {
        //.. TODO :: Player RenderUpdate는 Update고 나머지는 왜 Fixed? 일치화 시켜야함  // correction
        floorCtrl.FixedUpdateFloor();
        obstacleCtrl.FixedObstacleUpdate();
        coinCtrl.FixedUpdateCoin();
        itemCtrl.FixedItemUpdate();
        //..

        player.Gravity();
    }

    private void SetFloorEvent(Floor _floor)
    {
        obstacleCtrl.SetRandomObstaclePos(_floor);
        itemCtrl.SetRandomItemPos(_floor);
    }

    private void SetObstacleEvnent(Floor _floor, Obstacle _obstacle)
    {
        coinCtrl.SetCoinPosition(_floor, _obstacle);
    }

    private void OnUpdateScoreText(int _score)
    {
        scoreAmount.text = _score.ToString();
    }
    private void OnUpdateHeartUI()
    {
        // 줄일 수 있을거 같음 나중에 다시확인

        int playerHp = player.GetHp();

        if(playerHp == 0)
        {
            return;
        }

        if(curHeartCount > playerHp)
        {
            // 감소
            heartArr[curHeartCount - 1].color = offHeartColor;
            curHeartCount--;
            Debug.Log("플레이어 피 : " + playerHp);
        }
        else if(playerHp > heartArr.Length)
        {
            player.SetHp(curHeartCount);
        }
        else
        {
            // 증가
            curHeartCount++;
            heartArr[curHeartCount - 1].color = onHeartColor;
        }

    }
}
