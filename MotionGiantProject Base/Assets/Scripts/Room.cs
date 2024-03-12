﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2Int RoomIndex { get; set; }
    public Tilemap tilemap;
    public Tile doorTile;

    public void OpenDoor(Vector2Int direction) 
    {
        Debug.Log("About to Open Door");
        if (direction == Vector2Int.up)
        {
            Debug.Log("Opening Top door");
            openTopDoor();
        } else if (direction == Vector2Int.down)
        {
            Debug.Log("Opening Bottom door");
            openBottomDoor();
        } else if (direction == Vector2Int.left)
        {
            Debug.Log("Opening Left door");
            openLeftDoor();
        } else if (direction == Vector2Int.right)
        {
            Debug.Log("Opening Right door");
            openRightDoor();
        }
    }

    private void openTopDoor()
    {
        int centerX = tilemap.size.x / 2;
        int centerXP1 = (tilemap.size.x / 2) + 1;
        int centerXM1 = (tilemap.size.x / 2) - 1;
        int topY = tilemap.size.y - 3; // The top row of the tilemap
        Vector3Int doorPosition = new Vector3Int(centerX, topY, 0);
        Vector3Int doorPositionP1 = new Vector3Int(centerXP1, topY, 0);
        Vector3Int doorPositionM1 = new Vector3Int(centerXM1, topY, 0);
        tilemap.SetTile(doorPosition, doorTile); // Remove the tile
        tilemap.SetTile(doorPositionP1, doorTile); // Remove the tile
        tilemap.SetTile(doorPositionM1, doorTile); // Remove the tile
    }

    private void openBottomDoor()
    {
        int centerX = tilemap.size.x / 2;
        int centerXP1 = (tilemap.size.x / 2) + 1;
        int centerXM1 = (tilemap.size.x / 2) - 1;
        int bottomY = 0; // The bottom row of the tilemap
        Vector3Int doorPosition = new Vector3Int(centerX, bottomY, 0);
        Vector3Int doorPositionP1 = new Vector3Int(centerXP1, bottomY, 0);
        Vector3Int doorPositionM1 = new Vector3Int(centerXM1, bottomY, 0);
        tilemap.SetTile(doorPosition, doorTile); // Remove the tile
        tilemap.SetTile(doorPositionP1, doorTile); // Remove the tile
        tilemap.SetTile(doorPositionM1, doorTile); // Remove the tile
    }

    private void openLeftDoor()
    {
        int leftX = 0; // The left column of the tilemap
        int centerY = tilemap.size.y / 2;
        int centerYP1 = (tilemap.size.y / 2) + 1;
        int centerYM1 = (tilemap.size.y / 2) - 1;
        Vector3Int doorPosition = new Vector3Int(leftX, centerY, 0);
        Vector3Int doorPositionP1 = new Vector3Int(leftX, centerYP1, 0);
        Vector3Int doorPositionM1 = new Vector3Int(leftX, centerYM1, 0);
        tilemap.SetTile(doorPosition, doorTile); // Remove the tile
        tilemap.SetTile(doorPositionM1, doorTile); // Remove the tile
    }

    private void openRightDoor()
    {
        int rightX = tilemap.size.x - 1; // The right column of the tilemap
        int centerY = tilemap.size.y / 2;
        int centerYP1 = (tilemap.size.y / 2) + 1;
        int centerYM1 = (tilemap.size.y / 2) - 1;
        Vector3Int doorPosition = new Vector3Int(rightX, centerY, 0);
        Vector3Int doorPositionP1 = new Vector3Int(rightX, centerYP1, 0);
        Vector3Int doorPositionM1 = new Vector3Int(rightX, centerYM1, 0);
        tilemap.SetTile(doorPosition, doorTile); // Remove the tile
        tilemap.SetTile(doorPositionM1, doorTile); // Remove the tile
    }

}
