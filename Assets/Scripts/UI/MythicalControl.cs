using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MythicalControl : MonoBehaviour
{
    // characterManager 연동
    [SerializeField]
    CharacterManager characterManager;

    // 현재 보고있는 페이지 리스트
    [SerializeField]
    List<GameObject> pages;

    // 해당 리스트의 소환이 성립하기 위한 필요 유닛들 정보
    [SerializeField]
    List<string> conditions;

    // 체크표시 오브젝트 리스트
    [SerializeField]
    List<Image> checker;

    // 소환 가능 여부
    bool readyOK;

    private void OnEnable()
    {
        // 신화 창 오픈 시 0번 페이지 오픈
        OpenPage(0);
    }

    // 선택한 페이지 여는 함수
    public void OpenPage(int num)
    {
        // 넘겨받은 페이지와 같은 페이지라면
        for(int i=0; i<pages.Count; i++)
        {
            if(i == num)
            {
                // 해당 페이지를 활성화 시키고
                pages[i].SetActive(true);
                // 소환 조건별로 존재 하는지 모두 체크하여 있는 경우만 체크표시 띄우기)
                checker[0].enabled = characterManager.SearchCharacter(conditions[num].Split(",")[0], 1);
                checker[1].enabled = characterManager.SearchCharacter(conditions[num].Split(",")[1], 1);
                checker[2].enabled = characterManager.SearchCharacter(conditions[num].Split(",")[2], 1);

                // 검색결과에 따른 소환가능여부 저장
                readyOK = (checker[0].enabled && checker[1].enabled) && checker[2].enabled;
            }
            // 다른페이지면 비활성화
            else
            {
                pages[i].SetActive(false);
            }
        }
    }

    // 신화소환 선택 시
    public void MythicalSpawn(int num)
    {
        // 소환 가능한 경우라면
        if(readyOK)
        {
            // 신화 유닛을 가져와서 소환
            CharacterControl _mythical = characterManager.GetHero(3, num);
            _mythical = characterManager.GetFromCharacterPool(_mythical);

            // 재료 유닛들은 제거
            characterManager.RemoveCharacter(conditions[num].Split(",")[0], 1);
            characterManager.RemoveCharacter(conditions[num].Split(",")[1], 1);
            characterManager.RemoveCharacter(conditions[num].Split(",")[2], 1);

            //신화 유닛 배치
            characterManager.SetCharacterBase(_mythical, 1);
        }
    }
}
