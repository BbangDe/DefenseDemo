using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField]
    List<CharacterBase> topBaseList = new List<CharacterBase>();
    [SerializeField]
    List<CharacterBase> bottomBaseList = new List<CharacterBase>();

    HeroGradeWeights heroGradeWeights;

    List<List<CharacterControl>> heroList = new List<List<CharacterControl>>();
    [SerializeField]
    List<CharacterControl> normalHero = new List<CharacterControl>();
    [SerializeField]
    List<CharacterControl> rareHero = new List<CharacterControl>();
    [SerializeField]
    List<CharacterControl> heroHero = new List<CharacterControl>();
    [SerializeField]
    List<CharacterControl> MythicalHero = new List<CharacterControl>();

    List<CharacterControl> characterObjectPool = new List<CharacterControl>();

    [SerializeField]
    GameObject mixBtn;
    CharacterBase selectedBase;

    List<CharacterBase> openedBase = new List<CharacterBase>();

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

    public void AutoUpgrade(int height)
    {
        if(height == 0)
        {
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

    public void SetCharacterBase(CharacterControl newHero, int height)
    {
        if (height == 0)
        {
            bool setOK = false;
            for (int i = 0; i < topBaseList.Count; i++)
            {
                if (!topBaseList[i].IsFull() && topBaseList[i].GetHeroNeme().Equals(newHero.GetHeroName()))
                {
                    topBaseList[i].AddHero2(newHero, transform);
                    setOK = true;
                    break;
                }
            }

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

    public void CharacterRandomSpawn(int height)
    {
        CharacterControl newHero = GetFromCharacterPool(GetRandomHero());

        SetCharacterBase(newHero, height);
    }

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

    public CharacterControl GetHero(int grade, int num)
    {
        return heroList[grade][num];
    }

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

    public void SetMixButton(bool _active)
    {
        if(_active)
        {
            mixBtn.transform.position = Camera.main.WorldToScreenPoint(selectedBase.transform.position + (Vector3.down * 0.3f));
        }

        mixBtn.SetActive(_active);
    }

    public void MixHero(int height)
    {
        CharacterControl character = selectedBase.GetCharacter();
        CharacterControl nextCharacter = GetFromCharacterPool(heroList[character.GetGrade() + 1][character.GetAttackNum()]);

        selectedBase.RemoveHero(height);
        selectedBase.RemoveHero(height);
        selectedBase.RemoveHero(height);

        SetCharacterBase(nextCharacter, height);

        SetMixButton(false);
    }

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
