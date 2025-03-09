using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 웨이브별 소환 몬스터 수 및 시간
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

    // 몬스터 소환 1마리 1마리 사이 텀시간
    [SerializeField]
    float spawnTerm;
    WaitForSeconds spawn_term;

    // 각 모서리 좌표 데이터
    [SerializeField]
    List<Vector2> topTurningPoint;
    [SerializeField]
    List<Vector2> bottomTurningPoint;

    public void Init()
    {
        // 웨이브 데이터 읽어서 저장
        TextAsset waveInfoTxt = Resources.Load<TextAsset>("Json/waveInfo");
        waveInfoList = JsonUtility.FromJson<WaveInfoList>(waveInfoTxt.text);

        spawn_term = new WaitForSeconds(spawnTerm);
    }

    // 적 몬스터 오브젝트는 오브젝트 풀을 사용하여 소환
    // 일반 몬스터 풀에서 가져오기
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

    // 보스몬스터 풀에서 가져오기
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

    // 추가보스몬스터풀에서 가져오기
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

    // 넘겨받은 인덱스를 보고 좌표를 리턴
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

    // 현 웨이브의 플레이시간 리턴
    public int GetWaveTime(int wave)
    {
        return waveInfoList.waveInformation[wave - 1].time;
    }

    // 적 소환 코루틴 시작
    public void EnemySpawn(int wave)
    {
        StartCoroutine(StartEnemySpawn(wave));
    }

    // 추가 보스 몬스터 소환 시작
    public void AddBossSpawn(int wave)
    {
        StartCoroutine(AddBossSpawnStart(wave));
    }

    // 적 몬스터 소환 시작 코루틴
    IEnumerator StartEnemySpawn(int wave)
    {
        // 현 웨이브의 일반몬스터 소환 숫자만큼 반복
        for (int i = 0; i < waveInfoList.waveInformation[wave - 1].numOfNormalMonster; i++)
        {
            // 위쪽 스포너에서 소환할 몬스터 풀에서 꺼내기
            EnemyControl top_enemy = GetFromEnemyPool();
            // 위치 위쪽으로 설정
            top_enemy.SetHeight(0);
            // 몬스터 스피드 및 체력 설정
            top_enemy.SetSpeed(3f);
            top_enemy.SetHP(wave);
            // 현 EnemyManager 연동
            top_enemy.SetEnemyManager(this);

            // 적 몬스터는 현 오브젝트 아래에 생성
            top_enemy.transform.SetParent(transform);
            top_enemy.SetInitPos();

            // 소환된 적 오브젝트 활성화 및 적 인원 수 1증가
            top_enemy.gameObject.SetActive(true);
            GameManager.instance.AddEnemyCount(1);

            yield return new WaitForSeconds(0.1f);
            
            // 아래쪽도 같은 방식으로 추가
            EnemyControl bottom_enemy = GetFromEnemyPool();
            bottom_enemy.SetHeight(1);
            bottom_enemy.SetSpeed(3f);
            bottom_enemy.SetHP(wave);
            bottom_enemy.SetEnemyManager(this);
            
            bottom_enemy.transform.SetParent(transform);
            bottom_enemy.SetInitPos();

            bottom_enemy.gameObject.SetActive(true);
            GameManager.instance.AddEnemyCount(1);
            
            // 스폰 텀만큼 대기 후 재 스폰
            yield return spawn_term;
        }

        // 현 웨이브의 보스몬스터 수만큼 보스 스폰
        for (int i = 0; i < waveInfoList.waveInformation[wave - 1].numOfBossBonster; i++)
        {
            // 소환 방식은 일반과 동일
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

    // 추가 보스몬스터 소환도 보스와 동일
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
