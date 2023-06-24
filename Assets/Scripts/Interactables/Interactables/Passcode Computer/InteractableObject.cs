using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] PlayerSpawner playerSpawner;
    [SerializeField] Transform player;
    [SerializeField] GameObject interactDisplay;
    [SerializeField] Interactable onInteract;
    [SerializeField] InteractableType type;
    [SerializeField] float interactRange;
    [SerializeField] List<Transform> destionations = new List<Transform>();
    bool isInRange;

    public float InteractRange { get { return interactRange; } set { interactRange = value; } }


    void Update()
    {
        if (playerSpawner.localPlayer == null) return;
        isInRange = Vector3.Distance(transform.position, playerSpawner.localPlayer.transform.position) <= interactRange;
        //isInRange = Vector3.Distance(transform.position, player.position) <= interactRange;
        interactDisplay.SetActive(isInRange);
    }

    public void AttemptInteract()
    {
        if(isInRange)
        {
            switch (type)
            {
                case InteractableType.Other:
                    onInteract.Interact(null);
                    break;
                case InteractableType.Transporter:
                    onInteract.Interact(destionations);
                    break;
            }
        }
    }

    public enum InteractableType { 
        Transporter,
        Other
    }

}
