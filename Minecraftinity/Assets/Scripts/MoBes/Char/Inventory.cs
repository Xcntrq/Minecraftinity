using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Transform _face;
    [SerializeField] private float _pickupDistance;
    [SerializeField] private float _pullSpeed;
    [SerializeField] private int _logCount;

    public int LogCount
    {
        get => _logCount;

        private set
        {
            _logCount = value;
            OnLogCountChanged?.Invoke(_logCount);
        }
    }

    public event Action<int> OnLogCountChanged;

    public void TakeOneLog()
    {
        LogCount -= 1;
    }

    private void Start()
    {
        LogCount = 0;
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
                LogCount += 1;
                Destroy(block.gameObject);
            }
        }
    }
}