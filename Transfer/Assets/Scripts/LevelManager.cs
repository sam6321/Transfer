using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // rip avicii...
    public static string[] Levels { get; set; } = null;

    private string nextLevel = null;

    [SerializeField]
    private GameObject levelWinDialog;

    [SerializeField]
    private Button nextLevelButton = null;

    // Start is called before the first frame update
    void Start()
    {
        if (Levels != null)
        {
            Scene scene = SceneManager.GetActiveScene();
            int index = Array.IndexOf(Levels, scene);
            if (index >= 0)
            {
                // Set up the next level that we'll load into when the user wins
                if (index == Levels.Length - 1)
                {
                    // We're on the final level now
                    nextLevel = null;
                }
                else
                {
                    nextLevel = Levels[index + 1];
                }
            }
        }

        if (nextLevelButton)
        {
            nextLevelButton.gameObject.SetActive(nextLevel != null);
        }
    }

    public void OnNextLevelClick()
    {
        if(nextLevel != null)
        {
            SceneManager.LoadScene(nextLevel);
        }
    }

    public void OnReturnToMainMenuClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnWinLevel()
    {
        levelWinDialog.SetActive(true);
    }

    public void OnResetLevel()
    {
        // Reload the current level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
