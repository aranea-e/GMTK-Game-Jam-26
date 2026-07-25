using UnityEngine;
using System.Collections.Generic;

public class GuardSpawner : MonoBehaviour {
    List<Room> guardSpawnRooms;
    List<Room> targetSpawnRooms;
    public GameObject guardPrefab;
    public GameObject targetPrefab;

    public int minGuards;
    public int maxGuards;

    void Awake()
    {
        guardSpawnRooms = new List<Room>();
        targetSpawnRooms = new List<Room>();
    }

    void Start()
    {
        foreach (Room room in GetComponentsInChildren<Room>())
        {
            if (room.spawnGuards)
            {
                guardSpawnRooms.Add(room);
            }
            if (room.spawnTarget)
            {
                targetSpawnRooms.Add(room);
            }
        }

        int guardsToSpawn = Random.Range(minGuards, maxGuards + 1);
        for (int i = 0; i < guardsToSpawn; i++)
        {
            Room room = guardSpawnRooms[Random.Range(0, guardSpawnRooms.Count)];
            
            Vector2 spawnPosition = new Vector2(
                room.transform.position.x + Random.Range(-room.roomSize.x / 3, room.roomSize.x / 3),
                room.transform.position.y + Random.Range(-room.roomSize.y / 3, room.roomSize.y / 3)
            );
            Instantiate(guardPrefab, spawnPosition, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        }

        Room targetRoom = targetSpawnRooms[Random.Range(0, targetSpawnRooms.Count)];
        Vector2 targetSpawnPosition = new Vector2(
            targetRoom.transform.position.x + Random.Range(-targetRoom.roomSize.x / 3, targetRoom.roomSize.x / 3),
            targetRoom.transform.position.y + Random.Range(-targetRoom.roomSize.y / 3, targetRoom.roomSize.y / 3)
        );
        Instantiate(targetPrefab, targetSpawnPosition, Quaternion.Euler(0, 0, Random.Range(0, 360)));
    }
}
