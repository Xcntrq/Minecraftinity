using TMPro;
using UnityEngine;

public class LogTMP : MonoBehaviour
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
        GetComponent<TextMeshProUGUI>().SetText($"{count}");
        GetComponent<TextMeshProUGUI>().enabled = count > 0;
    }
}