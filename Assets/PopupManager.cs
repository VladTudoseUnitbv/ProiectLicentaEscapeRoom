using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    #region Singleton
    public static PopupManager Instance;

    private void Awake()
    {

        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    #endregion

    [SerializeField] PlayerSpawner playerSpawner;
    [SerializeField] PassCodeComputerUI passcodeComputerPopup;
    [SerializeField] LavaMapManager lavaMapManager;
    [HideInInspector] public GameObject activePanel;

    public PlayerSpawner PlayerSpawner => playerSpawner;
    public LavaMapManager LavaMapManager => lavaMapManager;

    public void TogglePasscodeComputerPopup()
    {
        passcodeComputerPopup.gameObject.SetActive(!passcodeComputerPopup.gameObject.activeSelf);
        if (passcodeComputerPopup.gameObject.activeSelf)
        {
            activePanel = passcodeComputerPopup.gameObject;
            passcodeComputerPopup.Init();
        }
        else
            activePanel = null;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(activePanel != null)
            {
                activePanel.SetActive(false);
                activePanel = null;
            }
        }
    }
}
