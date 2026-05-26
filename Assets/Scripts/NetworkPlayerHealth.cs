using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class NetworkPlayerHealth : NetworkBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] private Slider healthBar;
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

        SetupHealthBar();
        UpdateHealthBarClientRpc(currentHealth.Value);
    }

    public override void OnNetworkDespawn()
    {
        currentHealth.OnValueChanged += OnHealthChange;
    }

   
    void OnHealthChange(int previousHP, int newHP)
    {
        Debug.Log($"{gameObject.name} health Change: {previousHP} -> {newHP}");
    }
    void SetupHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth.Value;
        }
    }

    [ClientRpc]
    void UpdateHealthBarClientRpc(int health)
    {
        if (healthBar != null)
        {
            healthBar.value = health;
        }
    }

    public void TakeDamage(int damage)
    {
        if (!IsServer) return;

        currentHealth.Value -= damage;

        currentHealth.Value = Mathf.Clamp(currentHealth.Value, 0, maxHealth);
        UpdateHealthBarClientRpc(currentHealth.Value);

        if (currentHealth.Value <= 0) Respawn();
    }

    public void Respawn()
    {
        currentHealth.Value = maxHealth;
        UpdateHealthBarClientRpc(currentHealth.Value);
        spawnManager.Spawn();
    }
}
