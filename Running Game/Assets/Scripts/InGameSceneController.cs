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
    [SerializeField] private Text scoreAmount;

    private FloorController floorCtrl;
    private ObstacleController obstacleCtrl;
    private CoinController coinCtrl;

    private Player player;

    private float repositionX;
    private float inScenePosX;

    private void Awake()
    {
        repositionX = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        inScenePosX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        player = new Player(playerObj);

        coinCtrl = new CoinController(coinParent, player, repositionX, inScenePosX);

        obstacleCtrl = new ObstacleController(obstacleParent, player, repositionX, inScenePosX, SetCoinEvnent);

        floorCtrl = new FloorController(floorParent, player, repositionX, SetObstacleEvent);

        coinCtrl.scoreEvnet = UpdateScore;
    }

    private void Update()
    {
        player.MovePlayer();
    }

    private void FixedUpdate()
    {
        floorCtrl.UpdateFloor();
        obstacleCtrl.ObstacleUpdate();
        coinCtrl.UpdateCoin();
        player.Gravity();
    }

    private void SetObstacleEvent(Floor _floor)
    {
        obstacleCtrl.SetRandomObstaclePos(_floor);
    }

    private void SetCoinEvnent(Floor _floor, Obstacle _obstacle)
    {
        coinCtrl.SetCoinPosition(_floor, _obstacle);
    }

    private void UpdateScore(int _score)
    {
        scoreAmount.text = _score.ToString();
    }    
}
