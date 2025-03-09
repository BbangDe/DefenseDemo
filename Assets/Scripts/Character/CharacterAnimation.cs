using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    // characterControl ����
    CharacterControl characterControl;

    // ������ characterControl�� ������ ����
    private void OnEnable()
    {
        if(characterControl == null)
        {
            characterControl = transform.parent.GetComponent<CharacterControl>();
        }
    }

    // �ִϸ��̼� ���� Ŭ�� �Լ�
    public void Attack()
    {
        characterControl.Attack();
    }
}
