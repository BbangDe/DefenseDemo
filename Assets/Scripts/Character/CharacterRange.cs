using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRange : MonoBehaviour
{
    // 공격범위 스프라이트
    [SerializeField]
    SpriteRenderer spriteRenderer;
    // 범위에 들어오는 적들 저장할 Q
    public List<EnemyControl> enemyQ = new List<EnemyControl>();

    // 콜라이더 범위내로 들어오면 큐에 추가
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            enemyQ.Add(collision.transform.parent.GetComponent<EnemyControl>());
        }
    }

    // 나가면 큐에서 삭제
    private void OnTriggerExit2D(Collider2D collision)
    {
        EnemyControl enemy = collision.transform.parent.GetComponent<EnemyControl>();
        enemyQ.Remove(enemy);
    }

    // 비활성화될 시 범위 안보이게
    private void OnDisable()
    {
        ShowRange(false);
    }

    // 받아온 범위만큼 공격범위 지정
    public void SetRange(float range)
    {
        transform.localScale = Vector3.one * range;
    }

    // Q에서 제일 처음에 있는 것을 타겟으로 리턴
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

    // 범위 이미지 보이기/안보이기
    public void ShowRange(bool _active)
    {
        spriteRenderer.enabled = _active;
    }
}
