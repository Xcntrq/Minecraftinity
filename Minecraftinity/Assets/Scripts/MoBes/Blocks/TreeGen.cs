using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[DisallowMultipleComponent]
public class TreeGen : MonoBehaviour
{
    [SerializeField] private Log _log;
    [SerializeField] private Leaf _leaf;

    public void Generate(bool isInEditor)
    {
        Transform[] children = transform.GetComponentsInChildren<Transform>();

        for (int i = children.Length - 1; i >= 0; i--)
        {
            if (children[i] != transform)
            {
                DestroyBlock(children[i].gameObject, isInEditor);
            }
        }

        int height = 5 + 2 * Random.Range(0, 3);
        int maxWidth = Mathf.Max(height / 2, 7);
        int y = height / 2;

        for (int layer = 1; layer < height; layer++)
        {
            int layerWidth;

            if (layer > height / 2)
            {
                layerWidth = Mathf.Min(1 + (height - layer) * 2, maxWidth);
            }
            else
            {
                layerWidth = Mathf.Min(1 + layer * 2, maxWidth);
            }

            for (int x = -layerWidth / 2; x <= layerWidth / 2; x++)
            {
                for (int z = -layerWidth / 2; z <= layerWidth / 2; z++)
                {
                    bool isLog = (x == 0) && (z == 0) && (y < height);
                    bool isRandomlyRemoved = false;

                    if ((x == -layerWidth / 2) || (x == layerWidth / 2) || (z == -layerWidth / 2) || (z == layerWidth / 2))
                    {
                        isRandomlyRemoved = Random.Range(0, 5) == 0;
                    }

                    Vector3 localPos = new(x, y + 0.5f, z);
                    Vector3 globalPos = transform.TransformPoint(localPos);
                    Collider[] hitColliders = new Collider[1];
                    bool isOccupied = Physics.OverlapSphereNonAlloc(globalPos, 0.1f, hitColliders, Physics.AllLayers, QueryTriggerInteraction.Collide) > 0;

                    if (!isLog && !isRandomlyRemoved && !isOccupied)
                    {
                        Transform go = CreateBlock(_leaf, isInEditor);
                        go.localPosition = new Vector3(x, y, z);
                    }
                }
            }

            y++;
        }

        for (y = 0; y < height; y++)
        {
            Vector3 localPos = new(0, y + 0.5f, 0);
            Vector3 globalPos = transform.TransformPoint(localPos);
            Collider[] hitColliders = new Collider[1];
            bool isOccupied = Physics.OverlapSphereNonAlloc(globalPos, 0.1f, hitColliders, Physics.AllLayers, QueryTriggerInteraction.Collide) > 0;

            if (isOccupied && (hitColliders[0].attachedRigidbody != null) && hitColliders[0].attachedRigidbody.TryGetComponent(out Leaf _))
            {
                DestroyBlock(hitColliders[0].attachedRigidbody.gameObject, isInEditor);
            }

            Transform go = CreateBlock(_log, isInEditor);
            go.localPosition = new Vector3(0, y, 0);
        }
    }

    private Transform CreateBlock(MonoBehaviour moBe, bool isInEditor)
    {

#if UNITY_EDITOR
        if (isInEditor)
        {
            return PrefabUtility.InstantiatePrefab(moBe, transform).GetComponent<Transform>();
        }
#endif

        return Instantiate(moBe, transform).transform;
    }

    private void DestroyBlock(GameObject go, bool isInEditor)
    {

#if UNITY_EDITOR
        if (isInEditor)
        {
            DestroyImmediate(go);
            return;
        }
#endif

        Destroy(go);
    }
}