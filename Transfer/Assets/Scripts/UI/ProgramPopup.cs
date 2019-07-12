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

    [SerializeField]
    private float scaleTime = 0.1f;

    [SerializeField]
    private Image background;

    private List<Image> imagesCache = new List<Image>();

    public Program Program
    {
        get => program;
        set
        {
            program = value;

            Color color = program.Colour;
            color.a = 0.2f;
            background.color = color;

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

    private void Start()
    {
        transform.localScale = Vector3.zero;
    }

    private bool shown = false;
    private float showHideTime = -1;
    public bool Shown
    {
        get => shown;
        set
        {
            shown = value;
            showHideTime = Time.time;
        }
    }

    private void Update()
    {
        if (showHideTime >= 0)
        {
            float f = Mathf.InverseLerp(showHideTime, showHideTime + scaleTime, Time.time);
            f = Mathf.SmoothStep(0, 1, f);
            if (shown)
            {
                transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, f);
            }
            else
            {
                transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, f);
            }
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
