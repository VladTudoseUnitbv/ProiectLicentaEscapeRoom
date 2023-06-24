using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PasscodeComputer", menuName = "ScriptableObjects/Interactables/PasscodeComputer")]
public class PasscodeComputer : Interactable
{
    public override void Interact(object param)
    {
        PopupManager.Instance.TogglePasscodeComputerPopup();
    }
}
