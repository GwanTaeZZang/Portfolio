using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Walk = 0,
    Jump = 1,
    Land = 2,
}

public class Player
{
    private const float JUMP_POWER = 0.014f;
    private const float GRAVITY = 0.025f;
    private const float DOUBLE_JUMP_POWER = 0.9f;

    private Transform player;

    //.. TODO :: isJump / isDoubleJump 하나의 변수로 합쳐서 사용 할 수 있도록  // correction
    private bool isJump;
    //private bool isDoublejump;
    private bool isGround;
    private float curJumpPower;
    private float curGorundY;
    private float playerInterpolationY;

    private Animator anim;
    private PlayerState playerState;
    private Vector2 curPos;
    private Rect playerRect;

    public Player(Transform _player)
    {
        player = _player;
        anim = player.GetComponent<Animator>();

        isJump = false;
        //isDoublejump = false;
        curGorundY = 0;
        curPos = player.position;
        playerRect = new Rect(curPos.x - 1 * 0.5f, curPos.y + 1 * 0.5f, 1, 1);
    }

    public void UpdatePlayer()
    {
        if (Input.GetKeyDown("space") && !isJump)
        {
            Jump();

        }
        else if (Input.GetKeyDown("space") && isJump)
        {
            DoubleJump();

        }
    }


    public void Jump()
    {
        if (!isJump)
        {
            curJumpPower = JUMP_POWER;
            isJump = true;
            ChangePlayerAnimation(PlayerState.Jump);
        }
    }

    public void DoubleJump()
    {
        if (isJump)
        {
            curJumpPower = JUMP_POWER * DOUBLE_JUMP_POWER;
            //isDoublejump = true;
            ChangePlayerAnimation(PlayerState.Jump);
        }
    }

    public void Gravity()
    {
        if (!isGround || isJump)
        {
            curJumpPower -= Time.deltaTime * GRAVITY;
            //Vector2 pos = player.position;
            curPos.y += curJumpPower;
            player.position = curPos;

            if(curJumpPower < 0 && isJump && playerState == PlayerState.Jump)
            {
                ChangePlayerAnimation(PlayerState.Land);
            }

            if (curPos.y <= curGorundY)
            {
                ChangePlayerAnimation(PlayerState.Walk);
                isJump = false;
                //isDoublejump = false;
                curPos.y = curGorundY;
                player.transform.position = curPos;
                curJumpPower = 0;
            }
        }
    }

    private void SetPlayerPosY(float _posY)
    {
        curPos.y = _posY;
        player.position = curPos;
    }

    //.. TODO :: Animation 상태 변환은 Event 형식으로 처리 할 수 있게 변경 (한번만 들어올 수 있게) // correction
    private void ChangePlayerAnimation(PlayerState _state)
    {
        //if (playerState == _state)
        //{
        //    return;
        //}
        playerState = _state;
        anim.SetInteger("State", (int)_state);
    }

    public Vector2 GetPlayerPos()
    {
        return curPos;
    }

    public Rect GetRect()
    {
        playerRect.Set(curPos.x - 1 * 0.5f, curPos.y + 1 * 0.5f, 1, 1);
        return playerRect;
    }

    public void SetGroundPosY(float _groundY)
    {
        curGorundY = _groundY;
    }

    public void SetIsGround(bool _isGround)
    {
        isGround = _isGround;
    }

    public void PlayerPosYInterpolation(float _yPos)
    {
        if(playerInterpolationY != _yPos)
        {
            playerInterpolationY = _yPos;
            SetPlayerPosY(playerInterpolationY);
        }
    }

    
}
