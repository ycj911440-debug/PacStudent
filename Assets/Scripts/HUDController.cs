using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDController : MonoBehaviour
{
    public void ReturnToStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
}
