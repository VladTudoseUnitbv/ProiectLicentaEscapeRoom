using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameSpace_Generator : MonoBehaviour
{
    [SerializeField] Room roomPrefab;
    [SerializeField] Transform roomHolder;
    [SerializeField] GameObject tunnelPrefab;
    float tunnelLength = 15;
    List<Room> roomsToGenerateFrom = new List<Room>();
    int roomsGenerated = 0;
    float previousRoomSize = 0;
    float housePosX = 0;
    public void GenerateGameSpace(int numberOfRooms)
    {
        while (roomsGenerated < numberOfRooms)
            CreateRoom();
    }

    private void CreateRoom()
    {
        Room newRoom;
        float roomSize;
        float roomHeight;
        housePosX += previousRoomSize / 2;
        roomSize = Random.Range(10, 40);
        roomHeight = Random.Range(5, 10);
        newRoom = Instantiate(roomPrefab.gameObject, roomHolder).GetComponent<Room>();
        Room.DoorState overrideDoorState = 0;
        if (roomsGenerated == 0 || roomsGenerated == 29)
            overrideDoorState = roomsGenerated == 0 ? Room.DoorState.LEFT : Room.DoorState.RIGHT;
        newRoom.Generate(roomSize, roomSize, roomHeight, overrideDoorState);
        if (roomsGenerated > 0)
        {
            housePosX += roomSize / 2;
            housePosX += 2;
            housePosX += tunnelLength;
            GameObject newTunnel = Instantiate(tunnelPrefab, roomHolder);
            newTunnel.transform.position = new Vector3(housePosX - (roomSize + 2) / 2 - tunnelLength / 2, 0f, 0f);
        }
        newRoom.transform.position = new Vector3(roomsGenerated > 0 ? housePosX : 0, 0, 0);
        previousRoomSize = roomSize;
        roomsToGenerateFrom.Add(newRoom);
        roomsGenerated++;
    }

    private void Start()
    {
        GenerateGameSpace(30);
    }
}

public class Matrix
{
    private int rows;
    private int columns;
    private Room[,] rooms;
    public Room[,] Rooms { get { return rooms; } set { rooms = value; } }

    public Matrix(int rows, int columns)
    {
        this.rows = rows;
        this.columns = columns;
    }
}
