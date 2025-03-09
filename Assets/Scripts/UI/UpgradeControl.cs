using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeControl : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI upgradePriceText1;
    [SerializeField]
    TextMeshProUGUI upgradePriceText2;
    [SerializeField]
    TextMeshProUGUI upgradePriceText3;
    [SerializeField]
    TextMeshProUGUI upgradePriceText4;

    [SerializeField]
    TextMeshProUGUI upgradeLevelText1;
    [SerializeField]
    TextMeshProUGUI upgradeLevelText2;
    [SerializeField]
    TextMeshProUGUI upgradeLevelText3;
    [SerializeField]
    TextMeshProUGUI upgradeLevelText4;

    [SerializeField]
    TextMeshProUGUI goldTxt;
    [SerializeField]
    TextMeshProUGUI diaTxt;

    private void Update()
    {
        UI_Upgrade();
    }

    public void UpgradeButton(int grade)
    {
        if(GameManager.instance.CanUpgrade(grade))
        {
            GameManager.instance.SetUpgrade(grade);
            UI_Upgrade();
        }
    }

    void UI_Upgrade()
    {
        if (GameManager.instance.CanUpgrade(0))
        {
            upgradePriceText1.color = Color.white;
        }
        else
        {
            upgradePriceText1.color = Color.red;
        }

        if (GameManager.instance.CanUpgrade(1))
        {
            upgradePriceText2.color = Color.white;
        }
        else
        {
            upgradePriceText2.color = Color.red;
        }

        if (GameManager.instance.CanUpgrade(2))
        {
            upgradePriceText3.color = Color.white;
        }
        else
        {
            upgradePriceText3.color = Color.red;
        }

        if (GameManager.instance.CanUpgrade(3))
        {
            upgradePriceText4.color = Color.white;
        }
        else
        {
            upgradePriceText4.color = Color.red;
        }

        upgradePriceText1.text = GameManager.instance.GetUpgradePrice(0).ToString();
        upgradePriceText2.text = GameManager.instance.GetUpgradePrice(1).ToString();
        upgradePriceText3.text = GameManager.instance.GetUpgradePrice(2).ToString();
        upgradePriceText4.text = GameManager.instance.GetUpgradePrice(3).ToString();

        upgradeLevelText1.text = $"Lv.{GameManager.instance.GetUpgradeLevel(0)}";
        upgradeLevelText2.text = $"Lv.{GameManager.instance.GetUpgradeLevel(1)}";
        upgradeLevelText3.text = $"Lv.{GameManager.instance.GetUpgradeLevel(2)}";
        upgradeLevelText4.text = $"Lv.{GameManager.instance.GetUpgradeLevel(3)}";

        goldTxt.text = GameManager.instance.GetNumOfGold().ToString();
        diaTxt.text = GameManager.instance.GetNumOfDia().ToString();
    }
}
