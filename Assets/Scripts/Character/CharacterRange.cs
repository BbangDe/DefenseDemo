using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRange : MonoBehaviour
{
    // ���ݹ��� ��������Ʈ
    [SerializeField]
    SpriteRenderer spriteRenderer;
    // ������ ������ ���� ������ Q
    public List<EnemyControl> enemyQ = new List<EnemyControl>();

    // �ݶ��̴� �������� ������ ť�� �߰�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            enemyQ.Add(collision.transform.parent.GetComponent<EnemyControl>());
        }
    }

    // ������ ť���� ����
    private void OnTriggerExit2D(Collider2D collision)
    {
        EnemyControl enemy = collision.transform.parent.GetComponent<EnemyControl>();
        enemyQ.Remove(enemy);
    }

    // ��Ȱ��ȭ�� �� ���� �Ⱥ��̰�
    private void OnDisable()
    {
        ShowRange(false);
    }

    // �޾ƿ� ������ŭ ���ݹ��� ����
    public void SetRange(float range)
    {
        transform.localScale = Vector3.one * range;
    }

    // Q���� ���� ó���� �ִ� ���� Ÿ������ ����
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

    // ���� �̹��� ���̱�/�Ⱥ��̱�
    public void ShowRange(bool _active)
    {
        spriteRenderer.enabled = _active;
    }
}
