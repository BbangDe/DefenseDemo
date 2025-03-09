using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GambleControl : MonoBehaviour
{
    // ���� �� ��ȯ�� ����ϱ� ���� characterManager����
    [SerializeField]
    CharacterManager characterManager;

    // ���� ���̾� �� ǥ���� �ؽ�Ʈ
    [SerializeField]
    TextMeshProUGUI numOfDia;

    // ���� ���� ǥ�� �ؽ�Ʈ 1
    [SerializeField]
    TextMeshProUGUI gamblePriceText1;

    // ���� ���� ǥ�� �ؽ�Ʈ 2
    [SerializeField]
    TextMeshProUGUI gamblePriceText2;

    // ���� ��� ǥ�� �ؽ�Ʈ
    [SerializeField]
    List<TextMeshProUGUI> resultTxt;

    // ���� â�� Ȱ��ȭ �Ǹ� �ʱ� UI�� ������Ʈ
    private void OnEnable()
    {
        for(int i=0; i<resultTxt.Count; i++)
        {
            resultTxt[i].enabled = false;
        }
        UI_Upgrade();
    }

    // ��ȭ�Ǵ� UI���� ������Ʈ
    private void Update()
    {
        UI_Upgrade();
    }

    void UI_Upgrade()
    {
        // 0�� ���� ���� ���ɿ��� �޾Ƽ� �����ϸ� �Ͼ�۾�, �Ұ����ϸ� �����۾�
        if(GameManager.instance.CanGamble(0))
        {
            gamblePriceText1.color = Color.white;
        }
        else
        {
            gamblePriceText1.color = Color.red;
        }

        if (GameManager.instance.CanGamble(1))
        {
            gamblePriceText2.color = Color.white;
        }
        else
        {
            gamblePriceText2.color = Color.red;
        }

        // �� ���̾� ���� ������Ʈ
        numOfDia.text = GameManager.instance.GetNumOfDia().ToString();
    }

    // ���� ���� ���
    public bool GetSuccessFail(int num)
    {
        // Ȯ���� ���� ����Ʈ ����
        List<(bool result, float weight)> _list = new List<(bool result, float weight)>();
        if(num == 0)
        {
            _list.Add((true, 60f));
            _list.Add((false, 40f));
        }
        else
        {
            _list.Add((true, 20f));
            _list.Add((false, 80f));
        }

        // ��ü ����ġ ���� �� ���
        float totalWeight = 0f;
        foreach (var obj in _list)
        {
            totalWeight += obj.weight;
        }

        // 0���� ����ġ�� ������ ���� �����߻�
        float randomValue = UnityEngine.Random.value * totalWeight;

        // �߻��� ������ ����ġ���� ���鼭 ��� �������� �Ǵ�
        bool pick = false;
        foreach (var obj in _list)
        {
            randomValue -= obj.weight;
            if (randomValue <= 0)
            {
                pick = obj.result;
                break;
            }
        }

        return pick;
    }

    // ���� ��ư ���� ��� ����
    public void GambleSpawn(int num)
    {
        // ������ ������ ��쿡�� �۵�
         if(GameManager.instance.CanGamble(num))
        {
            // ���� ���� ���� �� ��������
            int price = 0;
            if (num == 0)
            {
                price = int.Parse(gamblePriceText1.text);
            }
            else
            {
                price = int.Parse(gamblePriceText2.text);
            }

            // ���� ���ݸ�ŭ ���̾� �Ҹ�
            GameManager.instance.AddNumOfDia(-1 * price);

            // ����ġ �����Լ��� ���� �����̸�
            if (GetSuccessFail(num))
            {
                // ���� �ؽ�Ʈ ��� �� �ش� ĳ���� ��ȯ
                resultTxt[num].text = "����";
                resultTxt[num].enabled = true;
                characterManager.SetCharacterBase(characterManager.GetFromCharacterPool(characterManager.GetHero(num + 1, UnityEngine.Random.Range(0, 2))), 1);
            }
            // ������ ��� ���� ���� ����
            else
            {
                resultTxt[num].text = "����";
                resultTxt[num].enabled = true;
            }

            UI_Upgrade();
        }

    }
}
