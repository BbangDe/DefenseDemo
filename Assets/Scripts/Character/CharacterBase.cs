using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterBase : MonoBehaviour, IPointerDownHandler
{
    // ���� ĭ�� ��ġ�Ǿ�����
    bool isTouched;
    // ���� ĭ�� 3���� ��á����
    bool isFull;
    // ���� ĭ�� ����� �ִ���
    int numOfHeroes;
    // ���� ĭ�� �ִ� ���� �̸���?
    string heroName;
    // ���� ĭ ������ ��Ÿ�
    float range;

    // �����ϴ� ��������Ʈ
    List<CharacterControl> heroes = new List<CharacterControl>();

    // ù ���� �� ��ġ ���� �ʿ��� ��
    [SerializeField]
    List<Vector3> positionPivot;

    // ���� ���� ������Ʈ
    [SerializeField]
    CharacterRange attackRange;

    // ���� ĭ ������Ʈ ��������Ʈ
    [SerializeField]
    SpriteRenderer spriteRenderer;

    // characterManager ���� ����
    CharacterManager characterManager;

    // ���� ĭ�� ������ ���
    public void OnPointerDown(PointerEventData eventData)
    {
        // �ش�ĭ�� �� ĭ�� �ƴ϶��
        if(numOfHeroes > 0)
        {
            if(isTouched)
            {
                // �̹� ��ġ�ƴ� ĭ�̸� ���� ���� ���, ���չ�ư ��Ȱ��ȭ
                isTouched = false;
                SetSortingLayer("Player");
                ShowRange(false);
                characterManager.SetMixButton(false);
            }
            else
            {
                // �ƴ϶�� ��ġ�� ǥ���صΰ�
                isTouched = true;
                // ���̾� ���� ��
                SetSortingLayer("Selected");
                
                // �ش� ĭ�� ���� ǥ�����ְ�, ���չ�ư Ȱ��ȭ
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

    // ���� ĭ�� ���� ��� ����
    public int GetGrade()
    {
        return heroes[0].GetGrade();
    }

    // ���� ĭ�� ���� �߰�
    public void AddHero(CharacterControl newHero, Transform parent)
    {
        // ���� ��Ͽ� �߰�
        heroes.Add(newHero);
        // ���� �� ����
        numOfHeroes++;
        // ����á���� ���� üũ
        isFull = numOfHeroes == 3;
        // �ش� ĭ�� ���� �̸� ��������
        heroName = newHero.GetHeroName();
        // �ش� ĭ�� ��Ÿ� ��������
        range = newHero.GetRange();
        // ���� ǥ�� ������Ʈ�� ����
        attackRange.SetRange(range);
        attackRange.gameObject.SetActive(true);

        // �� ���� Ȱ��ȭ
        newHero.SetCharacterBase(this);
        newHero.transform.position = transform.position + positionPivot[numOfHeroes - 1];
        newHero.transform.SetParent(parent);
        newHero.gameObject.SetActive(true);

        GameManager.instance.SetNowNumOfHero(1);
    }

    // AI�� �����߰��� ����� �Լ�(������ ����)
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

    // �ش� ĭ�� �ִ� ���� ����
    public CharacterControl GetCharacter()
    {
        if(numOfHeroes > 0)
        {
            return heroes[0];
        }

        return null;
    }

    // ĭ���� ������ ����
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

    // �ش� ĭ�� ���� á���� ǥ��
    public bool IsFull()
    {
        return isFull;
    }

    // �ش� ĭ�� ���� �� ����
    public int GetNumOfHeroes()
    {
        return numOfHeroes;
    }

    // �ش� ĭ�� ���� �̸� ����
    public string GetHeroNeme()
    {
        return heroName;
    }

    // ���� ������ ���� ���� �� Ÿ���� ������
    public EnemyControl GetTarget()
    {
        return attackRange.GetTarget();
    }

    // ���ݹ��� �����ֱ�
    public void ShowRange(bool _active)
    {
        attackRange.ShowRange(_active);
    }

    // sortingLayer ����
    public void SetSortingLayer(string _layer)
    {
        for (int i = 0; i < heroes.Count; i++)
        {
            heroes[i].SetSortingLayer(_layer);
        }
    }
}
