using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpController : MonoBehaviour
{
    // PC ����Ű
    private KeyCode jumpKey = KeyCode.UpArrow;

    // ����
    private bool isGrounded = false;
    private float maxAcceleration = 15f;
    private float currentAcceleration = 0f;
    private Coroutine coroutine; // ����, �߷� �ڷ�ƾ

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

    // ���� ������ ���� ��� ������
    private IEnumerator GravityJump()
    {
        while(currentAcceleration > 0)
        {
            transform.position = transform.position + Vector3.up * currentAcceleration * Time.deltaTime;
            currentAcceleration -= 6f * Time.deltaTime; // ���ӵ� �϶�

            yield return null;
        }

        currentAcceleration = 0;

        coroutine = StartCoroutine(GravityFall());
    }

    // �߷� �ϰ� ������
    private IEnumerator GravityFall()
    {
        while (true)
        {
            transform.position = transform.position + Vector3.down * currentAcceleration * Time.deltaTime;
            if (currentAcceleration < maxAcceleration) currentAcceleration += 8f * Time.deltaTime; // ���ӵ� ���

            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            StopCoroutine(coroutine);
            isGrounded = true;
            currentAcceleration = maxAcceleration; // ���� ���ӵ� �ƽ�
            animator.SetBool("isGrounded", true);
        }
        else if (collision.transform.tag == "building")
        {
            StopCoroutine(coroutine);
            currentAcceleration = 0; // ���� ���ӵ�0
            coroutine = StartCoroutine(GravityFall());
        }
    }
}
