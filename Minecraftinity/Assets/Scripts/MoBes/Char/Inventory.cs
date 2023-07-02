using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Transform _face;
    [SerializeField] private float _pickupDistance;
    [SerializeField] private float _pullSpeed;
    [SerializeField] private int _logCount;

    public event Action<int> OnLogCountChanged;

    private void Start()
    {
        OnLogCountChanged?.Invoke(_logCount);
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if ((rb != null) && rb.TryGetComponent(out Block block))
        {
            Vector3 dir = _face.position - rb.position;
            rb.velocity = _pullSpeed * dir.normalized;
            float dist = Vector3.Distance(_face.position, rb.position);
            if (dist < _pickupDistance)
            {
                _logCount++;
                OnLogCountChanged?.Invoke(_logCount);
                Destroy(block.gameObject);
            }
        }
    }
}