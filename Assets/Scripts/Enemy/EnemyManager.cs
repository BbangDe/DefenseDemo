using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // 각 웨이브별 몬스터 소환 수 데이터
    WaveInfoList waveInfoList;

    // 일반 몬스터
    [SerializeField]
    EnemyControl normalMonster;
    List<EnemyControl> enemyObjectPool = new List<EnemyControl>();

    // 보스 몬스터
    [SerializeField]
    EnemyControl bossMonster;
    List<EnemyControl> bossEnemyObjectPool = new List<EnemyControl>();

    // 추가 보스 몬스터
    [SerializeField]
    EnemyControl addBossMonster;
    List<EnemyControl> addBossEnemyObjectPool = new List<EnemyControl>();

    [SerializeField]
    float spawnTerm;
    WaitForSeconds spawn_term;

    [SerializeField]
    List<Vector2> topTurningPoint;
    [SerializeField]
    List<Vector2> bottomTurningPoint;

    public void Init()
    {
        TextAsset waveInfoTxt = Resources.Load<TextAsset>("Json/waveInfo");
        waveInfoList = JsonUtility.FromJson<WaveInfoList>(waveInfoTxt.text);

        spawn_term = new WaitForSeconds(spawnTerm);
    }

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

    public int GetWaveTime(int wave)
    {
        return waveInfoList.waveInformation[wave - 1].time;
    }

    public void EnemySpawn(int wave)
    {
        StartCoroutine(StartEnemySpawn(wave));
    }

    public void AddBossSpawn(int wave)
    {
        StartCoroutine(AddBossSpawnStart(wave));
    }

    IEnumerator StartEnemySpawn(int wave)
    {
        for (int i = 0; i < waveInfoList.waveInformation[wave - 1].numOfNormalMonster; i++)
        {
            EnemyControl top_enemy = GetFromEnemyPool();
            top_enemy.SetHeight(0);
            top_enemy.SetSpeed(3f);
            top_enemy.SetHP(wave);
            top_enemy.SetEnemyManager(this);

            top_enemy.transform.SetParent(transform);
            top_enemy.SetInitPos();

            top_enemy.gameObject.SetActive(true);
            GameManager.instance.AddEnemyCount(1);

            yield return new WaitForSeconds(0.1f);
            
            EnemyControl bottom_enemy = GetFromEnemyPool();
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

        for (int i = 0; i < waveInfoList.waveInformation[wave - 1].numOfBossBonster; i++)
        {
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
