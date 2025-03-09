using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 영웅 등급별 등장확률
[Serializable]
struct HeroGrade
{
    public float weight;
    public int grade;
}

[Serializable]
struct HeroGradeWeights
{
    public HeroGrade[] heroGradeList;
}

public class CharacterManager : MonoBehaviour
{
    // 위, 아래 유닛이 서있는 칸 오브젝트 리스트
    [SerializeField]
    List<CharacterBase> topBaseList = new List<CharacterBase>();
    [SerializeField]
    List<CharacterBase> bottomBaseList = new List<CharacterBase>();

    // 유닛 등급 확률 리스트
    HeroGradeWeights heroGradeWeights;

    // 등장 가능한 유닛 리스트
    List<List<CharacterControl>> heroList = new List<List<CharacterControl>>();
    // 등급별 유닛 리스트
    [SerializeField]
    List<CharacterControl> normalHero = new List<CharacterControl>();
    [SerializeField]
    List<CharacterControl> rareHero = new List<CharacterControl>();
    [SerializeField]
    List<CharacterControl> heroHero = new List<CharacterControl>();
    [SerializeField]
    List<CharacterControl> MythicalHero = new List<CharacterControl>();

    // 유닛을 가져올 오브젝트 풀
    List<CharacterControl> characterObjectPool = new List<CharacterControl>();

    // 조합버튼
    [SerializeField]
    GameObject mixBtn;
    CharacterBase selectedBase;

    List<CharacterBase> openedBase = new List<CharacterBase>();

    // 확률 데이터 읽어서 저장
    public void Init()
    {
        for(int i=0; i<topBaseList.Count; i++)
        {
            topBaseList[i].Init();
            bottomBaseList[i].Init();
        }

        TextAsset heroGradeTxt = Resources.Load<TextAsset>("Json/heroGrade");
        heroGradeWeights = JsonUtility.FromJson<HeroGradeWeights>(heroGradeTxt.text);

        heroList.Add(normalHero);
        heroList.Add(rareHero);
        heroList.Add(heroHero);
        heroList.Add(MythicalHero);
    }

    // AI가 사용할 자동 조합 함수
    public void AutoUpgrade(int height)
    {
        // 높이에 따라 맞는 리스트 선택
        if(height == 0)
        {
            // 가득찬 칸이면서 조합 가능하면 조합함수 실행
            for(int i=0; i<topBaseList.Count; i++)
            {
                if (topBaseList[i].IsFull() && topBaseList[i].GetGrade() < 2)
                {
                    selectedBase = topBaseList[i];
                    MixHero(0);
                }
            }
        }
        else
        {
            for (int i = 0; i < bottomBaseList.Count; i++)
            {
                if (bottomBaseList[i].IsFull() && bottomBaseList[i].GetGrade() < 2)
                {
                    MixHero(1);
                }
            }
        }
    }

