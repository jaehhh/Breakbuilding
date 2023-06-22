using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    // 조작키
    private KeyCode attackKey = KeyCode.Z;
    private KeyCode guardKey = KeyCode.DownArrow;
    private KeyCode skillKey = KeyCode.X;

    // 상태-공격
    private float attackCooldown = 0.15f; // 공격 속도 제한
    private bool isCanAttack = true;

    // 상태-가드
    private bool isGuarded = false;
    private float guardGauge = 1f;
    private float guardDuration = 0.16f;
    private Coroutine guardCoroutine;
    private WaitForSeconds waitGuardDuration;

    // 참조
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
        waitGuardDuration = new WaitForSeconds(guardDuration);
    }

    private void Update()
    {
        Attack();
        Guard();
        if (guardGauge < 1) guardGauge += 0.1f * Time.deltaTime;
    }

    private void Attack()
    {
        if (!isCanAttack) return;

        if (Input.GetKeyDown(attackKey))
        {
            animator.SetTrigger("attack");
            isCanAttack = false;
            Invoke("AttackCooldown", attackCooldown);
        }
    }

    private void AttackCooldown()
    {
        isCanAttack = true;
    }

    private void Guard()
    {
        if (guardGauge < 0.4f && !isGuarded) return;

        if(Input.GetKeyDown(guardKey))
        {
            guardGauge -= 0.4f;
            isGuarded = true;
            animator.SetBool("isGuard", true);
            guardCoroutine = StartCoroutine(GuardDuration());
        }
    }

    private IEnumerator GuardDuration()
    {
        yield return waitGuardDuration;
        StopGaurd();
    }

    public void StopGaurd()
    {
        if (guardCoroutine != null) StopCoroutine(guardCoroutine);
        isGuarded = true;
        animator.SetBool("isGuard", false);
    }
}
