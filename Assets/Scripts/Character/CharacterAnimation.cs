using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    CharacterControl characterControl;

    private void OnEnable()
    {
        if(characterControl == null)
        {
            characterControl = transform.parent.GetComponent<CharacterControl>();
        }
    }

    public void Attack()
    {
        characterControl.Attack();
    }
}