    // 이름으로 해당하는 유닛 찾아서 삭제
    public void RemoveCharacter(string _name, int height)
    {
        if(height == 0)
        {
            for (int i = 0; i < topBaseList.Count; i++)
            {
                if (topBaseList[i].GetNumOfHeroes() > 0 && topBaseList[i].GetHeroNeme().Equals(_name))
                {
                    topBaseList[i].RemoveHero(height);
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < bottomBaseList.Count; i++)
            {
                if (bottomBaseList[i].GetNumOfHeroes() > 0 && bottomBaseList[i].GetHeroNeme().Equals(_name))
                {
                    bottomBaseList[i].RemoveHero(height);
                    break;
                }
            }
        }
    }

    // 유닛이 있는지 없는지 판단하는 함수
    public bool SearchCharacter(string _name, int height)
    {
        if(height == 0)
        {
            bool findOK = false;
            for(int i=0; i<topBaseList.Count; i++)
            {
                if (topBaseList[i].GetNumOfHeroes() > 0 && topBaseList[i].GetHeroNeme().Equals(_name))
                {
                    findOK = true;
                    break;
                }
            }

            return findOK;
        }
        else
        {
            bool findOK = false;
            for (int i = 0; i < bottomBaseList.Count; i++)
            {
                if (bottomBaseList[i].GetNumOfHeroes() > 0 && bottomBaseList[i].GetHeroNeme().Equals(_name))
                {
                    findOK = true;
                    break;
                }
            }

            return findOK;
        }
    }

    // 유닛을 새로운 칸에 할당
    public void SetCharacterBase(CharacterControl newHero, int height)
    {
        if (height == 0)
        {
            bool setOK = false;
            for (int i = 0; i < topBaseList.Count; i++)
            {
                // 가득차 있지 않고 같은 이름이라면 해당 칸에 추가
                if (!topBaseList[i].IsFull() && topBaseList[i].GetHeroNeme().Equals(newHero.GetHeroName()))
                {
                    topBaseList[i].AddHero2(newHero, transform);
                    setOK = true;
                    break;
                }
            }

            // 그것도 아니라면 빈칸에 추가
            if(!setOK)
            {
                for (int i = 0; i < topBaseList.Count; i++)
                {
                    if (topBaseList[i].GetNumOfHeroes() == 0)
                    {
                        topBaseList[i].AddHero2(newHero, transform);
                        break;
                    }
                }
            }
        }
        // 아래도 같은 원리
        else
        {
            bool setOK = false;
            for (int i = 0; i < bottomBaseList.Count; i++)
            {
                if (bottomBaseList[i].GetNumOfHeroes() > 0)
                {
                    if (!bottomBaseList[i].IsFull() && bottomBaseList[i].GetHeroNeme().Equals(newHero.GetHeroName()))
                    {
                        bottomBaseList[i].AddHero(newHero, transform);
                        setOK = true;
                        break;
                    }
                }
            }

            if(!setOK)
            {
                for (int i = 0; i < bottomBaseList.Count; i++)
                {
                    if (bottomBaseList[i].GetNumOfHeroes() == 0)
                    {
                        bottomBaseList[i].AddHero(newHero, transform);
                        break;
                    }
                }
            }
        }
    }

    // 랜덤한 유닛을 풀에서 꺼내서 추가
    public void CharacterRandomSpawn(int height)
    {
        CharacterControl newHero = GetFromCharacterPool(GetRandomHero());

        SetCharacterBase(newHero, height);
    }

    // 풀에서 유닛 꺼내오는 함수
    public CharacterControl GetFromCharacterPool(CharacterControl hero)
    {
        string heroName = hero.GetHeroName();
        for (int i = 0; i < characterObjectPool.Count; i++)
        {
            if (!characterObjectPool[i].gameObject.activeSelf && characterObjectPool[i].GetHeroName().Equals(heroName))
            {
                return characterObjectPool[i];
            }
        }

        CharacterControl newHero = Instantiate(hero);
        characterObjectPool.Add(newHero);
        return newHero;
    }

    // 등급, 번호로 해당하는 유닛 리턴
    public CharacterControl GetHero(int grade, int num)
    {
        return heroList[grade][num];
    }

    // 이름이 동일한 유닛 찾아서 리턴
    public CharacterControl GetHero(string _name)
    {
        for(int i=0; i<heroList.Count; i++)
        {
            if (heroList[i][0].GetHeroName().Equals(_name))
            {
                return heroList[i][0];
            }
            else if (heroList[i][1].GetHeroName().Equals(_name))
            {
                return heroList[i][1];
            }
        }

        return null;
    }

    // 랜덤한 유닛 가져오기(유닛 등급별 확률에 따름)
    CharacterControl GetRandomHero()
    {
        float totalWeight = 0f;
        foreach(var heroGrade in heroGradeWeights.heroGradeList)
        {
            totalWeight += heroGrade.weight;
        }

        float randomValue = UnityEngine.Random.value * totalWeight;

        int pick = -1;
        foreach (var heroGrade in heroGradeWeights.heroGradeList)
        {
            randomValue -= heroGrade.weight;
            if(randomValue <= 0)
            {
                pick = heroGrade.grade;
                break;
            }
        }

        if(pick == -1)
        {
            pick = heroGradeWeights.heroGradeList[heroGradeWeights.heroGradeList.Length - 1].grade;
        }

        return GetHero(pick, UnityEngine.Random.Range(0, 2));
    }

    // 현재 선택된 칸 외의 칸은 비활성
    public void SetCharacterBase(CharacterBase _base)
    {
        for(int i=0; i<openedBase.Count; i++)
        {
            openedBase[i].ShowRange(false);
            openedBase[i].SetSortingLayer("Player");
        }
        openedBase.Clear();

        selectedBase = _base;
        openedBase.Add(selectedBase);
    }

    // 조합 가능할 경우 조합버튼 활성화
    public void SetMixButton(bool _active)
    {
        if(_active)
        {
            mixBtn.transform.position = Camera.main.WorldToScreenPoint(selectedBase.transform.position + (Vector3.down * 0.3f));
        }

        mixBtn.SetActive(_active);
    }

    // 조합함수
    public void MixHero(int height)
    {
        // 조합될 새 유닛 풀에서 가져오고
        CharacterControl character = selectedBase.GetCharacter();
        CharacterControl nextCharacter = GetFromCharacterPool(heroList[character.GetGrade() + 1][character.GetAttackNum()]);

        // 재료들 지우기
        selectedBase.RemoveHero(height);
        selectedBase.RemoveHero(height);
        selectedBase.RemoveHero(height);

        SetCharacterBase(nextCharacter, height);

        SetMixButton(false);
    }

    // 업그레이드에 따른 확률 변동
    public void UpgradeWeight()
    {
        for (int i = 0; i < heroGradeWeights.heroGradeList.Length; i++)
        {
            if (heroGradeWeights.heroGradeList[i].grade == 0)
            {
                heroGradeWeights.heroGradeList[i].weight -= 4;
            }
            else if (heroGradeWeights.heroGradeList[i].grade == 1)
            {
                heroGradeWeights.heroGradeList[i].weight += 2.7f;
            }
            else if (heroGradeWeights.heroGradeList[i].grade == 2)
            {
                heroGradeWeights.heroGradeList[i].weight += 1.3f;
            }
        }
    }
}
