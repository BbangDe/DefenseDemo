using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharracterAttack : MonoBehaviour
{
    [SerializeField]
    float speed;

    Vector3 target;
    float damage;

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
        Vector3 dir = target - transform.position;
        dir = dir.normalized;

        transform.position += dir * speed * Time.deltaTime;

        float diff = Vector3.Distance(transform.position, target);
        if(diff < 0.05f)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetTarget(EnemyControl _target)
    {
        target = _target.transform.position;
    }

    public void SetDamage(float _damage)
    {
        damage = _damage;
    }
}
