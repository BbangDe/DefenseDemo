using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // �ν��Ͻ� ����
    public static GameManager instance;

    [Space(5)]
    [Header("�Ŵ��� ����")]
    // �Ŵ��� ��Ʈ��Ʈ ����
    [SerializeField]
    UIManager uiManager;
    [SerializeField]
    EnemyManager enemyManager;
    [SerializeField]
    CharacterManager characterManager;
    [SerializeField]
    AIManager aiManager;

    [Space(5)]
    [Header("UI ����")]
    // ���̺� ������� ���� �ð�
    int remainTerm;
    // ���� ���̺�
    int wave;
    // ���� �帥 �ð�
    int time;
    // ���ð� ĳ�� ����
    WaitForSeconds one_second;

    // ���� ��ȯ�� �� ��
    int nowNumOfEnemy;
    // �ִ� ��ȯ �� ��
    int maxNumOfEnemy;

    // ���� ��� ��(��ȯ �ڿ�)
    int numOfGold;
    // ���� ���̾� ��(���� �ڿ�)
    int numOfDia;
    // ���� ��ȯ�� ���� ��
    int nowNumOfHero;
    // �ִ� ��ȯ ������ ���� ��
    int maxNumOfHero;
    // ��ȯ ����
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
        // �� ���� �ʱ�ȭ
        enemyManager.Init();

        // ĳ���� ���� �ʱ�ȭ
        characterManager.Init();

        // UI �ʱ�ȭ
        // ���̺� �� �ð� �ʱ�ȭ
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

    // 1�ʾ� �ð� ī��Ʈ
    IEnumerator TimeCounter()
    {
        while(true)
        {
            yield return one_second;

            // �ð� �帧 ����
            if(time > 0)
            {
                time -= 1;
            }
            uiManager.SetTimer(time);

            // ���̺� ���� ����
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
