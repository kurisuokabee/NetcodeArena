using UnityEngine;
using UnityEngine.Pool;
using TMPro;

public class FloatingDamageText : MonoBehaviour
{
    private ObjectPool<FloatingDamageText> pool;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Animator animator;
    [SerializeField] private float lifeTime = 3f;

    public void SetPool(ObjectPool<FloatingDamageText> objectPool)
    {
        pool = objectPool;
    }

    private void OnEnable()
    {
        CancelInvoke();

        Invoke(nameof(ReturnToPool), lifeTime);

        if (animator != null)
        {
            animator.Rebind();
            animator.Update(0f);
        }
    }

    public void Show(int damage)
    {
        text.text = damage.ToString();

        animator.Rebind();
        animator.Update(0f);
    }

    void ReturnToPool()
    {
        pool.Release(this);
    }
}