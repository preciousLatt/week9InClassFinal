using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Collider))]
public class FuelPickup : MonoBehaviour
{
    public IObjectPool<FuelPickup> Pool { get; set; }

    [Header("Lifecycle")]
    [SerializeField] private float selfDespawnTime = 10f;

    [Header("Launch (optional)")]
    [SerializeField] private Vector3 launchDir = Vector3.up;
    [SerializeField] private float launchSpeed = 0f;

    private Rigidbody _rb;
    private Coroutine _lifeRoutine;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnEnable()
    {
        // reset / kick it a bit if we have a rigidbody
        if (_rb != null)
        {
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;

            if (launchSpeed > 0f)
            {
                _rb.AddForce(launchDir.normalized * launchSpeed, ForceMode.VelocityChange);
            }
        }
        _lifeRoutine ??= StartCoroutine(DespawnAfter());
    }

    private void OnDisable()
    {
        if (_lifeRoutine != null)
        {
            StopCoroutine(_lifeRoutine);
            _lifeRoutine = null;
        }
    }

    private IEnumerator DespawnAfter()
    {
        yield return new WaitForSeconds(selfDespawnTime);
        ReturnToPool();
    }

    public void ReturnToPool()
    {
        Pool?.Release(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.AddFuel(); 
        }
        ReturnToPool();
    }
}
