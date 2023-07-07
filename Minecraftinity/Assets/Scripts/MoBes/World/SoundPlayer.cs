using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] _clips;
    [SerializeField] private float _volume;

    public void PlayRandomSound()
    {
        if (_clips.Length > 1)
        {
            int i = Random.Range(1, _clips.Length);
            GetComponent<AudioSource>().PlayOneShot(_clips[i], _volume);
            (_clips[0], _clips[i]) = (_clips[i], _clips[0]);
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(_clips[0], _volume);
        }
    }
}