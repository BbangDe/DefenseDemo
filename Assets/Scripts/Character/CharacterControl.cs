using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CharacterControl : MonoBehaviour
{
    // 유닛의 애니메이터
    [SerializeField]
    Animator animator;
    // 유닛의 소팅그룹
    [SerializeField]
    SortingGroup sortingGroup;

    // 공격 타입 0:검, 1:마법
    [SerializeField]
    int attackNum;
    // 유닛 이름
    [SerializeField]
    string heroName;
    // 유닛 등급
    [SerializeField]
    int grade;

    // 유닛 공격력
    [SerializeField]
    int attack;
    // 유닛 공격범위
    [SerializeField]
    float range;
    [SerializeField]
    // 유닛 공격 오브젝트
    CharracterAttack attackObj;

    // 유닛이 서있는 칸 오브젝트
    CharacterBase characterBase;

    // 해당 유닛의 공격 타겟
    EnemyControl target;
    // 공격 가능 상태 여부
    bool attackOK;
    // 현재 공격모션중인지 여부
    bool nowAttack;

    float count;

    // 공격 오브젝트를 다룰 오브젝트 풀
    List<CharracterAttack> attackPool = new List<CharracterAttack>();

    // 공격 타입 리턴
    public int GetAttackNum()
    {
        return attackNum;
    }

    // 유닛 이름 리턴
    public string GetHeroName()
    {
        return heroName;
    }

    // 유닛 등급 리턴
    public int GetGrade()
    {
        return grade;
    }

    // 공격 범위 리턴
    public float GetRange()
    {
        return range;
    }

    // 해당 유닛이 서있는 칸 오브젝트 세팅
    public void SetCharacterBase(CharacterBase _characterBase)
    {
        characterBase = _characterBase;
    }

    private void Update()
    {
        // 유닛의 타겟 설정
        target = characterBase.GetTarget();
        // 타겟이 널이 아니면 공격 가능
        attackOK = target != null;

        // 공격가능 하면
        if(attackOK)
        {
            // 공격중이 아니라면 공격하기
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
        // 불가능한 상태면 Idle모션 세팅
        else
        {
            if(nowAttack)
            {
                nowAttack = false;
                animator.SetTrigger("Idle");
            }
        }
    }

    // 공격 함수
    public void Attack()
    {
        // 데미지 설정
        float _damage = attack;

        // 등급에 따른 공격력 추가 계싼
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
            // 검 공격이면 데미지만 입히기
            if (attackNum == 0)
            {
                target.GetDamage(_damage);
            }
            // 마법이면 투사체 생성;
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

    // 오브젝트 풀에서 공격 오브젝트 꺼내기
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

    // 레이어 설정
    public void SetSortingLayer(string layer)
    {
        sortingGroup.sortingLayerName = layer;
    }
}
