using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 인스턴스 생성
    public static GameManager instance;

    [Space(5)]
    [Header("매니저 변수")]
    // 매니저 스트립트 연동
    [SerializeField]
    UIManager uiManager;
    [SerializeField]
    EnemyManager enemyManager;
    [SerializeField]
    CharacterManager characterManager;
    [SerializeField]
    AIManager aiManager;

    [Space(5)]
    [Header("UI 변수")]
    // 웨이브 변경까지 남은 시간
    int remainTerm;
    // 현재 웨이브
    int wave;
    // 현재 흐른 시간
    int time;
    // 대기시간 캐싱 변수
    WaitForSeconds one_second;

    // 현재 소환된 적 수
    int nowNumOfEnemy;
    // 최대 소환 적 수
    int maxNumOfEnemy;

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

    int upgradeBase1;
    int upgradeBase2;
    int upgradeBase3;
    int upgradeBase4;

    int upgradeLv1;
    int upgradeLv2;
    int upgradeLv3;
    int upgradeLv4;

    int upgradePrice1;
    int upgradePrice2;
    int upgradePrice3;
    int upgradePrice4;

    int gamblePrice1;
    int gamblePrice2;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    private void Start()
    {
        one_second = new WaitForSeconds(1f);

        Init();
    }

    void Init()
    {
        // 적 정보 초기화
        enemyManager.Init();

        // 캐릭터 정보 초기화
        characterManager.Init();

        // UI 초기화
        // 웨이브 및 시간 초기화
        remainTerm = 2;
        wave = 0;
        time = 0;
        nowNumOfEnemy = 0;
        maxNumOfEnemy = 100;

        numOfGold = 100;
        numOfDia = 0;
        nowNumOfHero = 0;
        maxNumOfHero = 20;
        summonPrice = 30;

        upgradeBase1 = 30;
        upgradeBase2 = 50;
        upgradeBase3 = 3;
        upgradeBase4 = 100;

        upgradeLv1 = 1;
        upgradeLv2 = 1;
        upgradeLv3 = 1;
        upgradeLv4 = 1;

        upgradePrice1 = 30;
        upgradePrice2 = 50;
        upgradePrice3 = 2;
        upgradePrice4 = 100;

        gamblePrice1 = 1;
        gamblePrice2 = 1;

        uiManager.Init(wave, time, nowNumOfEnemy, maxNumOfEnemy, numOfGold, numOfDia, nowNumOfHero, maxNumOfHero, summonPrice);

        StartCoroutine(TimeCounter());
    }

    public Transform GetUIParent()
    {
        return uiManager.transform;
    }

    public void AddEnemyCount(int num)
    {
        nowNumOfEnemy += num;
        if(nowNumOfEnemy < 0) nowNumOfEnemy = 0;
        uiManager.SetEnemyGuage(nowNumOfEnemy, maxNumOfEnemy);
    }

    public void SummonHero()
    {
        AddNumOfGold(-1 * summonPrice);
        summonPrice += 2;
        uiManager.SetSummonPrice(summonPrice);

        characterManager.CharacterRandomSpawn(1);
    }

    public void SetNowNumOfHero(int num)
    {
        nowNumOfHero += num;
        if(nowNumOfHero < 0) nowNumOfHero = 0;
        uiManager.SetNumOfHero(nowNumOfHero, maxNumOfHero);
    }

    public bool CanSummon()
    {
        if (nowNumOfHero == maxNumOfHero) return false;

        return numOfGold >= summonPrice;
    }

    public int GetNumOfGold()
    {
        return numOfGold;
    }

    public int GetNumOfDia()
    {
        return numOfDia;
    }

    public void AddNumOfGold(int num)
    {
        numOfGold += num;
        uiManager.SetNumOfGold(numOfGold);
        uiManager.SetSummonPrice(summonPrice);
    }

    public void AddNumOfDia(int num)
    {
        numOfDia += num;
        uiManager.SetNumOfDia(numOfDia);
    }

    public bool CanGamble(int grade)
    {
        switch(grade)
        {
            case 0:
                return numOfDia >= gamblePrice1;
            case 1:
                return numOfDia >= gamblePrice2;
        }

        return false;
    }

    public bool CanUpgrade(int grade)
    {
        switch(grade)
        {
            case 0:
                return numOfGold >= upgradePrice1;
            case 1:
                return numOfGold >= upgradePrice2;
            case 2:
                return numOfDia >= upgradePrice3;
            case 3:
                return numOfGold >= upgradePrice4;
        }

        return false;
    }

    public int GetUpgradePrice(int grade)
    {
        switch (grade)
        {
            case 0:
                return upgradePrice1;
            case 1:
                return upgradePrice2;
            case 2:
                return upgradePrice3;
            case 3:
                return upgradePrice4;
        }

        return 0;
    }

    public int GetUpgradeLevel(int grade)
    {
        switch (grade)
        {
            case 0:
                return upgradeLv1;
            case 1:
                return upgradeLv2;
            case 2:
                return upgradeLv3;
            case 3:
                return upgradeLv4;
        }

        return 0;
    }

    public void SetUpgrade(int grade)
    {
        switch (grade)
        {
            case 0:
                AddNumOfGold(-1 * upgradePrice1);
                upgradeLv1++;
                upgradePrice1 = upgradeBase1 * upgradeLv1;
                break;
            case 1:
                AddNumOfGold(-1 * upgradePrice2);
                upgradeLv2++;
                upgradePrice2 = upgradeBase2 * upgradeLv2;
                break;
            case 2:
                AddNumOfDia(-1 * upgradePrice3);
                upgradeLv3++;
                upgradePrice3 = upgradeBase3 * (upgradeLv3 / 2);
                break;
            case 3:
                AddNumOfGold(-1 * upgradePrice4);
                upgradeLv4++;
                characterManager.UpgradeWeight();
                break;
        }
    }

    public void ClickAddBoss()
    {
        enemyManager.AddBossSpawn(wave);
    }

    // 1초씩 시간 카운트
    IEnumerator TimeCounter()
    {
        while(true)
        {
            yield return one_second;

            // 시간 흐름 적용
            if(time > 0)
            {
                time -= 1;
            }
            uiManager.SetTimer(time);

            // 웨이브 변동 적용
            remainTerm -= 1;
            if (remainTerm == 0)
            {
                wave += 1;
                uiManager.SetWave(wave);
                enemyManager.EnemySpawn(wave);
                time = enemyManager.GetWaveTime(wave);
                remainTerm = time;
                uiManager.SetTimer(time);

                if(wave % 8 == 0)
                {
                    uiManager.ActiveAddBoss();
                }
            }
        }
    }
}
