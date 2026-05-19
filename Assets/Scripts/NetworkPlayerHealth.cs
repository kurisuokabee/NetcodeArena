using UnityEngine;
using Unity.Netcode;
public class NetworkPlayerHealth : NetworkBehaviour
{
    [SerializeField] int maxHealth = 100;
    public NetworkVariable<int> currentHealth = new(
        100,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
        );

    PlayerSpawnManager spawnManager;


    public override void OnNetworkSpawn()
    {
        if(IsServer)
        {
            currentHealth.Value = maxHealth; 
        }

        spawnManager = GetComponent<PlayerSpawnManager>();

        currentHealth.OnValueChanged += OnHealthChange;
    }

    public override void OnNetworkDespawn()
    {
        currentHealth.OnValueChanged += OnHealthChange;
    }

   

    void OnHealthChange(int previousHP, int newHP)
    {
        Debug.Log($"{gameObject.name} health Change: {previousHP} -> {newHP}");
    }

    public void TakeDamage(int damage)
    {
        if (!IsServer) return;

        currentHealth.Value -= damage;

        currentHealth.Value = Mathf.Clamp(currentHealth.Value, 0, maxHealth);

        if (currentHealth.Value <= 0) Respawn();
    }

    public void Respawn()
    {
        currentHealth.Value = maxHealth;
        spawnManager.Spawn();
    }
}
