using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class UIManager : MonoBehaviour
{
    // 현재 웨이브 시작시 나타내는 오브젝트
    [SerializeField]
    GameObject waveInfo;
    [SerializeField]
    TextMeshProUGUI waveInfoText;

    // 보스 웨이브 시작시 나타내는 오브젝트
    [Space(3)]
    [SerializeField]
    GameObject bossWaveInfo;
    [SerializeField]
    Image bossSprite;
    [SerializeField]
    TextMeshProUGUI bossWaveInfoText;
    [SerializeField]
    TextMeshProUGUI bossWaveNameText;
    [SerializeField]
    TextMeshProUGUI bossWaveTimeText;

    [Space(7)]
    [SerializeField]
    TextMeshProUGUI damageText;
    // 보스 체력 텍스트
    [SerializeField]
    TextMeshProUGUI bossHPText;

    [Space(7)]
    // 웨이브 표시 텍스트
    [SerializeField]
    TextMeshProUGUI waveTxt;

    // 시간 표시 텍스트
    [SerializeField]
    TextMeshProUGUI timerTxt;

    // 소환된 적 수 게이지
    [SerializeField]
    Image enemyGuageFill;
    [SerializeField]
    TextMeshProUGUI enemyGuageTxt;

    // 소유중인 재화 표시 UI 텍스트
    [SerializeField]
    TextMeshProUGUI numOfGoldTxt;
    [SerializeField]
    TextMeshProUGUI numOfDiaTxt;
    [SerializeField]
    TextMeshProUGUI numOfHeroTxt;
    [SerializeField]
    TextMeshProUGUI summonPriceTxt;

    // 추가 보스 소환 버튼
    [SerializeField]
    GameObject AddBoss;

    // 초기 UI값으로 세팅
    public void Init(int wave = 0, int time = 0, int nowNumOfEnemy = 0, int maxNumOfEnemy = 100, int numOfGold = 100, int numOfDia = 0, int nowNumOfHero = 0, int maxNumOfHero = 20, int summonPrice = 30)
    {
        // 웨이브 0으로 초기화
        SetWave(wave);

        // 0초로 초기화
        SetTimer(time);

        // 게이지 0으로 초기화
        SetEnemyGuage(nowNumOfEnemy, maxNumOfEnemy);

        SetNumOfGold(numOfGold);

        SetNumOfDia(numOfDia);

        SetNumOfHero(nowNumOfHero, maxNumOfHero);

        SetSummonPrice(summonPrice);
    }

    // 웨이브 세팅 함수
    public void SetWave(int wave)
    {
        waveTxt.text = $"WAVE {wave}";
        if(wave > 0)
        {
            if(wave % 5 == 0) StartCoroutine(ShowBossWaveInfo(wave));
            else StartCoroutine(ShowWaveInfo(wave));
        }
    } 

    // 시간 세팅 함수
    public void SetTimer(int time)
    {
        // 넘겨받은 시간이 0이하면 0초로 표기
        if(time <= 0)
        {
            timerTxt.text = "00:00";
        }
        // 그 외에는 계산하여 표기
        else
        {
            int minute = time / 60;
            int second = time % 60;

            timerTxt.text = string.Format("{0:D2}:{1:D2}", minute, second);
        }
    }

    // 소환된 적 게이지 세팅
    public void SetEnemyGuage(int nowNumOfEnemy, int maxNumOfEnemy)
    {
        enemyGuageFill.fillAmount = (float)nowNumOfEnemy / maxNumOfEnemy;
        enemyGuageTxt.text = $"{nowNumOfEnemy} / {maxNumOfEnemy}";
    }

    // 골드 개수 UI에 세팅
    public void SetNumOfGold(int numOfGold)
    {
        numOfGoldTxt.text = $"{numOfGold}";
    }

    // 다이아 개수 UI에 세팅
    public void SetNumOfDia(int numOfDia)
    {
        numOfDiaTxt.text = $"{numOfDia}";
    }

    // 소환 유닛 수 UI에 세팅
    public void SetNumOfHero(int nowNumOfHero, int maxNumOfHero)
    {
        numOfHeroTxt.text = $"{nowNumOfHero} / {maxNumOfHero}";

        // 현재 소환가능 여부를 가져와서 가능하면 하얀글씨, 불가능하면 빨간글씨
        if (GameManager.instance.CanSummon())
        {
            summonPriceTxt.color = Color.white;
        }
        else
        {
            summonPriceTxt.color = Color.red;
        }
    }

    // 변경된 소환 비용을 UI에 세팅
    public void SetSummonPrice(int summonPrice)
    {
        summonPriceTxt.text = $"{summonPrice}";

        // 소환 가능여부도 표시
        if(GameManager.instance.CanSummon())
        {
            summonPriceTxt.color = Color.white;
        }
        else
        {
            summonPriceTxt.color = Color.red;
        }
    }

    // 소환버튼 선택 시
    public void ClickSummonButton()
    {
        // 소환 가능할 경우만 소환 시작
        if(GameManager.instance.CanSummon())
        {
            GameManager.instance.SummonHero();
        }
    }

    // 웨이브 시작시 웨이브 보여줄 UI 애니메이션
    IEnumerator ShowWaveInfo(int wave)
    {
        waveInfoText.text = $"WAVE {wave}";
        waveInfo.SetActive(true);

        float diffY = 1 / 0.3f;

        float count = 0f;

        while(count < 0.3f)
        {
            count += Time.deltaTime;
            Vector3 _scale = waveInfo.transform.localScale;
            _scale.y += diffY * Time.deltaTime;
            waveInfo.transform.localScale = _scale;

            yield return null;
        }

        waveInfo.transform.localScale = Vector3.one;


        float diffX = 0.15f / 0.3f;
        diffY = 1f;
        
        count = 0f;

        while (count < 0.3f)
        {
            count += Time.deltaTime;
            Vector3 _scale = waveInfoText.transform.localScale;
            _scale.x += diffX * Time.deltaTime;
            _scale.y += diffY * Time.deltaTime;
            waveInfoText.transform.localScale = _scale;

            yield return null;
        }

        Vector3 scale = waveInfoText.transform.localScale;
        scale.x = 1.1f;
        scale.y = 1.3f;
        waveInfoText.transform.localScale = scale;


        diffX = 0.1f / 0.2f;
        diffY = 0.3f / 0.2f;

        count = 0f;

        while (count < 0.2f)
        {
            count += Time.deltaTime;
            Vector3 _scale = waveInfoText.transform.localScale;
            _scale.x -= diffX * Time.deltaTime;
            _scale.y -= diffY * Time.deltaTime;
            waveInfoText.transform.localScale = _scale;

            yield return null;
        }

        waveInfoText.transform.localScale = Vector3.one;

        yield return new WaitForSeconds(0.6f);

        waveInfo.SetActive(false);
        scale = waveInfo.transform.localScale;
        scale.y = 0f;
        waveInfo.transform.localScale = scale;
    }

    // 보스 웨이브 시 애니메이션
    IEnumerator ShowBossWaveInfo(int wave)
    {
        bossWaveInfoText.text = $"WAVE {wave}";
        bossWaveInfo.SetActive(true);

        float diffY = 1 / 0.3f;

        float count = 0f;

        while (count < 0.3f)
        {
            count += Time.deltaTime;
            Vector3 _scale = bossWaveInfo.transform.localScale;
            _scale.y += diffY * Time.deltaTime;
            bossWaveInfo.transform.localScale = _scale;

            yield return null;
        }

        bossWaveInfo.transform.localScale = Vector3.one;


        diffY = 100f / 0.3f;

        count = 0f;

        while (count < 0.3f)
        {
            count += Time.deltaTime;
            Vector3 _pos = bossWaveInfoText.rectTransform.position;
            _pos.y += diffY * Time.deltaTime;
            bossWaveInfoText.rectTransform.position = _pos;

            yield return null;
        }


        float diffX = 1 / 0.2f;
        diffY = 1 / 0.2f;

        float diffX2 = 5 / 0.2f;
        float diffY2 = 5 / 0.2f;

        count = 0f;

        while (count < 0.2f)
        {
            count += Time.deltaTime;

            Vector3 _scale = bossWaveNameText.rectTransform.localScale;
            _scale.x += diffX * Time.deltaTime;
            _scale.y += diffY * Time.deltaTime;
            bossWaveNameText.rectTransform.localScale = _scale;

            _scale = bossSprite.rectTransform.localScale;
            _scale.x += diffX2 * Time.deltaTime;
            _scale.y += diffY2 * Time.deltaTime;
            bossSprite.rectTransform.localScale = _scale;

            yield return null;
        }

        bossWaveNameText.rectTransform.localScale = Vector3.one;
        bossSprite.rectTransform.localScale = Vector3.one * 5f;

        diffX = 1 / 0.2f;
        diffY = 1 / 0.2f;

        count = 0f;

        while (count < 0.2f)
        {
            count += Time.deltaTime;

            Vector3 _scale = bossWaveTimeText.rectTransform.localScale;
            _scale.x += diffX * Time.deltaTime;
            _scale.y += diffY * Time.deltaTime;
            bossWaveTimeText.rectTransform.localScale = _scale;

            yield return null;
        }

        bossWaveTimeText.rectTransform.localScale = Vector3.one;

        yield return new WaitForSeconds(0.8f);

        bossWaveInfo.SetActive(false);

        Vector3 scale = bossWaveInfo.transform.localScale;
        scale.y = 0;
        bossWaveInfo.transform.localScale = scale;

        Vector3 pos = bossWaveInfoText.rectTransform.position;
        pos.y = 0f;
        bossWaveInfoText.rectTransform.position = pos;
        bossWaveNameText.rectTransform.localScale = Vector3.zero;
        bossWaveTimeText.rectTransform.localScale = Vector3.zero;
        bossSprite.rectTransform.localScale = Vector3.zero;
    }

    // 추가 보스 소환 버튼 활성화
    public void ActiveAddBoss()
    {
        AddBoss.SetActive(true);
    }

    // 추가 보스 소환 선택 시 실행
    public void AddBossClick()
    {
        GameManager.instance.ClickAddBoss();
        AddBoss.SetActive(false);
    }
}
