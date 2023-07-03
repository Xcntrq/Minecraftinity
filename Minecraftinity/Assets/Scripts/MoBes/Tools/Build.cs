using UnityEngine;

public class Build : Tool
{
    [SerializeField] private Log _log;
    [SerializeField] private float _buildDistance;
    [SerializeField] private Inventory _inventory;

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        bool isLMBDown = Input.GetMouseButtonDown(0);
        bool isInventoryEmpty = _inventory.LogCount == 0;

        if (isInventoryEmpty || !isLMBDown) return;

        Ray ray = _mainCamera.ViewportPointToRay(Vector2.one * 0.5f);
        bool isSomethingHit = Physics.Raycast(ray, out RaycastHit hitInfo, _buildDistance, Physics.AllLayers, QueryTriggerInteraction.Collide);

        if (isSomethingHit)
        {
            Vector3 basePos = hitInfo.collider.transform.position + hitInfo.normal * 0.5f;
            Instantiate(_log, basePos, Quaternion.identity).transform.up = hitInfo.normal;
            _inventory.TakeOneLog();
        }
    }
}