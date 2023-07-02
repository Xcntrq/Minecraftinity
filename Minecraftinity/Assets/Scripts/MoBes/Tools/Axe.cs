using UnityEngine;

public class Axe : Tool
{
    [SerializeField] private float _hitDistance;

    private Camera _mainCamera;

    public void OnSwingProc()
    {
        Ray ray = _mainCamera.ViewportPointToRay(Vector2.one * 0.5f);
        bool isSomethingHit = Physics.Raycast(ray, out RaycastHit hitInfo, _hitDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore);

        if (isSomethingHit && (hitInfo.rigidbody != null) && hitInfo.rigidbody.TryGetComponent(out Block block))
        {
            block.GetChopped();
        }
    }

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        GetComponent<Animator>().SetBool("IsSwinging", Input.GetMouseButton(0));

        bool w = Input.GetKey(KeyCode.W);
        bool a = Input.GetKey(KeyCode.A);
        bool s = Input.GetKey(KeyCode.S);
        bool d = Input.GetKey(KeyCode.D);

        GetComponent<Animator>().SetBool("IsMoving", w || a || s || d);
    }

    /*private void OnDrawGizmos()
    {
        Ray ray = Camera.main.ViewportPointToRay(Vector2.one * 0.5f);
        Debug.DrawRay(ray.origin, ray.direction.normalized * _hitDistance, Color.red, 0.5f);
    }*/
}