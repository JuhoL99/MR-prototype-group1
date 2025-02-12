using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonHandler : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Juho"); // Change "GameScene" to your actual scene name
    }

    public void ShowAbout()
    {
        Debug.Log("Show About Info"); // Replace this with actual UI panel activation
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
