using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���̺꺰 ��ȯ ���� �� �� �ð�
[Serializable]
struct WaveInfo
{
    public int wave;
    public int numOfNormalMonster;
    public int numOfBossBonster;
    public int time;
}

[Serializable]
struct WaveInfoList
{
    public WaveInfo[] waveInformation;
}

public class EnemyManager : MonoBehaviour
{
    // �� ���̺꺰 ���� ��ȯ �� ������
    WaveInfoList waveInfoList;

    // �Ϲ� ����
    [SerializeField]
    EnemyControl normalMonster;
    List<EnemyControl> enemyObjectPool = new List<EnemyControl>();

    // ���� ����
    [SerializeField]
    EnemyControl bossMonster;
    List<EnemyControl> bossEnemyObjectPool = new List<EnemyControl>();

    // �߰� ���� ����
    [SerializeField]
    EnemyControl addBossMonster;
    List<EnemyControl> addBossEnemyObjectPool = new List<EnemyControl>();

    // ���� ��ȯ 1���� 1���� ���� �ҽð�
    [SerializeField]
    float spawnTerm;
    WaitForSeconds spawn_term;

    // �� �𼭸� ��ǥ ������
    [SerializeField]
    List<Vector2> topTurningPoint;
    [SerializeField]
    List<Vector2> bottomTurningPoint;

    public void Init()
    {
        // ���̺� ������ �о ����
        TextAsset waveInfoTxt = Resources.Load<TextAsset>("Json/waveInfo");
        waveInfoList = JsonUtility.FromJson<WaveInfoList>(waveInfoTxt.text);

        spawn_term = new WaitForSeconds(spawnTerm);
    }

    // �� ���� ������Ʈ�� ������Ʈ Ǯ�� ����Ͽ� ��ȯ
    // �Ϲ� ���� Ǯ���� ��������
    EnemyControl GetFromEnemyPool()
    {
        for(int i = 0; i < enemyObjectPool.Count; i++)
        {
            if(!enemyObjectPool[i].gameObject.activeSelf)
            {
                return enemyObjectPool[i];
            }
        }

        EnemyControl enemy = Instantiate(normalMonster);
        enemyObjectPool.Add(enemy);
        return enemy;
    }

    // �������� Ǯ���� ��������
    EnemyControl GetFromBossEnemyPool()
    {
        for (int i = 0; i < bossEnemyObjectPool.Count; i++)
        {
            if (!bossEnemyObjectPool[i].gameObject.activeSelf)
            {
                return bossEnemyObjectPool[i];
            }
        }

        EnemyControl enemy = Instantiate(bossMonster);
        bossEnemyObjectPool.Add(enemy);
        return enemy;
    }

    // �߰���������Ǯ���� ��������
    EnemyControl GetFromAddBossEnemyPool()
    {
        for (int i = 0; i < addBossEnemyObjectPool.Count; i++)
        {
            if (!addBossEnemyObjectPool[i].gameObject.activeSelf)
            {
                return addBossEnemyObjectPool[i];
            }
        }

        EnemyControl enemy = Instantiate(addBossMonster);
        addBossEnemyObjectPool.Add(enemy);
        return enemy;
    }

    // �Ѱܹ��� �ε����� ���� ��ǥ�� ����
    public Vector3 GetEndingPosition(int idx, int height)
    {
        if(height == 0)
        {
            return topTurningPoint[idx];
        }
        else
        {
            return bottomTurningPoint[idx];
        }
    }

    // �� ���̺��� �÷��̽ð� ����
    public int GetWaveTime(int wave)
    {
        return waveInfoList.waveInformation[wave - 1].time;
    }

    // �� ��ȯ �ڷ�ƾ ����
    public void EnemySpawn(int wave)
    {
        StartCoroutine(StartEnemySpawn(wave));
    }

    // �߰� ���� ���� ��ȯ ����
    public void AddBossSpawn(int wave)
    {
        StartCoroutine(AddBossSpawnStart(wave));
    }

