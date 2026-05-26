using UnityEngine;
using Unity.Netcode;
public class NetworkPlayerAttack : NetworkBehaviour
{
    [SerializeField] float attackRange = 5f;
    int damage;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] KeyCode playerAttackKey = KeyCode.Mouse0;
    ObjectPooler objectPooler;

    void Start()
    {
        objectPooler = ObjectPooler.instance;
    }
    private void Update()
    {   
        if(!IsOwner) return;

        if(Input.GetKeyDown(playerAttackKey))
        {
            RequestAttackServerRpc();
        }
    }

    [ServerRpc]
    void RequestAttackServerRpc()
    {
        Vector3 attackCenter = transform.position + transform.forward;
        Collider[] hits = Physics.OverlapSphere(attackCenter, attackRange, playerLayer);

        foreach (Collider hit in hits)
        {
            if (hit.gameObject == gameObject) continue;

            NetworkPlayerHealth playerHealth = hit.GetComponent<NetworkPlayerHealth>();

            damage = Random.Range(10, 25);

            if (playerHealth != null)
            {   
                playerHealth.TakeDamage(damage);

                ShowDamageClientRpc(damage, playerHealth.transform.position + new Vector3(
                Random.Range(-0.5f, 0.5f),
                2,
                0f));

                break;
            }
        }

    }

    [ClientRpc]
    void ShowDamageClientRpc(int damage, Vector3 position)
    {
        var damageText = ObjectPooler.instance.Get();

        damageText.transform.position = position;
        damageText.Show(damage);
    }

    private void OnDrawGizmos()
    {   
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward, attackRange);
    }
}
