using UnityEngine;

public class Build : Tool
{
    [SerializeField] private Log _logHeld;
    [SerializeField] private Log _logColliderless;
    [SerializeField] private Log _logPlaceholder;
    [SerializeField] private float _buildDistance;
    [SerializeField] private Inventory _inventory;

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        bool w = Input.GetKey(KeyCode.W);
        bool a = Input.GetKey(KeyCode.A);
        bool s = Input.GetKey(KeyCode.S);
        bool d = Input.GetKey(KeyCode.D);
        GetComponent<Animator>().SetBool("IsMoving", w || a || s || d);

        bool isLMBDown = Input.GetMouseButtonDown(0);
        bool isInventoryEmpty = _inventory.LogCount == 0;

        if (isInventoryEmpty || !isLMBDown) return;

        Ray ray = _mainCamera.ViewportPointToRay(Vector2.one * 0.5f);
        bool isSomethingHit = Physics.Raycast(ray, out RaycastHit hitInfo, _buildDistance, Physics.AllLayers, QueryTriggerInteraction.Collide);

        if (isSomethingHit)
        {
            _inventory.TakeOneLog();
            Vector3 basePos = hitInfo.collider.transform.position + hitInfo.normal * 0.5f;
            Log placeholderLog = Instantiate(_logPlaceholder, basePos, Quaternion.identity);
            placeholderLog.transform.up = hitInfo.normal;
            Log newLog = Instantiate(_logColliderless, _logHeld.transform.position, _logHeld.transform.rotation);
            newLog.GetPlaced(_logHeld, placeholderLog);
            GetComponent<Animator>().SetTrigger("GetReady");
        }
    }
}