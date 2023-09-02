using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public GameObject[] treePrefabs; // Array of tree prefabs to spawn
    public GameObject[] housePrefabs; // Array of house prefabs to spawn
    public GameObject roadPrefab; // The prefab of the road segment
    public float roadLength = 20f; // The length of each road segment
    public int numberOfSegments = 5; // The number of segments to generate initially
    public float spawnDistance = 100f; // The distance ahead of the player to spawn new segments

    private Transform[] roadSegments; // Array to store the generated road segments

    private void Start()
    {
        roadSegments = new Transform[numberOfSegments];

        // Generate the initial road segments
        for (int i = 0; i < numberOfSegments; i++)
        {
            roadSegments[i] = CreateRoadSegment(i * roadLength);
        }
    }

    private void Update()
    {
        // Check if the player has moved beyond the last road segment and generate new segments accordingly
        float playerPositionX = player.position.x;
        float lastSegmentPositionX = roadSegments[roadSegments.Length - 1].position.x;

        if (playerPositionX > lastSegmentPositionX - spawnDistance)
        {
            MoveAndReuseSegment();
        }
    }

    private Transform CreateRoadSegment(float positionX)
    {
        Vector3 position = new Vector3(positionX, 0f, 0f);
        Transform roadSegment = Instantiate(roadPrefab, position, Quaternion.identity).transform;
        GenerateObstaclesOnSegment(roadSegment);
        return roadSegment;
    }

    private void MoveAndReuseSegment()
    {
        // Move the first segment to the end of the segments array and update its position
        Transform firstSegment = roadSegments[0];
        for (int i = 1; i < numberOfSegments; i++)
        {
            roadSegments[i - 1] = roadSegments[i];
        }

        float newSegmentPositionX = roadSegments[numberOfSegments - 2].position.x + roadLength;
        roadSegments[numberOfSegments - 1] = CreateRoadSegment(newSegmentPositionX);

        // Reposition the first segment at the end
        firstSegment.position = new Vector3(newSegmentPositionX - roadLength, 0f, 0f);
    }

    private void GenerateObstaclesOnSegment(Transform segment)
    {
        // Randomly spawn trees and houses on the road segment
        int numberOfTrees = Random.Range(2, 5);
        int numberOfHouses = Random.Range(1, 3);

        for (int i = 0; i < numberOfTrees; i++)
        {
            SpawnRandomObstacle(segment, treePrefabs);
        }

        for (int i = 0; i < numberOfHouses; i++)
        {
            SpawnRandomObstacle(segment, housePrefabs);
        }
    }

    private void SpawnRandomObstacle(Transform segment, GameObject[] obstaclePrefabs)
    {
        int randomIndex = Random.Range(0, obstaclePrefabs.Length);
        GameObject obstaclePrefab = obstaclePrefabs[randomIndex];

        Vector3 spawnPosition = new Vector3(
            Random.Range(-roadLength / 2f, roadLength / 2f),
            obstaclePrefab.transform.position.y,
            segment.position.z + Random.Range(-roadLength / 2f, roadLength / 2f)
        );

        Instantiate(obstaclePrefab, segment.TransformPoint(spawnPosition), obstaclePrefab.transform.rotation, segment);
    }
}
