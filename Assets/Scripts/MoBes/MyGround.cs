using UnityEngine;

[DisallowMultipleComponent]
public class MyGround : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private float _scale;
    [SerializeField] private Dirt _dirt;
    [SerializeField] private MyTree _tree;
    [SerializeField] private float _treeChance;

    public void Generate()
    {
        Transform[] children = transform.GetComponentsInChildren<Transform>();

        for (int i = children.Length - 1; i >= 0; i--)
        {
            if (children[i] != transform)
            {
                DestroyImmediate(children[i].gameObject);
            }
        }

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
                    Transform go = Instantiate(_dirt, transform).transform;
                    go.localPosition = new Vector3(x, y, z);
                }

                bool treeChance = Random.Range(0f, 1f) < _treeChance;

                if (treeChance)
                {
                    Transform go = Instantiate(_tree, transform).transform;
                    go.localPosition = new Vector3(x, maxY, z);
                }
            }
        }
    }
}