using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CharacterControl : MonoBehaviour
{
    // ������ �ִϸ�����
    [SerializeField]
    Animator animator;
    // ������ ���ñ׷�
    [SerializeField]
    SortingGroup sortingGroup;

    // ���� Ÿ�� 0:��, 1:����
    [SerializeField]
    int attackNum;
    // ���� �̸�
    [SerializeField]
    string heroName;
    // ���� ���
    [SerializeField]
    int grade;

    // ���� ���ݷ�
    [SerializeField]
    int attack;
    // ���� ���ݹ���
    [SerializeField]
    float range;
    [SerializeField]
    // ���� ���� ������Ʈ
    CharracterAttack attackObj;

    // ������ ���ִ� ĭ ������Ʈ
    CharacterBase characterBase;

    // �ش� ������ ���� Ÿ��
    EnemyControl target;
    // ���� ���� ���� ����
    bool attackOK;
    // ���� ���ݸ�������� ����
    bool nowAttack;

    float count;

    // ���� ������Ʈ�� �ٷ� ������Ʈ Ǯ
    List<CharracterAttack> attackPool = new List<CharracterAttack>();

    // ���� Ÿ�� ����
    public int GetAttackNum()
    {
        return attackNum;
    }

    // ���� �̸� ����
    public string GetHeroName()
    {
        return heroName;
    }

    // ���� ��� ����
    public int GetGrade()
    {
        return grade;
    }

    // ���� ���� ����
    public float GetRange()
    {
        return range;
    }

    // �ش� ������ ���ִ� ĭ ������Ʈ ����
    public void SetCharacterBase(CharacterBase _characterBase)
    {
        characterBase = _characterBase;
    }

    private void Update()
    {
        // ������ Ÿ�� ����
        target = characterBase.GetTarget();
        // Ÿ���� ���� �ƴϸ� ���� ����
        attackOK = target != null;

        // ���ݰ��� �ϸ�
        if(attackOK)
        {
            // �������� �ƴ϶�� �����ϱ�
            if(!nowAttack)
            {
                nowAttack = true;
                animator.SetTrigger($"Attack{attackNum}");
            }
            else
            {
                nowAttack = false;
                animator.SetTrigger("Idle");
            }
        }
        // �Ұ����� ���¸� Idle��� ����
        else
        {
            if(nowAttack)
            {
                nowAttack = false;
                animator.SetTrigger("Idle");
            }
        }
    }

    // ���� �Լ�
    public void Attack()
    {
        // ������ ����
        float _damage = attack;

        // ��޿� ���� ���ݷ� �߰� ���
        if(grade == 0)
        {
            _damage += GameManager.instance.GetUpgradeLevel(0) * 7;
        }
        else if (grade == 1)
        {
            _damage += GameManager.instance.GetUpgradeLevel(1) * 9;
        }
        else if (grade == 2)
        {
            _damage += GameManager.instance.GetUpgradeLevel(0) * 50;
        }
        else if (grade == 3)
        {
            _damage += GameManager.instance.GetUpgradeLevel(0) * 100;
        }

        if (target != null)
        {
            // �� �����̸� �������� ������
            if (attackNum == 0)
            {
                target.GetDamage(_damage);
            }
            // �����̸� ����ü ����;
            else
            {
                CharracterAttack newAttack = GetFromAttackPool(attackObj);
                newAttack.transform.position = transform.position;
                newAttack.SetTarget(target);
                newAttack.SetDamage(_damage);
                newAttack.gameObject.SetActive(true);
            }
        }
    }

    // ������Ʈ Ǯ���� ���� ������Ʈ ������
    CharracterAttack GetFromAttackPool(CharracterAttack atk)
    {
        for (int i = 0; i < attackPool.Count; i++)
        {
            if (!attackPool[i].gameObject.activeSelf)
            {
                return attackPool[i];
            }
        }

        CharracterAttack newAttack = Instantiate(atk);
        attackPool.Add(newAttack);
        return newAttack;
    }

    // ���̾� ����
    public void SetSortingLayer(string layer)
    {
        sortingGroup.sortingLayerName = layer;
    }
}
