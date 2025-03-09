using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MythicalControl : MonoBehaviour
{
    [SerializeField]
    CharacterManager characterManager;

    [SerializeField]
    List<GameObject> pages;

    [SerializeField]
    List<string> conditions;

    [SerializeField]
    List<Image> checker;

    bool readyOK;

    private void OnEnable()
    {
        OpenPage(0);
    }

    public void OpenPage(int num)
    {
        for(int i=0; i<pages.Count; i++)
        {
            if(i == num)
            {
                pages[i].SetActive(true);
                checker[0].enabled = characterManager.SearchCharacter(conditions[num].Split(",")[0], 1);
                checker[1].enabled = characterManager.SearchCharacter(conditions[num].Split(",")[1], 1);
                checker[2].enabled = characterManager.SearchCharacter(conditions[num].Split(",")[2], 1);

                readyOK = (checker[0].enabled && checker[1].enabled) && checker[2].enabled;
            }
            else
            {
                pages[i].SetActive(false);
            }
        }
    }

    public void MythicalSpawn(int num)
    {
        if(readyOK)
        {
            CharacterControl _mythical = characterManager.GetHero(3, num);
            _mythical = characterManager.GetFromCharacterPool(_mythical);

            characterManager.RemoveCharacter(conditions[num].Split(",")[0], 1);
            characterManager.RemoveCharacter(conditions[num].Split(",")[1], 1);
            characterManager.RemoveCharacter(conditions[num].Split(",")[2], 1);

            characterManager.SetCharacterBase(_mythical, 1);
        }
    }
}
