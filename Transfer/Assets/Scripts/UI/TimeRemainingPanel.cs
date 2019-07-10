using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeRemainingPanel : MonoBehaviour
{
    [SerializeField]
    private Image progress;

    [SerializeField]
    private RobotManager robotManager;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            OnStep();
        }
        float f = Mathf.InverseLerp(robotManager.NextUpdate - robotManager.UpdateDelay, robotManager.NextUpdate, Time.time);
        progress.transform.localScale = new Vector3(1, 1.0f - f, 1);
    }

    public void OnStep()
    {
        robotManager.ForceStep();
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
