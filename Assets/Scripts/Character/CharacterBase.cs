using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterBase : MonoBehaviour, IPointerDownHandler
{
    bool isTouched;
    bool isFull;
    int numOfHeroes;
    string heroName;
    float range;

    List<CharacterControl> heroes = new List<CharacterControl>();

    [SerializeField]
    List<Vector3> positionPivot;

    [SerializeField]
    CharacterRange attackRange;

    [SerializeField]
    SpriteRenderer spriteRenderer;

    CharacterManager characterManager;

    public void OnPointerDown(PointerEventData eventData)
    {
        if(numOfHeroes > 0)
        {
            if(isTouched)
            {
                isTouched = false;
                SetSortingLayer("Player");
                ShowRange(false);
                characterManager.SetMixButton(false);
            }
            else
            {
                isTouched = true;
                SetSortingLayer("Selected");
                
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

    public int GetGrade()
    {
        return heroes[0].GetGrade();
    }

    public void AddHero(CharacterControl newHero, Transform parent)
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

        GameManager.instance.SetNowNumOfHero(1);
    }

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

    public CharacterControl GetCharacter()
    {
        if(numOfHeroes > 0)
        {
            return heroes[0];
        }

        return null;
    }

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

    public bool IsFull()
    {
        return isFull;
    }

    public int GetNumOfHeroes()
    {
        return numOfHeroes;
    }

    public string GetHeroNeme()
    {
        return heroName;
    }

    public EnemyControl GetTarget()
    {
        return attackRange.GetTarget();
    }

    public void ShowRange(bool _active)
    {
        attackRange.ShowRange(_active);
    }

    public void SetSortingLayer(string _layer)
    {
        for (int i = 0; i < heroes.Count; i++)
        {
            heroes[i].SetSortingLayer(_layer);
        }
    }
}
