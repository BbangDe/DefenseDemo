using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRange : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer spriteRenderer;

    public List<EnemyControl> enemyQ = new List<EnemyControl>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            enemyQ.Add(collision.transform.parent.GetComponent<EnemyControl>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EnemyControl enemy = collision.transform.parent.GetComponent<EnemyControl>();
        enemyQ.Remove(enemy);
    }

    private void OnDisable()
    {
        ShowRange(false);
    }

    public void SetRange(float range)
    {
        transform.localScale = Vector3.one * range;
    }

    public EnemyControl GetTarget()
    {
        if(enemyQ.Count == 0)
        {
            return null;
        }
        else
        {
            return enemyQ[0];
        }
    }

    public void ShowRange(bool _active)
    {
        spriteRenderer.enabled = _active;
    }
}
