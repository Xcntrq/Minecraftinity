using UnityEngine;

[DisallowMultipleComponent]
public class MyTree : MonoBehaviour
{
    [SerializeField] private Log _log;
    [SerializeField] private Leaf _leaf;

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
                    bool chance = true;

                    if ((x == -layerWidth / 2) || (x == layerWidth / 2) || (z == -layerWidth / 2) || (z == layerWidth / 2))
                    {
                        chance = Random.Range(0, 4) != 0;
                    }

                    if (!isLog && chance)
                    {
                        Transform go = Instantiate(_leaf, transform).transform;
                        go.localPosition = new Vector3(x, y, z);
                    }
                }
            }

            y++;
        }

        for (y = 0; y < height; y++)
        {
            Transform go = Instantiate(_log, transform).transform;
            go.localPosition = new Vector3(0, y, 0);
        }
    }
}