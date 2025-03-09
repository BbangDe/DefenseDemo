using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    // characterControl 연동
    CharacterControl characterControl;

    // 연동된 characterControl이 없으면 연동
    private void OnEnable()
    {
        if(characterControl == null)
        {
            characterControl = transform.parent.GetComponent<CharacterControl>();
        }
    }

    // 애니메이션 공격 클립 함수
    public void Attack()
    {
        characterControl.Attack();
    }
}
