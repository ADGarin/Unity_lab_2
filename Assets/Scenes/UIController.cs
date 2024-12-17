using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public void OnStartGame()
    {
        SceneManager.LoadScene("DynScene");
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
