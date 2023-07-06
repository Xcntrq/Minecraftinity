using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class Log : MonoBehaviour
{
    [SerializeField] private Collider _mainCollider;
    [SerializeField] private float _timeToBuild;
    [SerializeField] private ParticleSystem _particles;

    public void GetPlaced(Log heldLog, Log placeholderLog)
    {
        IEnumerator move = Move(heldLog, placeholderLog);
        StartCoroutine(move);
    }

    private IEnumerator Move(Log heldLog, Log placeholderLog)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = placeholderLog.transform.position;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = placeholderLog.transform.rotation;
        Vector3 startScale = heldLog.transform.localScale;
        Vector3 endScale = placeholderLog.transform.localScale;
        float timer = 0f;
        float lerp = 0f;

        while (lerp < 1f)
        {
            lerp = timer / _timeToBuild;
            transform.localScale = Vector3.Lerp(startScale, endScale, lerp);
            Quaternion rot = Quaternion.Lerp(startRot, endRot, lerp);
            Vector3 pos = Vector3.Lerp(startPos, endPos, lerp);
            transform.SetPositionAndRotation(pos, rot);
            timer += Time.deltaTime;

            yield return null;
        }

        ParticleSystem ps = Instantiate(_particles);
        ps.transform.position = placeholderLog.transform.position + placeholderLog.transform.up * 0.5f;
        ps.transform.up = transform.up;
        Destroy(ps.gameObject, 5f);

        Destroy(placeholderLog.gameObject);
        _mainCollider.enabled = true;
    }
}