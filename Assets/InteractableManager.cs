using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    [SerializeField] List<InteractableObject> interactableObjects = new List<InteractableObject>();

    public void OnClickInteract()
    {
        foreach(InteractableObject interactableObject in interactableObjects)
        {
            interactableObject.AttemptInteract();
        }
    }
}
