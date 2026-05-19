using Unity.Netcode;
using UnityEngine;

public class PlayerSpawnManager : NetworkBehaviour
{
    private static int nextSpawnIndex;

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            return;
        }

        Spawn();
    }

    public void Spawn()
    {
        GameObject[] spawnPointObjects = GameObject.FindGameObjectsWithTag("SpawnPoint");

        if (spawnPointObjects.Length == 0)
        {
            Debug.LogWarning("No objects with the SpawnPoint tag were found.");
            return;
        }

        Transform selectedSpawnPoint = spawnPointObjects[nextSpawnIndex].transform;

        CharacterController characterController = GetComponent<CharacterController>();

        if (characterController != null)
        {
            characterController.enabled = false;
        }

        transform.SetPositionAndRotation(selectedSpawnPoint.position, selectedSpawnPoint.rotation);

        if (characterController != null)
        {
            characterController.enabled = true;
        }

        nextSpawnIndex++;

        if (nextSpawnIndex >= spawnPointObjects.Length)
        {
            nextSpawnIndex = 0;
        }
    }
}