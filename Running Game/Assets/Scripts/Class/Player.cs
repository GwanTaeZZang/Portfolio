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
    private const int DEAD_POINT = -10;
    private const float COLLISION_DLEAY_TIME = 2f;

    private Transform player;
    private SpriteRenderer playerSpriteRenderer;
    private int maxhp;
    private int hp;

    public delegate void OnCollisionDelegate();
    public OnCollisionDelegate onObstacleCollisionEvent;

    //.. TODO :: isJump / isDoubleJump 하나의 변수로 합쳐서 사용 할 수 있도록  // correction
    private bool isJump;
    private bool isDoublejump;
    private bool isGround;
    private float curJumpPower;
    private float curGorundY;
    private float playerInterpolationY;

    // Item Effect
    private bool isMagnet;
    private int magnetRange;
    private float magnetTime;

    private Animator anim;
    private PlayerState playerState;
    private Vector2 curPos;
    private Rect playerRect;

    private bool hitAble;
    private float DontCollisonTime;

    private Color hitAbleColor;
    private Color hitDisableColor;

    public Player(Transform _player, int _hp)
    {
        player = _player;
        //hp = _hp;
        maxhp = hp = _hp;
        anim = player.GetComponent<Animator>();
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();

        isJump = false;
        isDoublejump = false;
        curGorundY = 0;
        curPos = player.position;
        playerRect = new Rect(curPos.x - 1 * 0.5f, curPos.y + 1 * 0.5f, 1, 1);

        isMagnet = false;
        hitAble = true;
        DontCollisonTime = COLLISION_DLEAY_TIME;

        magnetRange = 1;

        ColorUtility.TryParseHtmlString("#FFFFFF", out hitAbleColor);
        ColorUtility.TryParseHtmlString("#747474", out hitDisableColor);

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



        if (isMagnet)
        {
            magnetTime -= Time.deltaTime;
            if(magnetTime < 0)
            {
                SetMagnetEffect(false, 1, 0);
            }
        }

        if (!hitAble)
        {
            DontCollisonTime -= Time.deltaTime;
            Debug.Log("무적상태 ");
            if(DontCollisonTime < 0)
            {
                DontCollisonTime = COLLISION_DLEAY_TIME;
                hitAble = true;
                playerSpriteRenderer.color = hitAbleColor;
            }
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
        if (isJump && !isDoublejump)
        {
            curJumpPower = JUMP_POWER * DOUBLE_JUMP_POWER;
            isDoublejump = true;
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
                isDoublejump = false;
                curPos.y = curGorundY;
                player.transform.position = curPos;
                curJumpPower = 0;
            }

            if(curPos.y < DEAD_POINT)
            {
                SetHp(-maxhp);
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

    public void SetHp(int _amount)
    {
        if(hp + _amount > maxhp)
        {
            hp = maxhp;
        }
        else if (hp + _amount <= 0)
        {
            hp = 0;
        }
        else
        {
            hp += _amount;
        }

        onObstacleCollisionEvent?.Invoke();
    }

    public int GetHp()
    {
        return hp;
    }

    public void SetMagnetEffect(bool _isMagnet, int _rangeValue, float _time)
    {
        isMagnet = _isMagnet;
        magnetRange = _rangeValue;
        magnetTime = _time;
    }

    public bool IsMagnet()
    {
        return isMagnet;
    }
    
    public int GetMagnetRange()
    {
        return magnetRange;
    }

    public bool IsHit()
    {
        return hitAble;
    }
    public void OffHit()
    {
        hitAble = false;
        playerSpriteRenderer.color = hitDisableColor;
    }
}
