using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Dino_State
{
    idle,
    jump,
    run,
    hit
}

public class Dino : MonoBehaviour
{
    public float startJumpPower;
    public float jumpPower;
    public bool isGround;

    private Rigidbody2D rigid;
    private Animator anim;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isGround = true;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            rigid.AddForce(Vector2.up * startJumpPower, ForceMode2D.Impulse);
        }
        else if (Input.GetButton("Jump"))
        {
            jumpPower = Mathf.Lerp(jumpPower, 0, 0.1f);  // 보간 
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isGround)
        {
            ChangeAnim(Dino_State.run);
            isGround = true;
            jumpPower = 1;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isGround)
        {
            ChangeAnim(Dino_State.jump);
            isGround = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        rigid.simulated = false;
        ChangeAnim(Dino_State.hit);
    }

    private void ChangeAnim(Dino_State _state)
    {
        anim.SetInteger("State", (int)_state);
    }
}
