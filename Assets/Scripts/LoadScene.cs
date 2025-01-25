using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{

    public void OpenScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }


}
