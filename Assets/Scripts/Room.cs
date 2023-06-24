using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Room : MonoBehaviour
{
    [SerializeField] Transform floor;
    [SerializeField] Transform roof;
    [SerializeField] float structureWidth = 1f;
    [SerializeField] float doorHeight = 2.5f;
    [SerializeField] float doorLength = 3.75f;
    [SerializeField] Transform[] fullWalls;
    [SerializeField] Transform[] doorWallsLeft;
    [SerializeField] Transform[] doorWallsRight;
    [SerializeField] Transform[] doorWallsMid;
    [SerializeField] Transform[] doors;
    [SerializeField] GameObject[] doorWallHolders;

    public DoorState doorState = 0;
    bool randomDoors = false;

    public enum DoorState
    {
        LEFT = 1,
        RIGHT = 2,
        UP = 4,
        DOWN = 8
    }

    public void Generate(float width, float length, float height, DoorState overrideDoorState)
    {
        //Adding + 2 to the scale of the floor and roof because the width and length refer to the actual inside - empty space part of the room, so the floor and the roof are larger to compensate
        floor.localScale = new Vector3(width + structureWidth * 2, structureWidth, length + structureWidth * 2);
        roof.localScale = new Vector3(width + structureWidth * 2, structureWidth, length + structureWidth * 2);
        roof.position = new Vector3(0f, height + structureWidth, 0f);


        if (randomDoors)
        {
            for (int i = 0; i < 4; i++)
            {
                if (UnityEngine.Random.Range(0, 2) == 0) continue;
                doorState |= (DoorState)Math.Pow(2, i);
            }
        }
        else doorState = overrideDoorState == 0 ? doorState = DoorState.LEFT | DoorState.RIGHT : overrideDoorState;

        if ((doorState & DoorState.LEFT) != 0)
        {
            GenerateDoorWall(width, length, height, 1);
        }
        else GenerateFullWall(width, length, height, 1);

        if ((doorState & DoorState.RIGHT) != 0)
        {
            GenerateDoorWall(width, length, height, 3);
        }
        else GenerateFullWall(width, length, height, 3);
        if ((doorState & DoorState.UP) != 0)
        {
            GenerateDoorWall(width, length, height, 0);
        }
        else GenerateFullWall(width, length, height, 0);
        if ((doorState & DoorState.DOWN) != 0)
        {
            GenerateDoorWall(width, length, height, 2);
        }
        else GenerateFullWall(width, length, height, 2);
    }

    private void GenerateFullWall(float width, float length, float height, int wallIndex)
    {
        fullWalls[wallIndex].localScale = new Vector3(wallIndex % 2 == 0 ? width : length, structureWidth, height);
        float xPos = wallIndex % 2 == 0 ? 0 : width / 2 + structureWidth / 2;
        float zPos = wallIndex % 2 == 0 ? length / 2 + structureWidth / 2 : 0;
        if (wallIndex == 2)
            zPos *= -1;
        if (wallIndex == 3)
            xPos *= -1;
        fullWalls[wallIndex].position = new Vector3(xPos, height / 2 + structureWidth / 2, zPos);
        float yRot = wallIndex % 2 == 0 ? 0 : 90;
        fullWalls[wallIndex].localRotation = Quaternion.Euler(90f, yRot, 0f);

        fullWalls[wallIndex].gameObject.SetActive(true);
        doorWallHolders[wallIndex].SetActive(false);
    }

    private void GenerateDoorWall(float width, float length, float height, int wallIndex)
    {
        Vector3 wallScale = new Vector3(wallIndex % 2 == 0 ? (width - doorLength) / 2 : (length - doorLength) / 2, structureWidth, height);
        doorWallsLeft[wallIndex].localScale = wallScale;
        doorWallsRight[wallIndex].localScale = wallScale;
        doorWallsMid[wallIndex].localScale = new Vector3(doorLength, structureWidth, height - doorHeight);

        float xPos = wallIndex % 2 == 0 ? 0 : width / 2 + structureWidth / 2;
        float zPos = wallIndex % 2 == 0 ? length / 2 + structureWidth / 2 : 0;
        if (wallIndex == 2)
            zPos *= -1;
        if (wallIndex == 3)
            xPos *= -1;
        Vector3 wallPosition = new Vector3(xPos, height / 2 + structureWidth / 2, zPos);
        doorWallHolders[wallIndex].transform.position = wallPosition;
        Vector3 doorWallPosLength = new Vector3((length / 2 + doorLength / 2) / 2, 0f, 0f);
        Vector3 doorWallPosWidth = new Vector3((width / 2 + doorLength / 2) / 2, 0f, 0f);
        doorWallsLeft[wallIndex].localPosition = wallIndex % 2 == 0 ? doorWallPosWidth : doorWallPosLength;
        doorWallsRight[wallIndex].localPosition = wallIndex % 2 == 0 ? doorWallPosWidth * -1f : doorWallPosLength * -1f;
        doorWallsMid[wallIndex].localPosition = new Vector3(0f, (height - doorWallsMid[wallIndex].transform.localScale.z) / 2, 0f);
        doors[wallIndex].localPosition = new Vector3(0f, -height / 2 + doorHeight / 2);

        float yRot = wallIndex % 2 == 0 ? 0 : 90;
        doorWallHolders[wallIndex].transform.rotation = Quaternion.Euler(0f, yRot, 0f);

        fullWalls[wallIndex].gameObject.SetActive(false);
        doorWallHolders[wallIndex].SetActive(true);
    }
}
