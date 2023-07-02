using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[DisallowMultipleComponent]
public class WorldGen : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private float _scale;
    [SerializeField] private Grass _grass;
    [SerializeField] private TreeGen _treeGen;
    [SerializeField] private float _treeChance;

    public void Generate(bool isInEditor)
    {
        Transform[] children = transform.GetComponentsInChildren<Transform>();

        for (int i = children.Length - 1; i >= 0; i--)
        {
            if (children[i] != transform)
            {
                DestroyImmediate(children[i].gameObject);
            }
        }

        Transform grass = new GameObject("Grass").transform;
        Transform trees = new GameObject("Trees").transform;
        grass.parent = transform;
        trees.parent = transform;

        for (int x = -_width; x <= _width; x++)
        {
            for (int z = -_width; z <= _width; z++)
            {
                float xf = Mathf.InverseLerp(-_width, _width, x) * _scale;
                xf -= (int)xf;

                float zf = Mathf.InverseLerp(-_width, _width, z) * _scale;
                zf -= (int)zf;

                float perlin = Mathf.PerlinNoise(xf, zf);
                float yf = Mathf.Lerp(0, _height, perlin);
                int maxY = (int)yf;

                for (int y = maxY; y <= maxY; y++)
                {
                    Transform go = CreateBlock(_grass, grass, isInEditor);
                    go.localPosition = new Vector3(x, y, z);
                }

                bool treeChance = Random.Range(0f, 1f) < _treeChance;

                if (treeChance)
                {
                    Transform go = CreateBlock(_treeGen, trees, isInEditor);
                    go.localPosition = new Vector3(x, maxY, z);
                    go.GetComponent<TreeGen>().Generate(isInEditor);
                }
            }
        }
    }

    private Transform CreateBlock(MonoBehaviour moBe, Transform transform, bool isInEditor)
    {

#if UNITY_EDITOR
        if (isInEditor)
        {
            return PrefabUtility.InstantiatePrefab(moBe, transform).GetComponent<Transform>();
        }
#endif

        return Instantiate(moBe, transform).transform;
    }
}