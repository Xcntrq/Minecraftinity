using StarterAssets;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField] private float _cd;
    [SerializeField] private FirstPersonController _fpc;

    private float _timer;

    private void Update()
    {
        bool w = Input.GetKey(KeyCode.W);
        bool a = Input.GetKey(KeyCode.A);
        bool s = Input.GetKey(KeyCode.S);
        bool d = Input.GetKey(KeyCode.D);
        bool isWalking = w || a || s || d;

        if (_timer > 0f)
        {
            _timer -= Time.deltaTime;
        }

        if ((_timer <= 0f) && isWalking && _fpc.Grounded)
        {
            _timer += _cd;

            GetComponent<SoundPlayer>().PlayRandomSound();
        }

        if (_timer < 0f)
        {
            _timer = 0f;
        }
    }
}