using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgramPopup : MonoBehaviour
{
    private Program program = null;

    [SerializeField]
    private RectTransform imageHolder;

    [SerializeField]
    private Text nameText;

    [SerializeField]
    private GameObject imagePrefab;

    private List<Image> imagesCache = new List<Image>();

    public Program Program
    {
        get => program;
        set
        {
            program = value;

            nameText.text = string.Join(", ", program.Actions.Select(a => a.displayName));

            EnsureEnoughImages(program.Actions.Count);
            for(int i = 0; i < imagesCache.Count; i++)
            {
                Image image = imagesCache[i];
                if(i >= program.Actions.Count)
                {
                    image.gameObject.SetActive(false);
                }
                else
                {
                    image.gameObject.SetActive(true);
                    image.sprite = program.Actions[i].icon;
                }
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }
    }

    private void EnsureEnoughImages(int count)
    {
        if(!imageHolder)
        {
            imageHolder = transform.Find("ImagesHolder") as RectTransform;
        }

        int needed = count - imagesCache.Count;
        while(needed > 0)
        {
            GameObject image = Instantiate(imagePrefab, imageHolder);
            imagesCache.Add(image.GetComponent<Image>());
            needed--;
        }
    }
}
