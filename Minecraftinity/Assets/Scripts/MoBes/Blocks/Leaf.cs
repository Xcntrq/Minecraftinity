using AmazingAssets.AdvancedDissolve;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Leaf : MonoBehaviour
{
    [SerializeField] private WorldGen _worldGen;
    [SerializeField] private int _logRange;
    [SerializeField] private float _logCheckDelay;
    [SerializeField] private float _deathDealyMin;
    [SerializeField] private float _deathDealyMax;
    [SerializeField] private List<Block> _nearbyLogs;
    [SerializeField] private float _dissolveTime;
    [SerializeField] private Renderer _renderer;

    public void SetWorldGen(WorldGen worldGen)
    {
        _worldGen = worldGen;
        _worldGen.OnWorldGenerated += WorldGen_OnWorldGenerated;
    }

    private void OnDestroy()
    {
        _worldGen.OnWorldGenerated -= WorldGen_OnWorldGenerated;
    }

    private void Awake()
    {
        if (_worldGen != null)
        {
            _worldGen.OnWorldGenerated += WorldGen_OnWorldGenerated;
        }
    }

    private void Start()
    {
        Collider[] hitColliders = new Collider[10];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position + Vector3.up * 0.5f, 0.1f, hitColliders, Physics.AllLayers, QueryTriggerInteraction.Collide);

        for (int i = 0; i < numColliders; i++)
        {
            Collider collider = hitColliders[i];
            if ((collider.attachedRigidbody != null) && collider.attachedRigidbody.TryGetComponent(out Log _))
            {
                print("removing a leaf");
                Destroy(gameObject);
                break;
            }
        }
    }

    private void WorldGen_OnWorldGenerated()
    {
        FindNearbyLogs();
        IEnumerator logChecker = NearbyLogCheck();
        StartCoroutine(logChecker);
    }

    private void FindNearbyLogs()
    {
        _nearbyLogs = new List<Block>();
        Vector3 centerOfSelf = transform.position + 0.5f * Vector3.up;

        for (int x = -_logRange; x <= _logRange; x++)
        {
            for (int y = -_logRange; y <= _logRange; y++)
            {
                for (int z = -_logRange; z <= _logRange; z++)
                {
                    Vector3 offset = new(x, y, z);
                    Collider[] hitColliders = new Collider[1];
                    bool isOccupied = Physics.OverlapSphereNonAlloc(centerOfSelf + offset, 0.1f, hitColliders, Physics.AllLayers, QueryTriggerInteraction.Collide) > 0;

                    if (isOccupied && (hitColliders[0].attachedRigidbody != null) && hitColliders[0].attachedRigidbody.TryGetComponent(out Block log))
                    {
                        _nearbyLogs.Add(log);
                    }
                }
            }
        }
    }

    private IEnumerator NearbyLogCheck()
    {
        while (_nearbyLogs.Count > 0)
        {
            for (int i = _nearbyLogs.Count - 1; i >= 0; i--)
            {
                if ((_nearbyLogs[i] == null) || _nearbyLogs[i].IsChopped)
                {
                    _nearbyLogs.RemoveAt(i);
                }
            }

            yield return new WaitForSeconds(_logCheckDelay);
        }

        IEnumerator death = Suicide();
        StartCoroutine(death);
    }

    private IEnumerator Suicide()
    {
        float delay = Random.Range(_deathDealyMin, _deathDealyMax);
        yield return new WaitForSeconds(delay);

        Material material = _renderer.material;

        float timer = 0f;
        float lerp = 0f;

        while (lerp < 1f)
        {
            lerp = timer / _dissolveTime;
            AdvancedDissolveProperties.Cutout.Standard.UpdateLocalProperty(material, AdvancedDissolveProperties.Cutout.Standard.Property.Clip, lerp);
            timer += Time.deltaTime;

            yield return null;
        }

        Destroy(gameObject);
    }
}