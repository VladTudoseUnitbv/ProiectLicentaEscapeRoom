using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{
    public float rotationSpeed = 0.35f; // Adjust this value to control the rotation speed

    private void Update()
    {
        // Calculate the rotation amount based on time and speed
        float rotationAmount = Time.deltaTime * rotationSpeed;

        // Create a rotation vector for rotation
        Vector3 rotationVector = new Vector3(0, rotationAmount, 0);

        // Rotate the skybox 
        RenderSettings.skybox.SetFloat("_Rotation", RenderSettings.skybox.GetFloat("_Rotation") + rotationAmount);
    }
}
