using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[System.Serializable]
public class WeightedItemPrefab
{
    public FuelPickup prefab;
    [Range(0f, 1f)] public float weight = 1f;
    [Min(1)] public int stackDefaultCapacity = 10;
    [Min(1)] public int maxPoolSize = 50;
}

public class ItemObjectPool : MonoBehaviour
{
    public List<WeightedItemPrefab> items = new();
    private readonly Dictionary<FuelPickup, IObjectPool<FuelPickup>> _pools = new();
    private float _totalWeight;

    private void Awake()
    {
        BuildPools();
    }

    private void BuildPools()
    {
        _pools.Clear();
        _totalWeight = 0f;

        foreach (var w in items)
        {
            if (w.prefab == null || w.weight <= 0f) continue;
            _totalWeight += w.weight;

            // capture local variables for the factory/lambdas
            var prefab = w.prefab;
            IObjectPool<FuelPickup> pool = null;

            pool = new ObjectPool<FuelPickup>(
                createFunc: () =>
                {
                    var item = Instantiate(prefab, transform);
                    item.name = prefab.name + " (Pooled)";
                    item.Pool = pool; // assign its pool so it can Release()
                    return item;
                },
                actionOnGet: item => { item.gameObject.SetActive(true); },
                actionOnRelease: item => { item.gameObject.SetActive(false); },
                actionOnDestroy: item => { if (item != null) Destroy(item.gameObject); },
                collectionCheck: true,
                defaultCapacity: w.stackDefaultCapacity,
                maxSize: w.maxPoolSize
            );

            _pools[prefab] = pool;
        }
    }

    private FuelPickup ChoosePrefab()
    {
        float r = Random.value * _totalWeight;
        float acc = 0f;
        foreach (var w in items)
        {
            if (w.prefab == null || w.weight <= 0f) continue;
            acc += w.weight;
            if (r <= acc) return w.prefab;
        }
        return items.Count > 0 ? items[0].prefab : null;
    }

    public FuelPickup SpawnRandom(Vector3 position, Quaternion rotation)
    {
        var prefab = ChoosePrefab();
        if (prefab == null) return null;

        var pool = _pools[prefab];
        var item = pool.Get();
        item.transform.SetPositionAndRotation(position, rotation);
        return item;
    }
}
