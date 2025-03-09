using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class UIManager : MonoBehaviour
{
    // ���� ���̺� ���۽� ��Ÿ���� ������Ʈ
    [SerializeField]
    GameObject waveInfo;
    [SerializeField]
    TextMeshProUGUI waveInfoText;

    // ���� ���̺� ���۽� ��Ÿ���� ������Ʈ
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
    // ���� ü�� �ؽ�Ʈ
    [SerializeField]
    TextMeshProUGUI bossHPText;

    [Space(7)]
    // ���̺� ǥ�� �ؽ�Ʈ
    [SerializeField]
    TextMeshProUGUI waveTxt;

    // �ð� ǥ�� �ؽ�Ʈ
    [SerializeField]
    TextMeshProUGUI timerTxt;

    // ��ȯ�� �� �� ������
    [SerializeField]
    Image enemyGuageFill;
    [SerializeField]
    TextMeshProUGUI enemyGuageTxt;

    // �������� ��ȭ ǥ�� UI �ؽ�Ʈ
    [SerializeField]
    TextMeshProUGUI numOfGoldTxt;
    [SerializeField]
    TextMeshProUGUI numOfDiaTxt;
    [SerializeField]
    TextMeshProUGUI numOfHeroTxt;
    [SerializeField]
    TextMeshProUGUI summonPriceTxt;

    // �߰� ���� ��ȯ ��ư
    [SerializeField]
    GameObject AddBoss;

    // �ʱ� UI������ ����
    public void Init(int wave = 0, int time = 0, int nowNumOfEnemy = 0, int maxNumOfEnemy = 100, int numOfGold = 100, int numOfDia = 0, int nowNumOfHero = 0, int maxNumOfHero = 20, int summonPrice = 30)
    {
        // ���̺� 0���� �ʱ�ȭ
        SetWave(wave);

        // 0�ʷ� �ʱ�ȭ
        SetTimer(time);

        // ������ 0���� �ʱ�ȭ
        SetEnemyGuage(nowNumOfEnemy, maxNumOfEnemy);

        SetNumOfGold(numOfGold);

        SetNumOfDia(numOfDia);

        SetNumOfHero(nowNumOfHero, maxNumOfHero);

        SetSummonPrice(summonPrice);
    }

    // ���̺� ���� �Լ�
    public void SetWave(int wave)
    {
        waveTxt.text = $"WAVE {wave}";
        if(wave > 0)
        {
            if(wave % 5 == 0) StartCoroutine(ShowBossWaveInfo(wave));
            else StartCoroutine(ShowWaveInfo(wave));
        }
    } 

    // �ð� ���� �Լ�
    public void SetTimer(int time)
    {
        // �Ѱܹ��� �ð��� 0���ϸ� 0�ʷ� ǥ��
        if(time <= 0)
        {
            timerTxt.text = "00:00";
        }
        // �� �ܿ��� ����Ͽ� ǥ��
        else
        {
            int minute = time / 60;
            int second = time % 60;

            timerTxt.text = string.Format("{0:D2}:{1:D2}", minute, second);
        }
    }

    // ��ȯ�� �� ������ ����
    public void SetEnemyGuage(int nowNumOfEnemy, int maxNumOfEnemy)
    {
        enemyGuageFill.fillAmount = (float)nowNumOfEnemy / maxNumOfEnemy;
        enemyGuageTxt.text = $"{nowNumOfEnemy} / {maxNumOfEnemy}";
    }

    // ��� ���� UI�� ����
    public void SetNumOfGold(int numOfGold)
    {
        numOfGoldTxt.text = $"{numOfGold}";
    }

    // ���̾� ���� UI�� ����
    public void SetNumOfDia(int numOfDia)
    {
        numOfDiaTxt.text = $"{numOfDia}";
    }

    // ��ȯ ���� �� UI�� ����
    public void SetNumOfHero(int nowNumOfHero, int maxNumOfHero)
    {
        numOfHeroTxt.text = $"{nowNumOfHero} / {maxNumOfHero}";

        // ���� ��ȯ���� ���θ� �����ͼ� �����ϸ� �Ͼ�۾�, �Ұ����ϸ� �����۾�
        if (GameManager.instance.CanSummon())
        {
            summonPriceTxt.color = Color.white;
        }
        else
        {
            summonPriceTxt.color = Color.red;
        }
    }

    // ����� ��ȯ ����� UI�� ����
    public void SetSummonPrice(int summonPrice)
    {
        summonPriceTxt.text = $"{summonPrice}";

        // ��ȯ ���ɿ��ε� ǥ��
        if(GameManager.instance.CanSummon())
        {
            summonPriceTxt.color = Color.white;
        }
        else
        {
            summonPriceTxt.color = Color.red;
        }
    }

    // ��ȯ��ư ���� ��
    public void ClickSummonButton()
    {
        // ��ȯ ������ ��츸 ��ȯ ����
        if(GameManager.instance.CanSummon())
        {
            GameManager.instance.SummonHero();
        }
    }

    // ���̺� ���۽� ���̺� ������ UI �ִϸ��̼�
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

    // ���� ���̺� �� �ִϸ��̼�
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

    // �߰� ���� ��ȯ ��ư Ȱ��ȭ
    public void ActiveAddBoss()
    {
        AddBoss.SetActive(true);
    }

    // �߰� ���� ��ȯ ���� �� ����
    public void AddBossClick()
    {
        GameManager.instance.ClickAddBoss();
        AddBoss.SetActive(false);
    }
}
