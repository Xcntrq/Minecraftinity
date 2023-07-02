using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private Collider _trigger;

    public void GetChopped()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        if (!rb.isKinematic) return;

        transform.localScale *= 0.3f;
        Vector3 force = Vector3.up;
        force.x = Random.Range(-1f, 1f);
        force.z = Random.Range(-1f, 1f);
        rb.isKinematic = false;
        rb.AddForce(force * 4, ForceMode.VelocityChange);
        _trigger.gameObject.SetActive(true);
    }
}