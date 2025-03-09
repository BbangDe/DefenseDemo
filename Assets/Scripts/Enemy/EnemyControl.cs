using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class EnemyControl : MonoBehaviour
{
    // EnemyManager 연동
    EnemyManager enemyManager;

    // 적 오브젝트의 스프라이트
    [SerializeField]
    SpriteRenderer enemySprite;

    // 적 오브젝트 최종 크기 데이터(소환시 점점 커지며 등장할 때 사용)
    [SerializeField]
    Vector2 monsterScale;
    // 적 오브젝트 등장 애니메이션 작동 시간
    [SerializeField]
    float changeAmount;

    // 적 스탯 데이터
    float speed;
    float nowHP;
    float maxHP;
    [SerializeField]
    float baseHP;

    // 위, 아래 스폰 위치
    int height;

    // 적이 이동하는 맵의 방향대로 각 모서리를 스폰지점을 0으로 해서 순서대로 0 1 2 3 으로 체크
    int startingPos;
    int endingPos;

    // 적 리지드바디
    [SerializeField]
    Rigidbody2D rb;

    // 적 이동방향
    public Vector2 dir;

    // 적 체력 게이지 트랜스폼
    [SerializeField]
    Transform hp_fill;
    // 체력 최대치일 때의 x크기값
    [SerializeField]
    float maxFill;

    // 보스 몬스터 유무
    [SerializeField]
    bool isBoss;
    // 보스몬스터 체력 표시 텍스트
    [SerializeField]
    TextMeshProUGUI hpText;
    TextMeshProUGUI thisHpText;

    [SerializeField]
    TextMeshProUGUI damageText;
    TextMeshProUGUI thisDamageText;

    private void OnEnable()
    {
        // 등장 애니메이션 실행
        StartCoroutine(ScaleControl());
    }

    private void OnDisable()
    {
        // 사라질때 초기 크기로 변경
        transform.localScale = Vector2.zero;
        Destroy(thisHpText);
    }

    private void Update()
    {
        // 보스이면서 체력 텍스트 오브젝트가 null이 아니라면
        if(isBoss && thisHpText != null)
        {
            // 현재 체력 표시
            thisHpText.text = ((int)nowHP).ToString();
            thisHpText.transform.position = Camera.main.WorldToScreenPoint(transform.position + (Vector3.up * 0.6f));
            thisHpText.gameObject.SetActive(true);
        }

        // 최대체력과의 비율을 구해서 게이지 x스케일 세팅
        float fillRate = nowHP / maxHP;
        Vector3 scale = hp_fill.localScale;
        scale.x = maxFill * fillRate;
        hp_fill.localScale = scale;

        // 시작 꼭지점, 끝 꼭지점을 통해 이동방향 벡터를 계산
        Vector3 st = enemyManager.GetEndingPosition(startingPos, height);
        Vector3 ed = enemyManager.GetEndingPosition(endingPos, height);

        // 벡터를 통해 위 아래 좌 우 중 어디인지 저장
        if(st.y < ed.y)
        {
            dir = Vector2.up;
        }
        else if (st.y > ed.y)
        {
            dir = Vector2.down;
        }

        if (st.x < ed.x)
        {
            dir = Vector2.right;
        }
        else if (st.x > ed.x)
        {
            dir = Vector2.left;
        }

        // 이동 방향으로 적 이동
        Vector3 newPos = rb.position + dir * speed * Time.deltaTime;
        rb.velocity = Vector2.zero;
        rb.MovePosition(newPos);

        // 각 모서리 끝에 도착했는지 체크
        // 이동 방향에 따라 스프라이트 flip
        if(dir == Vector2.up)
        {
            enemySprite.flipX = rb.position.x < 0;

            if (rb.position.y >= ed.y)
            {
                rb.position = ed;

                startingPos = endingPos;
                endingPos += 1;
                if (endingPos > 3) endingPos = 0;
            }
        }
        else if (dir == Vector2.down)
        {
            enemySprite.flipX = rb.position.x < 0;

            if (rb.position.y <= ed.y)
            {
                rb.position = ed;

                startingPos = endingPos;
                endingPos += 1;
                if (endingPos > 3) endingPos = 0;
            }
        }
        else if (dir == Vector2.right)
        {
            if (rb.position.x >= ed.x)
            {
                rb.position = ed;

                startingPos = endingPos;
                endingPos += 1;
                if (endingPos > 3) endingPos = 0;
            }
        }
        else if (dir == Vector2.left)
        {
            if (rb.position.x <= ed.x)
            {
                rb.position = ed;

                startingPos = endingPos;
                endingPos += 1;
                if (endingPos > 3) endingPos = 0;
            }
        }
    }

    // 초기 스폰위치 세팅
    public void SetInitPos()
    {
        transform.position = enemyManager.GetEndingPosition(startingPos, height);
    }

    // 위, 아래 중 어디 스폰인지 세팅 후 첫 이동방향 0 > 1 설정
    public void SetHeight(int _height)
    {
        height = _height;

        startingPos = 0;
        endingPos = 1;
    }

    // 적 스피드 설정
    public void SetSpeed(float _speed)
    {
        speed = _speed;
    }

    // 적 체력 설정
    public void SetHP(int wave)
    {
        maxHP = baseHP + baseHP * wave;
        nowHP = maxHP;
    }

    // EnemyManager와 연동 함수
    public void SetEnemyManager(EnemyManager _enemyManager)
    {
        enemyManager = _enemyManager;
    }

    // 플레이어로부터 입은 데미지 처리
    public void GetDamage(float _attack)
    {
        // 기본 공격력에서 0.6 ~ 1.6배의 데미지로 처리
        float _rate = Random.Range(0.6f, 1.6f);
        float damage = _attack * _rate;

        // 체력 처리
        nowHP -= damage;
        if(nowHP <= 0)
        {
            SetDeath();
        }
    }

    // 죽을 경우
    void SetDeath()
    {
        // 보스, 일반 몬스터 보상 처리
        if(isBoss)
        {
            // 50골드, 2다이아 보상
            GameManager.instance.AddNumOfGold(50);
            GameManager.instance.AddNumOfDia(2);

            AIManager.instance.AddNumOfGold(50);
            AIManager.instance.AddNumOfDia(2);
        }
        else
        {
            // 3골드 보상
            GameManager.instance.AddNumOfGold(3);

            AIManager.instance.AddNumOfGold(3);
        }
        
        // 적 카운트 수 1감소 및 오브젝트 비활성화
        GameManager.instance.AddEnemyCount(-1);
        gameObject.SetActive(false);
    }

    // 적 등장 애니메이션
    IEnumerator ScaleControl()
    {
        // 한 반복 턴에 변경시킬 정도값
        float diffX = monsterScale.x / changeAmount;
        float diffY = monsterScale.y / changeAmount;

        float count = 0f;

        // 애니메이션 시간동안 스케일 조정하며 애니메이션 실행
        while(count < changeAmount)
        {
            count += Time.deltaTime;

            Vector3 _scale = transform.localScale;
            _scale.x += diffX * Time.deltaTime;
            _scale.y += diffY * Time.deltaTime;
            transform.localScale = _scale;

            yield return null;
        }

        transform.localScale = monsterScale;

        if(isBoss)
            thisHpText = Instantiate(hpText, GameManager.instance.GetUIParent());
    }
}
