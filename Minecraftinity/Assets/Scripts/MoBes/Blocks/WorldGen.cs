using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Random;

[DisallowMultipleComponent]
public class WorldGen : MonoBehaviour
{
    [SerializeField] private int _radius;
    [SerializeField] private int _height;
    [SerializeField] private float _scale;
    [SerializeField] private Grass _grass;
    [SerializeField] private TreeGen _treeGen;
    [SerializeField] private float _treeChance;

    public event Action OnWorldGenerated;

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

        for (int x = -_radius; x <= _radius; x++)
        {
            for (int z = -_radius; z <= _radius; z++)
            {
                if (x * x + z * z >= _radius * _radius) continue;

                float xf = Mathf.InverseLerp(-_radius, _radius, x) * _scale;
                xf -= (int)xf;

                float zf = Mathf.InverseLerp(-_radius, _radius, z) * _scale;
                zf -= (int)zf;

                float perlin = Mathf.PerlinNoise(xf, zf);
                float yf = Mathf.Lerp(0, _height, perlin);
                int perlinY = (int)yf;

                float distanceFromCenter = Mathf.Sqrt(x * x + z * z);
                int circleY = _radius - (int)distanceFromCenter - 1;

                int actualY = Mathf.Min(perlinY, circleY);

                for (int y = actualY; y <= actualY; y++)
                {
                    Transform go = CreateBlock(_grass, grass, isInEditor);
                    go.localPosition = new Vector3(x, y, z);
                }

                bool isTreeProcced = Range(0f, 1f) < _treeChance;

                if (isTreeProcced)
                {
                    Transform go = CreateBlock(_treeGen, trees, isInEditor);
                    go.localPosition = new Vector3(x, actualY, z);
                    go.GetComponent<TreeGen>().Generate(this, isInEditor);
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

    private void Start()
    {
        OnWorldGenerated?.Invoke();
    }
}