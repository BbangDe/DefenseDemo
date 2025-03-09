using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    static public AIManager instance;

    [SerializeField]
    CharacterManager characterManager;

    // 보유 골드 수(소환 자원)
    int numOfGold;
    // 보유 다이아 수(도박 자원)
    int numOfDia;
    // 현재 소환된 영웅 수
    int nowNumOfHero;
    // 최대 소환 가능한 영웅 수
    int maxNumOfHero;
    // 소환 가격
    int summonPrice;

    float count;

    private void Start()
    {
        instance = this;

        numOfGold = 100;
        numOfDia = 0;
        nowNumOfHero = 0;
        maxNumOfHero = 20;
        summonPrice = 30;

        count = 0;
    }

    private void Update()
    {
        count += Time.deltaTime;

        if(count < 0.1f)
        {
            if (numOfGold >= summonPrice)
            {
                AddNumOfGold(-1 * summonPrice);
                summonPrice += 2;

                nowNumOfHero++;
                characterManager.CharacterRandomSpawn(0);

                count = 0f;
            }
        }
        else if (count < 0.2f)
        {
            characterManager.AutoUpgrade(0);
            count = 0f;
        }
    }

    public void AddNumOfGold(int num)
    {
        numOfGold += num;
    }

    public void AddNumOfDia(int num)
    {
        numOfDia += num;
    }
}
