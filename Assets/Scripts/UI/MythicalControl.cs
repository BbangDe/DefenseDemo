using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MythicalControl : MonoBehaviour
{
    // characterManager ����
    [SerializeField]
    CharacterManager characterManager;

    // ���� �����ִ� ������ ����Ʈ
    [SerializeField]
    List<GameObject> pages;

    // �ش� ����Ʈ�� ��ȯ�� �����ϱ� ���� �ʿ� ���ֵ� ����
    [SerializeField]
    List<string> conditions;

    // üũǥ�� ������Ʈ ����Ʈ
    [SerializeField]
    List<Image> checker;

    // ��ȯ ���� ����
    bool readyOK;

    private void OnEnable()
    {
        // ��ȭ â ���� �� 0�� ������ ����
        OpenPage(0);
    }

    // ������ ������ ���� �Լ�
    public void OpenPage(int num)
    {
        // �Ѱܹ��� �������� ���� ���������
        for(int i=0; i<pages.Count; i++)
        {
            if(i == num)
            {
                // �ش� �������� Ȱ��ȭ ��Ű��
                pages[i].SetActive(true);
                // ��ȯ ���Ǻ��� ���� �ϴ��� ��� üũ�Ͽ� �ִ� ��츸 üũǥ�� ����)
                checker[0].enabled = characterManager.SearchCharacter(conditions[num].Split(",")[0], 1);
                checker[1].enabled = characterManager.SearchCharacter(conditions[num].Split(",")[1], 1);
                checker[2].enabled = characterManager.SearchCharacter(conditions[num].Split(",")[2], 1);

                // �˻������ ���� ��ȯ���ɿ��� ����
                readyOK = (checker[0].enabled && checker[1].enabled) && checker[2].enabled;
            }
            // �ٸ��������� ��Ȱ��ȭ
            else
            {
                pages[i].SetActive(false);
            }
        }
    }

    // ��ȭ��ȯ ���� ��
    public void MythicalSpawn(int num)
    {
        // ��ȯ ������ �����
        if(readyOK)
        {
            // ��ȭ ������ �����ͼ� ��ȯ
            CharacterControl _mythical = characterManager.GetHero(3, num);
            _mythical = characterManager.GetFromCharacterPool(_mythical);

            // ��� ���ֵ��� ����
            characterManager.RemoveCharacter(conditions[num].Split(",")[0], 1);
            characterManager.RemoveCharacter(conditions[num].Split(",")[1], 1);
            characterManager.RemoveCharacter(conditions[num].Split(",")[2], 1);

            //��ȭ ���� ��ġ
            characterManager.SetCharacterBase(_mythical, 1);
        }
    }
}
