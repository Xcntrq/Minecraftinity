using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Inventory _))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}