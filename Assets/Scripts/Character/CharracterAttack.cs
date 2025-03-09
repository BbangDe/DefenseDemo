using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharracterAttack : MonoBehaviour
{
    // 공격 투사체 속도
    [SerializeField]
    float speed;

    // 공격 타겟
    Vector3 target;
    // 공격 데미지
    float damage;

    // 적에 맞으면 데미지 주기
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "EnemyHitBox")
        {
            collision.GetComponent<EnemyControl>().GetDamage(damage);
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // 타겟과의 위치로 방향 계산
        Vector3 dir = target - transform.position;
        dir = dir.normalized;

        // 방향대로 이동
        transform.position += dir * speed * Time.deltaTime;

        float diff = Vector3.Distance(transform.position, target);
        if(diff < 0.05f)
        {
            gameObject.SetActive(false);
        }
    }

    // 받아온 타겟의 위치를 목표로 설정
    public void SetTarget(EnemyControl _target)
    {
        target = _target.transform.position;
    }

    // 받아온 데미지로 해당 투사체 데미지 설정
    public void SetDamage(float _damage)
    {
        damage = _damage;
    }
}
