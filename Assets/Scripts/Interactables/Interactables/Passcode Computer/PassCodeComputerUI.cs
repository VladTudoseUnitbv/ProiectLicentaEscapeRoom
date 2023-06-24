using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;

public class PassCodeComputerUI : MonoBehaviour
{
    [SerializeField] TMP_InputField passCodeInput;
    [SerializeField] InteractableObject transporterRef;
    char[] passcodeChars = new char[3];
    [SerializeField] TMP_Text[] barrelTexts;
    [SerializeField] string[] roomNames;
    [SerializeField] TMP_Text wrongPassText;
    [SerializeField] Animator separatorAnimator;
    [SerializeField] PhotonView photonView;
    int attempts = 3;

    private void Start()
    {
        passcodeChars[0] = 'B';
        passcodeChars[1] = 'S';
        passcodeChars[2] = 'E';
        for (int i = 0; i < 3; i++)
        {
            //passcodeChars[i] = (char)('A' + Random.Range(0, 26));
            barrelTexts[i].text = $"Barrel {i+1}\n{roomNames[i]}\nSector {passcodeChars[i]}";
        }
        gameObject.SetActive(false);
    }
    public void Init()
    {
        passCodeInput.text = string.Empty;
        wrongPassText.gameObject.SetActive(false);
        attempts = 3;
    }

    public void SubmitPasscode()
    {
        string inputText = passCodeInput.text.ToUpper();
        for (int i = 0; i < 3; i++)
        {
            if (inputText[i] != passcodeChars[i])
            {
                WrongPass();
                return;
            }
        }
        CorrectPass();
    }

    private void WrongPass()
    {
        if (attempts > 0)
        {
            wrongPassText.text = $"Wrong password. {attempts} attempts left.";
            wrongPassText.gameObject.SetActive(true);
            passCodeInput.text = string.Empty;
            attempts--;
        }
        else
        {
            PopupManager.Instance.TogglePasscodeComputerPopup();
            //Increase threath level
        }
    }

    private void CorrectPass()
    {
        PopupManager.Instance.TogglePasscodeComputerPopup();
        photonView.RPC("UnlockTransporter", RpcTarget.All);
    }

    [PunRPC]
    private void UnlockTransporter()
    {
        transporterRef.InteractRange = 1.5f;
        separatorAnimator.SetTrigger("Open");
    }
}
