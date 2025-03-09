using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeControl : MonoBehaviour
{
    // 각 업그레이드 별 금액 변수
    [SerializeField]
    TextMeshProUGUI upgradePriceText1;
    [SerializeField]
    TextMeshProUGUI upgradePriceText2;
    [SerializeField]
    TextMeshProUGUI upgradePriceText3;
    [SerializeField]
    TextMeshProUGUI upgradePriceText4;

    // 각 업그레이드 별 현재 레벨 변수
    [SerializeField]
    TextMeshProUGUI upgradeLevelText1;
    [SerializeField]
    TextMeshProUGUI upgradeLevelText2;
    [SerializeField]
    TextMeshProUGUI upgradeLevelText3;
    [SerializeField]
    TextMeshProUGUI upgradeLevelText4;

    // 현재 골드, 다이아 정보 표시 변수
    [SerializeField]
    TextMeshProUGUI goldTxt;
    [SerializeField]
    TextMeshProUGUI diaTxt;

    private void Update()
    {
        UI_Upgrade();
    }

    // 업그레이드 버튼 선택 시
    public void UpgradeButton(int grade)
    {
        // 업그레이드가 가능한 경우에만 실행
        if(GameManager.instance.CanUpgrade(grade))
        {
            // 업그레이드 함수 실행
            GameManager.instance.SetUpgrade(grade);
            UI_Upgrade();
        }
    }

    // UI 정보 업데이트
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
