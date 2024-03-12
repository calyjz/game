﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class RoomTileMapManager : MonoBehaviour
{
    [SerializeField] GameObject roomPrefab;
    [SerializeField] private int maxRooms = 15;
    [SerializeField] private int minRooms = 10;

    private int roomWidth = 24;
    private int roomHeight = 17;

    private int gridSizeX = 10;
    private int gridSizeY = 10;

    private List<GameObject> roomObjects = new List<GameObject>();

    private Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();

    private bool generationComplete = false;

    private int[,] roomGrid;

    private int roomCount;

    private void Start()
    {
        roomGrid = new int[gridSizeX, gridSizeY];
        roomQueue = new Queue<Vector2Int>();

        Vector2Int initialRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);
    }

    private void Update()
    {
        if (roomQueue.Count > 0 && roomCount < maxRooms && !generationComplete)
        {
            Vector2Int roomIndex = roomQueue.Dequeue();
            int gridX = roomIndex.x;
            int gridY = roomIndex.y;
            TryGenerateRoom(new Vector2Int(gridX - 1, gridY));
            TryGenerateRoom(new Vector2Int(gridX, gridY - 1));
            TryGenerateRoom(new Vector2Int(gridX + 1, gridY));
            TryGenerateRoom(new Vector2Int(gridX, gridY + 1));
        } else if (roomCount < minRooms)
        {
            Debug.Log("Rooms were less than min amount");
            RegenerateRooms();
        } 
        else if (!generationComplete)
        {
            Debug.Log($"Generation complete, {roomCount} rooms created");
            generationComplete = true;
        }
    }

    private void StartRoomGenerationFromRoom(Vector2Int roomIndex)
    {
        roomQueue.Enqueue(roomIndex);
        int x = roomIndex.x;
        int y = roomIndex.y;
        roomGrid[x, y] = 1;
        roomCount++;
        Debug.Log("Room Generation");
        var initialRoom = Instantiate(roomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        initialRoom.name = $"Room-{roomCount}";
        initialRoom.GetComponent<Room>().RoomIndex = roomIndex;
        roomObjects.Add(initialRoom);
    }

    private bool TryGenerateRoom(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;

        if (roomCount >= maxRooms)
        {
            return false;
        }

        if (Random.value < 0.5f && roomIndex != Vector2Int.zero)
        {
            return false;
        }

        if (CountAdjacentRooms(roomIndex) > 1)
        {
            return false;
        }

        roomQueue.Enqueue(roomIndex);
        roomGrid[x, y] = 1;
        roomCount++;

        var newRoom = Instantiate(roomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        newRoom.GetComponent<Room>().RoomIndex = roomIndex;
        newRoom.name = $"Room-{roomCount}";
        roomObjects.Add(newRoom);

        OpenDoors(newRoom, x, y);

        return true;
    }

    private void RegenerateRooms()
    {
        roomObjects.ForEach(Destroy);
        roomObjects.Clear();
        roomGrid = new int[gridSizeX, gridSizeY];
        roomQueue.Clear();
        roomCount = 0;
        generationComplete = false;

        Vector2Int initialRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);
    }

    void OpenDoors(GameObject room, int x, int y)
    {
        Room newRoomScript = room.GetComponent<Room>();

        //Neighbors
        Room leftRoomScript = GetRoomScriptAt(new Vector2Int(x - 1, y));
        Room rightRoomScript = GetRoomScriptAt(new Vector2Int(x + 1, y));
        Room bottomRoomScript = GetRoomScriptAt(new Vector2Int(x, y - 1));
        Room topRoomScript = GetRoomScriptAt(new Vector2Int(x, y + 1));

        //Determine which doors to open
        if (x > 0 && roomGrid[x - 1, y] != 0)
        {
            newRoomScript.OpenDoor(Vector2Int.left);
            leftRoomScript.OpenDoor(Vector2Int.right);
        }
        if (x < gridSizeX - 1 && roomGrid[x + 1, y] != 0)
        {
            newRoomScript.OpenDoor(Vector2Int.right);
            rightRoomScript.OpenDoor(Vector2Int.left);
        }
        if (y > 0 && roomGrid[x, y - 1] != 0)
        {
            newRoomScript.OpenDoor(Vector2Int.down);
            bottomRoomScript.OpenDoor(Vector2Int.up);
        }
        if (y < gridSizeY - 1 && roomGrid[x, y + 1] != 0)
        {
            newRoomScript.OpenDoor(Vector2Int.up);
            topRoomScript.OpenDoor(Vector2Int.down);
        }

    }

    Room GetRoomScriptAt(Vector2Int index)
    {
        GameObject roomObject = roomObjects.Find(r => r.GetComponent<Room>().RoomIndex == index);
        if(roomObject != null)
        {
            return roomObject.GetComponent<Room>();
        }
        return null;
    }

    private int CountAdjacentRooms(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;
        int count = 0;

        if (x > 0 && roomGrid[x - 1, y] != 0) count++; //left neighbor
        if (x < gridSizeX - 1 && roomGrid[x + 1, y] != 0) count++; //right neighbor
        if (y > 0 && roomGrid[x, y - 1] != 0) count++; // Bottom neighbor
        if (y < gridSizeY - 1 && roomGrid[x, y + 1] != 0) count++; // Top neighbor

        return count;
    }

    private Vector3 GetPositionFromGridIndex(Vector2Int gridIndex)
    {
        int gridX = gridIndex.x;
        int gridY = gridIndex.y;
        return new Vector3(roomWidth * (gridX - gridSizeX / 2), roomHeight * (gridY - gridSizeY / 2));
    }

    public Room GetCurrentRoom(Vector3 position)
    {
        Vector2Int gridIndex = new Vector2Int(
            Mathf.FloorToInt((position.x + roomWidth * gridSizeX / 2) / roomWidth),
            Mathf.FloorToInt((position.y + roomHeight * gridSizeY / 2) / roomHeight)
        );

        return GetRoomScriptAt(gridIndex);
    }

    public int getRoomWidth()
    {
        return roomWidth;
    }

    public int getRoomHeight()
    {
        return roomHeight;
    }
}
