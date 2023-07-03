using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private float _upForce;
    [SerializeField] private float _sideForceDelta;
    [SerializeField] private Collider _trigger;

    public bool IsChopped => !GetComponent<Rigidbody>().isKinematic;

    public void GetChopped()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        if (!rb.isKinematic) return;

        transform.localScale *= 0.3f;
        Vector3 force = Vector3.zero;
        force.y = _upForce;
        force.x = Random.Range(-_sideForceDelta, _sideForceDelta);
        force.z = Random.Range(-_sideForceDelta, _sideForceDelta);
        rb.isKinematic = false;
        rb.AddForce(force, ForceMode.VelocityChange);
        _trigger.gameObject.SetActive(true);
    }
}