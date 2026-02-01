using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    // Name of the scene you want to load
    public string sceneName = "SampleScene";

    void Update()
    {
        // If Enter key is pressed
        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoadGame();
        }
    }

    // This function will be called when Play Button is clicked
    public void LoadGame()
    {
        SceneManager.LoadScene(2);
    }
}
