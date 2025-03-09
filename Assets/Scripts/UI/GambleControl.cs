using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GambleControl : MonoBehaviour
{
    [SerializeField]
    CharacterManager characterManager;

    [SerializeField]
    TextMeshProUGUI numOfDia;

    [SerializeField]
    TextMeshProUGUI gamblePriceText1;

    [SerializeField]
    TextMeshProUGUI gamblePriceText2;

    [SerializeField]
    List<TextMeshProUGUI> resultTxt;

    private void OnEnable()
    {
        for(int i=0; i<resultTxt.Count; i++)
        {
            resultTxt[i].enabled = false;
        }
        UI_Upgrade();
    }

    private void Update()
    {
        UI_Upgrade();
    }

    void UI_Upgrade()
    {
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

        numOfDia.text = GameManager.instance.GetNumOfDia().ToString();
    }

    public bool GetSuccessFail(int num)
    {
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

        float totalWeight = 0f;
        foreach (var obj in _list)
        {
            totalWeight += obj.weight;
        }

        float randomValue = UnityEngine.Random.value * totalWeight;

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

    public void GambleSpawn(int num)
    {
         if(GameManager.instance.CanGamble(num))
        {
            int price = 0;
            if (num == 0)
            {
                price = int.Parse(gamblePriceText1.text);
            }
            else
            {
                price = int.Parse(gamblePriceText2.text);
            }

            GameManager.instance.AddNumOfDia(-1 * price);

            if (GetSuccessFail(num))
            {
                resultTxt[num].text = "성공";
                resultTxt[num].enabled = true;
                characterManager.SetCharacterBase(characterManager.GetFromCharacterPool(characterManager.GetHero(num + 1, UnityEngine.Random.Range(0, 2))), 1);
            }
            else
            {
                resultTxt[num].text = "실패";
                resultTxt[num].enabled = true;
            }

            UI_Upgrade();
        }

    }
}
