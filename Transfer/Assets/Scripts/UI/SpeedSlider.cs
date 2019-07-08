using UnityEngine;
using UnityEngine.UI;

public class SpeedSlider : MonoBehaviour
{
    [SerializeField]
    private Text speedText;

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Text titleText;

    private bool isEnabled = true;

    public bool Enabled
    {
        get => isEnabled;
        set
        {
            isEnabled = value;
            if(isEnabled)
            {
                Color32 grey = new Color32(50, 50, 50, 255);
                speedText.color = grey;
                titleText.color = grey;
                slider.interactable = true;
            }
            else
            {
                Color32 grey = new Color32(128, 128, 128, 255);
                speedText.color = grey;
                titleText.color = grey;
                slider.interactable = false;
            }
        }
    }

    public void OnSliderValueChanged(float value)
    {
        speedText.text = ((int)value).ToString() + 's';
    }
}
