using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // rip avicii...
    public static string[] Levels { get; set; } = null;
    public static bool AutoAdvance { get; private set; } = true;
    public static float RobotSpeed { get; private set; } = 1;

    private string nextLevel = null;

    [SerializeField]
    private GameObject levelWinDialog;

    [SerializeField]
    private Button nextLevelButton = null;

    // Start is called before the first frame update
    void Start()
    {
        //Mouse events should only interact with robots, not with other elements
        Physics.queriesHitTriggers = false;

        if (Levels != null)
        {
            Scene scene = SceneManager.GetActiveScene();
            int index = Array.IndexOf(Levels, scene.name);
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

        DragSource.EnablePopups();
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
        DragSource.DisablePopups();
        levelWinDialog.SetActive(true);
    }

    public void OnResetLevel()
    {
        // Reload the current level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetAutoAdvance(bool autoAdvance)
    {
        AutoAdvance = autoAdvance;
    }

    public void SetRobotSpeed(float speed)
    {
        RobotSpeed = speed;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