    // �� ���� ��ȯ ���� �ڷ�ƾ
    IEnumerator StartEnemySpawn(int wave)
    {
        // �� ���̺��� �Ϲݸ��� ��ȯ ���ڸ�ŭ �ݺ�
        for (int i = 0; i < waveInfoList.waveInformation[wave - 1].numOfNormalMonster; i++)
        {
            // ���� �����ʿ��� ��ȯ�� ���� Ǯ���� ������
            EnemyControl top_enemy = GetFromEnemyPool();
            // ��ġ �������� ����
            top_enemy.SetHeight(0);
            // ���� ���ǵ� �� ü�� ����
            top_enemy.SetSpeed(3f);
            top_enemy.SetHP(wave);
            // �� EnemyManager ����
            top_enemy.SetEnemyManager(this);

            // �� ���ʹ� �� ������Ʈ �Ʒ��� ����
            top_enemy.transform.SetParent(transform);
            top_enemy.SetInitPos();

            // ��ȯ�� �� ������Ʈ Ȱ��ȭ �� �� �ο� �� 1����
            top_enemy.gameObject.SetActive(true);
            GameManager.instance.AddEnemyCount(1);

            yield return new WaitForSeconds(0.1f);
            
            // �Ʒ��ʵ� ���� ������� �߰�
            EnemyControl bottom_enemy = GetFromEnemyPool();
            bottom_enemy.SetHeight(1);
            bottom_enemy.SetSpeed(3f);
            bottom_enemy.SetHP(wave);
            bottom_enemy.SetEnemyManager(this);
            
            bottom_enemy.transform.SetParent(transform);
            bottom_enemy.SetInitPos();

            bottom_enemy.gameObject.SetActive(true);
            GameManager.instance.AddEnemyCount(1);
            
            // ���� �Ҹ�ŭ ��� �� �� ����
            yield return spawn_term;
        }

        // �� ���̺��� �������� ����ŭ ���� ����
        for (int i = 0; i < waveInfoList.waveInformation[wave - 1].numOfBossBonster; i++)
        {
            // ��ȯ ����� �Ϲݰ� ����
            EnemyControl top_enemy = GetFromBossEnemyPool();
            top_enemy.SetHeight(0);
            top_enemy.SetSpeed(3f);
            top_enemy.SetHP(wave);
            top_enemy.SetEnemyManager(this);

            top_enemy.transform.SetParent(transform);
            top_enemy.SetInitPos();

            top_enemy.gameObject.SetActive(true);
            GameManager.instance.AddEnemyCount(1);

            yield return new WaitForSeconds(0.1f);

            EnemyControl bottom_enemy = GetFromBossEnemyPool();
            bottom_enemy.SetHeight(1);
            bottom_enemy.SetSpeed(3f);
            bottom_enemy.SetHP(wave);
            bottom_enemy.SetEnemyManager(this);
            
            bottom_enemy.transform.SetParent(transform);
            bottom_enemy.SetInitPos();

            bottom_enemy.gameObject.SetActive(true);
            GameManager.instance.AddEnemyCount(1);

            yield return spawn_term;
        }
    }

    // �߰� �������� ��ȯ�� ������ ����
    IEnumerator AddBossSpawnStart(int wave)
    {
        EnemyControl top_enemy = GetFromAddBossEnemyPool();
        top_enemy.SetHeight(0);
        top_enemy.SetSpeed(3f);
        top_enemy.SetHP(wave);
        top_enemy.SetEnemyManager(this);

        top_enemy.transform.SetParent(transform);
        top_enemy.SetInitPos();

        top_enemy.gameObject.SetActive(true);
        GameManager.instance.AddEnemyCount(1);

        yield return new WaitForSeconds(0.1f);

        EnemyControl bottom_enemy = GetFromAddBossEnemyPool();
        bottom_enemy.SetHeight(1);
        bottom_enemy.SetSpeed(3f);
        bottom_enemy.SetHP(wave);
        bottom_enemy.SetEnemyManager(this);

        bottom_enemy.transform.SetParent(transform);
        bottom_enemy.SetInitPos();

        bottom_enemy.gameObject.SetActive(true);
        GameManager.instance.AddEnemyCount(1);
    }
}
