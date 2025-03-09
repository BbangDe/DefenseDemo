using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class EnemyControl : MonoBehaviour
{
    // EnemyManager ����
    EnemyManager enemyManager;

    // �� ������Ʈ�� ��������Ʈ
    [SerializeField]
    SpriteRenderer enemySprite;

    // �� ������Ʈ ���� ũ�� ������(��ȯ�� ���� Ŀ���� ������ �� ���)
    [SerializeField]
    Vector2 monsterScale;
    // �� ������Ʈ ���� �ִϸ��̼� �۵� �ð�
    [SerializeField]
    float changeAmount;

    // �� ���� ������
    float speed;
    float nowHP;
    float maxHP;
    [SerializeField]
    float baseHP;

    // ��, �Ʒ� ���� ��ġ
    int height;

    // ���� �̵��ϴ� ���� ������ �� �𼭸��� ���������� 0���� �ؼ� ������� 0 1 2 3 ���� üũ
    int startingPos;
    int endingPos;

    // �� ������ٵ�
    [SerializeField]
    Rigidbody2D rb;

    // �� �̵�����
    public Vector2 dir;

    // �� ü�� ������ Ʈ������
    [SerializeField]
    Transform hp_fill;
    // ü�� �ִ�ġ�� ���� xũ�Ⱚ
    [SerializeField]
    float maxFill;

    // ���� ���� ����
    [SerializeField]
    bool isBoss;
    // �������� ü�� ǥ�� �ؽ�Ʈ
    [SerializeField]
    TextMeshProUGUI hpText;
    TextMeshProUGUI thisHpText;

    [SerializeField]
    TextMeshProUGUI damageText;
    TextMeshProUGUI thisDamageText;

    private void OnEnable()
    {
        // ���� �ִϸ��̼� ����
        StartCoroutine(ScaleControl());
    }

    private void OnDisable()
    {
        // ������� �ʱ� ũ��� ����
        transform.localScale = Vector2.zero;
        Destroy(thisHpText);
    }

    private void Update()
    {
        // �����̸鼭 ü�� �ؽ�Ʈ ������Ʈ�� null�� �ƴ϶��
        if(isBoss && thisHpText != null)
        {
            // ���� ü�� ǥ��
            thisHpText.text = ((int)nowHP).ToString();
            thisHpText.transform.position = Camera.main.WorldToScreenPoint(transform.position + (Vector3.up * 0.6f));
            thisHpText.gameObject.SetActive(true);
        }

        // �ִ�ü�°��� ������ ���ؼ� ������ x������ ����
        float fillRate = nowHP / maxHP;
        Vector3 scale = hp_fill.localScale;
        scale.x = maxFill * fillRate;
        hp_fill.localScale = scale;

        // ���� ������, �� �������� ���� �̵����� ���͸� ���
        Vector3 st = enemyManager.GetEndingPosition(startingPos, height);
        Vector3 ed = enemyManager.GetEndingPosition(endingPos, height);

        // ���͸� ���� �� �Ʒ� �� �� �� ������� ����
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

        // �̵� �������� �� �̵�
        Vector3 newPos = rb.position + dir * speed * Time.deltaTime;
        rb.velocity = Vector2.zero;
        rb.MovePosition(newPos);

        // �� �𼭸� ���� �����ߴ��� üũ
        // �̵� ���⿡ ���� ��������Ʈ flip
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

    // �ʱ� ������ġ ����
    public void SetInitPos()
    {
        transform.position = enemyManager.GetEndingPosition(startingPos, height);
    }

    // ��, �Ʒ� �� ��� �������� ���� �� ù �̵����� 0 > 1 ����
    public void SetHeight(int _height)
    {
        height = _height;

        startingPos = 0;
        endingPos = 1;
    }

    // �� ���ǵ� ����
    public void SetSpeed(float _speed)
    {
        speed = _speed;
    }

    // �� ü�� ����
    public void SetHP(int wave)
    {
        maxHP = baseHP + baseHP * wave;
        nowHP = maxHP;
    }

    // EnemyManager�� ���� �Լ�
    public void SetEnemyManager(EnemyManager _enemyManager)
    {
        enemyManager = _enemyManager;
    }

    // �÷��̾�κ��� ���� ������ ó��
    public void GetDamage(float _attack)
    {
        // �⺻ ���ݷ¿��� 0.6 ~ 1.6���� �������� ó��
        float _rate = Random.Range(0.6f, 1.6f);
        float damage = _attack * _rate;

        // ü�� ó��
        nowHP -= damage;
        if(nowHP <= 0)
        {
            SetDeath();
        }
    }

    // ���� ���
    void SetDeath()
    {
        // ����, �Ϲ� ���� ���� ó��
        if(isBoss)
        {
            // 50���, 2���̾� ����
            GameManager.instance.AddNumOfGold(50);
            GameManager.instance.AddNumOfDia(2);

            AIManager.instance.AddNumOfGold(50);
            AIManager.instance.AddNumOfDia(2);
        }
        else
        {
            // 3��� ����
            GameManager.instance.AddNumOfGold(3);

            AIManager.instance.AddNumOfGold(3);
        }
        
        // �� ī��Ʈ �� 1���� �� ������Ʈ ��Ȱ��ȭ
        GameManager.instance.AddEnemyCount(-1);
        gameObject.SetActive(false);
    }

    // �� ���� �ִϸ��̼�
    IEnumerator ScaleControl()
    {
        // �� �ݺ� �Ͽ� �����ų ������
        float diffX = monsterScale.x / changeAmount;
        float diffY = monsterScale.y / changeAmount;

        float count = 0f;

        // �ִϸ��̼� �ð����� ������ �����ϸ� �ִϸ��̼� ����
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
