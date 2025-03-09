using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GambleControl : MonoBehaviour
{
    // 도박 후 소환에 사용하기 위해 characterManager연동
    [SerializeField]
    CharacterManager characterManager;

    // 현재 다이아 수 표기할 텍스트
    [SerializeField]
    TextMeshProUGUI numOfDia;

    // 도박 가격 표기 텍스트 1
    [SerializeField]
    TextMeshProUGUI gamblePriceText1;

    // 도박 가격 표기 텍스트 2
    [SerializeField]
    TextMeshProUGUI gamblePriceText2;

    // 도박 결과 표기 텍스트
    [SerializeField]
    List<TextMeshProUGUI> resultTxt;

    // 도박 창이 활성화 되면 초기 UI로 업데이트
    private void OnEnable()
    {
        for(int i=0; i<resultTxt.Count; i++)
        {
            resultTxt[i].enabled = false;
        }
        UI_Upgrade();
    }

    // 변화되는 UI정보 업데이트
    private void Update()
    {
        UI_Upgrade();
    }

    void UI_Upgrade()
    {
        // 0번 도박 현재 가능여부 받아서 가능하면 하얀글씨, 불가능하면 빨간글씨
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

        // 현 다이아 정보 업데이트
        numOfDia.text = GameManager.instance.GetNumOfDia().ToString();
    }

    // 성공 실패 계산
    public bool GetSuccessFail(int num)
    {
        // 확률에 따라 리스트 생성
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

        // 전체 가중치 값의 합 계산
        float totalWeight = 0f;
        foreach (var obj in _list)
        {
            totalWeight += obj.weight;
        }

        // 0부터 가중치합 사이의 수를 랜덤발생
        float randomValue = UnityEngine.Random.value * totalWeight;

        // 발생한 수에서 가중치들을 빼면서 어느 구간인지 판단
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

    // 도박 버튼 누를 경우 실행
    public void GambleSpawn(int num)
    {
        // 도박이 가능할 경우에만 작동
         if(GameManager.instance.CanGamble(num))
        {
            // 현재 도박 가격 값 가져오기
            int price = 0;
            if (num == 0)
            {
                price = int.Parse(gamblePriceText1.text);
            }
            else
            {
                price = int.Parse(gamblePriceText2.text);
            }

            // 도박 가격만큼 다이아 소모
            GameManager.instance.AddNumOfDia(-1 * price);

            // 가중치 랜덤함수를 통해 성공이면
            if (GetSuccessFail(num))
            {
                // 성공 텍스트 띄운 후 해당 캐릭터 소환
                resultTxt[num].text = "성공";
                resultTxt[num].enabled = true;
                characterManager.SetCharacterBase(characterManager.GetFromCharacterPool(characterManager.GetHero(num + 1, UnityEngine.Random.Range(0, 2))), 1);
            }
            // 실패일 경우 실패 문구 띄우기
            else
            {
                resultTxt[num].text = "실패";
                resultTxt[num].enabled = true;
            }

            UI_Upgrade();
        }

    }
}
