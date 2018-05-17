using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    [SerializeField]
    private Slider timeScaleSlider;
    [SerializeField]
    private float step = 0.5f;
    [SerializeField]
    private KeyCode increaseButton;
    [SerializeField]
    private KeyCode decreaseButton;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(decreaseButton))
        {
            timeScaleSlider.value -= step;
        }
        else if (Input.GetKeyDown(increaseButton))
        {
            timeScaleSlider.value += step;
        }

        Time.timeScale = timeScaleSlider.value;
    }
}
