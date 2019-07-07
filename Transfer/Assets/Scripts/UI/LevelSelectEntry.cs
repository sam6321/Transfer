using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectEntry : MonoBehaviour
{
    public string Scene { get; set; }
    public string Title { get => title.text; set => title.text = value; }
    public string Description { get => description.text; set => description.text = value; }
    public Sprite Sprite { get => image.sprite; set => image.sprite = value; }

    [SerializeField]
    private Text title;

    [SerializeField]
    private Text description;

    [SerializeField]
    private Image image;

    public void OnClickEntry()
    {
        SceneManager.LoadScene(Scene);
    }
}
