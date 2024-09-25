using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerController : MonoBehaviour
{
    //基础规则（尝试）
    //滚轮开启跳跃模式持续1s
    //跳跃状态下QE与鼠标同步可以提供向上的瞬时力
    //AD左右移动


    public float speed;
    public float force;
    private Rigidbody2D rb;
    private bool isJump = false;
    private bool isGround = false;
    private float mouseX;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Move();
        JumpState();
        CanGetUpForce();
    }

    void Move()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
    }

    //TODO：加上地面检测不然会脚踩脚
    void JumpState()
    {
        if (Input.mouseScrollDelta.y < 0 && !isJump)
        {
            isJump = true;
            Debug.Log("jump");
            StartCoroutine(JumpTime());
        }
    }

    IEnumerator JumpTime()
    {
        yield return new WaitForSeconds(1f);
        isJump = false;
    }

    void CanGetUpForce()
    {
        if (isJump && SycnQEandMouse())
        {
            Debug.Log("space");
            rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }
    }

    //TODO：手感很差，可以一直Q或E
    bool SycnQEandMouse()
    {
        mouseX = Input.GetAxis("Mouse X");
        Debug.Log(mouseX);
        if (Input.GetKeyDown(KeyCode.E) && mouseX > 0)
        {
            return true;
        }
        if (Input.GetKeyDown(KeyCode.Q) && mouseX < 0)
        {
            return true;
        }
        return false;
    }
}
