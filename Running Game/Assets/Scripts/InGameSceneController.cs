using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameSceneController : MonoBehaviour
{
    [SerializeField] private Transform startFloorObj;
    [SerializeField] private Transform playerObj;
    [SerializeField] private Transform passFloorStandard;
    [SerializeField] private Transform RepositionFloorStandard;

    private Floor startFloor;
    private Player player;

    private void Awake()
    {
        startFloor = new Floor(startFloorObj, RepositionFloorStandard);
        player = new Player(playerObj);
    }

    private void Update()
    {
        startFloor.MoveFloor();
    }
}
