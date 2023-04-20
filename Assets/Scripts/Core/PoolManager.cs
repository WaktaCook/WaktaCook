using UnityEngine;
using UnityEngine.Pool;
using WaktaCook.Gameplay;

public abstract class PoolManager<T> : MonoBehaviour where T : Component
{
    public T prefab;

    // pool 관련
    public enum PoolType
    {
        Stack,
        LinkedList
    }

    public PoolType poolType;

    // Collection checks will throw errors if we try to release an item that is already in the pool.
    public bool collectionCheck = true;
    public int maxSize = 10;

    IObjectPool<T> m_Pool;
    protected IObjectPool<T> Pool
    {
        get
        {
            if (m_Pool == null)
            {
                if (poolType == PoolType.Stack)
                    m_Pool = new ObjectPool<T>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionCheck, 10, maxSize);
                else
                    m_Pool = new LinkedPool<T>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionCheck, maxSize);
            }
            return m_Pool;
        }
        set
        {
            m_Pool = value;
        }
    }

    protected virtual T CreatePooledItem()
    {
        return Instantiate(prefab.gameObject, this.transform).GetComponent<T>();
    }

    protected virtual void OnTakeFromPool(T item)
    {
        item.gameObject.SetActive(true);
    }

    protected virtual void OnReturnedToPool(T item)
    {
        item.gameObject.SetActive(false);
    }

    protected virtual void OnDestroyPoolObject(T item)
    {
        Destroy(item.gameObject);
    }

    public virtual T Get()
    {
        return Pool.Get();
    }
}
