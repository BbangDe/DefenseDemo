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

    // AI 데이터 초기화
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
        // 시간 카운트해서 AI 움직임에 약간의 딜레이 추가
        count += Time.deltaTime;

        // 소환 버튼은 0.1초 간격으로 누르기
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
        // 그 외의 조합은 0.2초 딜레이
        else if (count < 0.2f)
        {
            characterManager.AutoUpgrade(0);
            count = 0f;
        }
    }

    // AI의 골드 수 증감함수
    public void AddNumOfGold(int num)
    {
        numOfGold += num;
    }

    // AI의 다이아 수 증감함수
    public void AddNumOfDia(int num)
    {
        numOfDia += num;
    }
}
