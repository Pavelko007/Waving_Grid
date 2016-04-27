using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadNext(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
