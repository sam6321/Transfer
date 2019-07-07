using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    [Serializable]
    public class LevelSelectLevel
    {
        public Sprite image;
        public string title;
        public string description;
        public string scene;
    }

    [SerializeField]
    private GameObject levelEntry;

    [SerializeField]
    private LevelSelectLevel[] levels;

    void Start()
    {
        LevelManager.Levels = levels.Select(l => l.scene).ToArray();

        RectTransform content = GetComponent<ScrollRect>().content;
        foreach (LevelSelectLevel level in levels)
        {
            GameObject entry = Instantiate(levelEntry, content);

            LevelSelectEntry selectEntry = entry.GetComponent<LevelSelectEntry>();
            selectEntry.Scene = level.scene;
            selectEntry.Sprite = level.image;
            selectEntry.Title = level.title;
            selectEntry.Description = level.description;
        }
    }
}
