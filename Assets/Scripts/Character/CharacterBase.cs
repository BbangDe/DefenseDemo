using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterBase : MonoBehaviour, IPointerDownHandler
{
    // 현재 칸이 터치되었는지
    bool isTouched;
    // 현재 칸에 3명이 꽉찼는지
    bool isFull;
    // 현재 칸에 몇명이 있는지
    int numOfHeroes;
    // 현재 칸에 있는 유닛 이름은?
    string heroName;
    // 현재 칸 유닛의 사거리
    float range;

    // 존재하는 영웅리스트
    List<CharacterControl> heroes = new List<CharacterControl>();

    // 첫 등장 시 위치 조절 필요한 값
    [SerializeField]
    List<Vector3> positionPivot;

    // 공격 범위 오브젝트
    [SerializeField]
    CharacterRange attackRange;

    // 현재 칸 오브젝트 스프라이트
    [SerializeField]
    SpriteRenderer spriteRenderer;

    // characterManager 연동 변수
    CharacterManager characterManager;

    // 현재 칸을 선택할 경우
    public void OnPointerDown(PointerEventData eventData)
    {
        // 해당칸이 빈 칸이 아니라면
        if(numOfHeroes > 0)
        {
            if(isTouched)
            {
                // 이미 터치됐던 칸이면 기존 선택 취소, 조합버튼 비활성화
                isTouched = false;
                SetSortingLayer("Player");
                ShowRange(false);
                characterManager.SetMixButton(false);
            }
            else
            {
                // 아니라면 터치중 표시해두고
                isTouched = true;
                // 레이어 조절 후
                SetSortingLayer("Selected");
                
                // 해당 칸의 범위 표시해주고, 조합버튼 활성화
                characterManager.SetCharacterBase(this);
                ShowRange(true);
                if (isFull && heroes[0].GetGrade() < 2)
                {
                    characterManager.SetMixButton(true);
                }
            }
        }
    }

    public void Init()
    {
        isTouched = false;
        isFull = false;
        numOfHeroes = 0;
        heroName = "";
        range = 0;
        heroes.Clear();

        attackRange.gameObject.SetActive(false);

        if(characterManager == null)
        {
            characterManager = transform.parent.parent.GetComponent<CharacterManager>();
        }
    }

    // 현재 칸의 유닛 등급 리턴
    public int GetGrade()
    {
        return heroes[0].GetGrade();
    }

    // 현재 칸에 유닛 추가
    public void AddHero(CharacterControl newHero, Transform parent)
    {
        // 유닛 목록에 추가
        heroes.Add(newHero);
        // 유닛 수 증가
        numOfHeroes++;
        // 가득찼는지 여부 체크
        isFull = numOfHeroes == 3;
        // 해당 칸의 영웅 이름 가져오기
        heroName = newHero.GetHeroName();
        // 해당 칸의 사거리 가져오기
        range = newHero.GetRange();
        // 범위 표시 오브젝트에 적용
        attackRange.SetRange(range);
        attackRange.gameObject.SetActive(true);

        // 새 유닛 활성화
        newHero.SetCharacterBase(this);
        newHero.transform.position = transform.position + positionPivot[numOfHeroes - 1];
        newHero.transform.SetParent(parent);
        newHero.gameObject.SetActive(true);

        GameManager.instance.SetNowNumOfHero(1);
    }

    // AI의 유닛추가에 사용할 함수(구조는 동일)
    public void AddHero2(CharacterControl newHero, Transform parent)
    {
        heroes.Add(newHero);
        numOfHeroes++;
        isFull = numOfHeroes == 3;
        heroName = newHero.GetHeroName();
        range = newHero.GetRange();
        attackRange.SetRange(range);
        attackRange.gameObject.SetActive(true);

        newHero.SetCharacterBase(this);
        newHero.transform.position = transform.position + positionPivot[numOfHeroes - 1];
        newHero.transform.SetParent(parent);
        newHero.gameObject.SetActive(true);
    }

    // 해당 칸에 있는 유닛 리턴
    public CharacterControl GetCharacter()
    {
        if(numOfHeroes > 0)
        {
            return heroes[0];
        }

        return null;
    }

    // 칸에서 적유닛 삭제
    public void RemoveHero(int height)
    {
        numOfHeroes--;
        isFull = false;

        heroes[numOfHeroes].gameObject.SetActive(false);
        heroes.RemoveAt(numOfHeroes);
        if(height == 1)
            GameManager.instance.SetNowNumOfHero(-1);

        if (numOfHeroes == 0)
        {
            Init();
        }
    }

    // 해당 칸이 가득 찼는지 표시
    public bool IsFull()
    {
        return isFull;
    }

    // 해당 칸의 영웅 수 리턴
    public int GetNumOfHeroes()
    {
        return numOfHeroes;
    }

    // 해당 칸의 영웅 이름 리턴
    public string GetHeroNeme()
    {
        return heroName;
    }

    // 공격 범위에 들어온 적들 중 타겟을 가져옴
    public EnemyControl GetTarget()
    {
        return attackRange.GetTarget();
    }

    // 공격범위 보여주기
    public void ShowRange(bool _active)
    {
        attackRange.ShowRange(_active);
    }

    // sortingLayer 설정
    public void SetSortingLayer(string _layer)
    {
        for (int i = 0; i < heroes.Count; i++)
        {
            heroes[i].SetSortingLayer(_layer);
        }
    }
}
