using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CharacterControl : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    SortingGroup sortingGroup;

    [SerializeField]
    int attackNum;
    [SerializeField]
    string heroName;
    [SerializeField]
    int grade;

    [SerializeField]
    int attack;
    [SerializeField]
    float range;
    [SerializeField]
    CharracterAttack attackObj;

    CharacterBase characterBase;

    EnemyControl target;
    bool attackOK;
    bool nowAttack;

    float count;

    List<CharracterAttack> attackPool = new List<CharracterAttack>();

    public int GetAttackNum()
    {
        return attackNum;
    }

    public string GetHeroName()
    {
        return heroName;
    }

    public int GetGrade()
    {
        return grade;
    }

    public float GetRange()
    {
        return range;
    }

    public void SetCharacterBase(CharacterBase _characterBase)
    {
        characterBase = _characterBase;
    }

    private void Update()
    {
        target = characterBase.GetTarget();
        attackOK = target != null;

        if(attackOK)
        {
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
        else
        {
            if(nowAttack)
            {
                nowAttack = false;
                animator.SetTrigger("Idle");
            }
        }
    }

    public void Attack()
    {
        float _damage = attack;

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
            if (attackNum == 0)
            {
                target.GetDamage(_damage);
            }
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

    public void SetSortingLayer(string layer)
    {
        sortingGroup.sortingLayerName = layer;
    }
}
