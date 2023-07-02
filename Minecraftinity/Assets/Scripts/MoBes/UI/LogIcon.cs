using UnityEngine;
using UnityEngine.UI;

public class LogIcon : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;

    private void Awake()
    {
        _inventory.OnLogCountChanged += Inventory_OnLogCountChanged;
    }

    private void OnDestroy()
    {
        _inventory.OnLogCountChanged -= Inventory_OnLogCountChanged;
    }

    private void Inventory_OnLogCountChanged(int count)
    {
        GetComponent<Image>().enabled = count > 0;
    }
}