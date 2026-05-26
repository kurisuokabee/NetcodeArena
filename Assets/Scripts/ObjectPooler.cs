using UnityEngine;
using UnityEngine.Pool;

public class ObjectPooler : MonoBehaviour
{   
    public static ObjectPooler instance;
    [Header("Pool Settings")]
    [SerializeField] private FloatingDamageText prefab;
    [SerializeField] private int defaultCapacity = 10;
    [SerializeField] private int maxSize = 30;

    private ObjectPool<FloatingDamageText> pool;

    private void Awake()
    {
        pool = new ObjectPool<FloatingDamageText>(
            CreateObject,
            OnGet,
            OnRelease,
            OnDestroyItem,
            true,
            defaultCapacity,
            maxSize
        );

        instance = this;
    }

    FloatingDamageText CreateObject()
    {
        FloatingDamageText text = Instantiate(prefab, transform);
        text.SetPool(pool);

        return text;
    }

    private void OnGet(FloatingDamageText text)
    {
        text.gameObject.SetActive(true);
    }

    private void OnRelease(FloatingDamageText text)
    {
        text.gameObject.SetActive(false);
    }

    private void OnDestroyItem(FloatingDamageText text)
    {
        Destroy(text.gameObject);
    }

     public FloatingDamageText Get()
    {
        return pool.Get();
    }

    public void Return(FloatingDamageText text)
    {
        pool.Release(text);
    }
}