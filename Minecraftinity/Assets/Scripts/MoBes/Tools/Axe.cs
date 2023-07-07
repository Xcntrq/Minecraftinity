using Cinemachine;
using UnityEngine;

public class Axe : Tool
{
    [SerializeField] private float _hitDistance;
    [SerializeField] private CameraShaker _cameraShaker;
    [SerializeField] private float _shakeAmp;
    [SerializeField] private float _shakeFreq;
    [SerializeField] private float _shakeTime;
    [SerializeField] private float _shakeVelMin;
    [SerializeField] private float _shakeVelMax;
    [SerializeField] private ParticleSystem _particleHit;
    [SerializeField] private SoundPlayer _missSounds;
    [SerializeField] private SoundPlayer _hitSounds;

    private Camera _mainCamera;

    public void OnSwingProc()
    {
        Ray ray = _mainCamera.ViewportPointToRay(Vector2.one * 0.5f);
        bool isSomethingHit = Physics.Raycast(ray, out RaycastHit hitInfo, _hitDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore);

        if (isSomethingHit && (hitInfo.rigidbody != null) && hitInfo.rigidbody.TryGetComponent(out Block block))
        {
            block.GetChopped();
            //_cameraShaker.ShakeVCam(_shakeAmp, _shakeFreq, _shakeTime);

            ParticleSystem ps = Instantiate(_particleHit);
            Vector3 offset = hitInfo.point - hitInfo.collider.transform.position;
            ps.transform.position = hitInfo.collider.transform.position + offset * 0.5f;
            ps.transform.up = offset;
            Destroy(ps.gameObject, 5f);

            _hitSounds.PlayRandomSound();

            Vector3 vel = Vector3.zero;
            vel.x = Random.Range(0, 2) == 0 ? Random.Range(_shakeVelMin, _shakeVelMax) : -Random.Range(_shakeVelMin, _shakeVelMax);
            vel.y = Random.Range(0, 2) == 0 ? Random.Range(_shakeVelMin, _shakeVelMax) : -Random.Range(_shakeVelMin, _shakeVelMax);
            vel.z = Random.Range(0, 2) == 0 ? Random.Range(_shakeVelMin, _shakeVelMax) : -Random.Range(_shakeVelMin, _shakeVelMax);
            GetComponent<CinemachineImpulseSource>().GenerateImpulseWithVelocity(vel);
        }
        else
        {
            _missSounds.PlayRandomSound();
        }
    }

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        GetComponent<Animator>().SetBool("IsSwinging", Input.GetMouseButton(0) || Input.GetMouseButtonDown(0));

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