using UnityEngine;
using Unity.Netcode;
public class NetworkPlayerAttack : NetworkBehaviour
{
    [SerializeField] float attackRange = 5f;
    [SerializeField] int damage = 25;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] KeyCode playerAttackKey = KeyCode.Space;

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

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                break;
            }
        }

    }

    private void OnDrawGizmos()
    {   
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward, attackRange);
    }
}
