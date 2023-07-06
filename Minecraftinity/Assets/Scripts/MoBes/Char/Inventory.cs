using System;
using System.Collections;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Transform _face;
    [SerializeField] private float _pickupDistance;
    [SerializeField] private float _pullSpeed;
    [SerializeField] private int _logCount;
    [SerializeField] private RectTransform _logSlot;
    [SerializeField] private float _slotScale;
    [SerializeField] private float _slotTimer;

    private IEnumerator _scaleSlot;

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
        LogCount = _logCount;
        _scaleSlot = ScaleSlot();
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

                StopCoroutine(_scaleSlot);
                _scaleSlot = ScaleSlot();
                StartCoroutine(_scaleSlot);
            }
        }
    }

    private IEnumerator ScaleSlot()
    {
        Vector3 startScale = Vector3.one * _slotScale;
        Vector3 endScale = Vector3.one;
        float timer = 0f;
        float lerp = 0f;

        while (lerp < 1f)
        {
            lerp = timer / _slotTimer;
            _logSlot.localScale = Vector3.Lerp(startScale, endScale, lerp);
            timer += Time.deltaTime;

            yield return null;
        }
    }
}