using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private const float JUMP_POWER = 0.043f;
    private const float GRAVITY = 0.1f;
    private const float DOUBLE_JUMP_POWER = 0.8f;

    private Transform player;
    private bool isJump;
    private bool isDoublejump;
    private bool isGround;
    private float curJumpPower;
    private float curGorundY;
    private float playerInterpolationY;
    public Player(Transform _player)
    {
        player = _player;
        isJump = false;
        isDoublejump = false;
        curGorundY = 0;
    }

    public void MovePlayer()
    {
        if (Input.GetKeyDown("space") && !isJump)
        {
            Jump();
        }
        else if (Input.GetKeyDown("space") && isJump)
        {
            DoubleJump();
        }
        Gravity();

    }

    public void Jump()
    {
        if (!isJump)
        {
            curJumpPower = JUMP_POWER;
            isJump = true;
        }
    }
    public void DoubleJump()
    {
        if (!isDoublejump && isJump)
        {
            curJumpPower = JUMP_POWER * DOUBLE_JUMP_POWER;
            isDoublejump = true;
        }
    }
    public void Gravity()
    {
        if (!isGround || isJump)
        {
            curJumpPower -= Time.deltaTime * GRAVITY;
            Vector2 pos = player.position;
            pos.y += curJumpPower;
            player.position = pos;
            //player.Translate(0, curJumpPower, 0);
            if (player.transform.position.y < curGorundY)
            {
                isJump = false;
                isDoublejump = false;
                player.transform.position = new Vector2(player.transform.position.x, curGorundY);
            }
        }
    }

    //public bool IsJump()
    //{
    //    return isJump;
    //}

    private void SetPlayerPosY(float _posY)
    {
        player.position = new Vector2(player.position.x, _posY);
    }

    public Vector2 GetPlayerPos()
    {
        return player.position;
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
