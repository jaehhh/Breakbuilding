using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpController : MonoBehaviour
{
    // PC 조작키
    private KeyCode jumpKey = KeyCode.UpArrow;

    // 상태
    private bool isGrounded = false;
    private float maxAcceleration = 15f;
    private float currentAcceleration = 0f;
    private Coroutine coroutine; // 점프, 중력 코루틴

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        coroutine = StartCoroutine(GravityFall());
    }

    private void Update()
    {
        Jump();
        
    }

    private void Jump()
    {
        if (!isGrounded) return;

#if UNITY_EDITOR
        if(Input.GetKeyDown(jumpKey))
        {
            isGrounded = false;
            animator.SetBool("isGrounded", false);

            coroutine = StartCoroutine(GravityJump());
        }

#elif UNITY_ANDROID

#endif
    }

    // 점프 조작을 통한 상승 움직임
    private IEnumerator GravityJump()
    {
        while(currentAcceleration > 0)
        {
            transform.position = transform.position + Vector3.up * currentAcceleration * Time.deltaTime;
            currentAcceleration -= 6f * Time.deltaTime; // 가속도 하락

            yield return null;
        }

        currentAcceleration = 0;

        coroutine = StartCoroutine(GravityFall());
    }

    // 중력 하강 움직임
    private IEnumerator GravityFall()
    {
        while (true)
        {
            transform.position = transform.position + Vector3.down * currentAcceleration * Time.deltaTime;
            if (currentAcceleration < maxAcceleration) currentAcceleration += 8f * Time.deltaTime; // 가속도 상승

            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            StopCoroutine(coroutine);
            isGrounded = true;
            currentAcceleration = maxAcceleration; // 최초 가속도 맥스
            animator.SetBool("isGrounded", true);
        }
        else if (collision.transform.tag == "building")
        {
            StopCoroutine(coroutine);
            currentAcceleration = 0; // 최초 가속도0
            coroutine = StartCoroutine(GravityFall());
        }
    }
}
