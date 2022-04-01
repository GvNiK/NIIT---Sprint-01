using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadScreenUI : MonoBehaviour
{
    public Slider slider;
    public Text progressText;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        progressText = GetComponent<Text>();

        slider.value = 0.5f;
    }

}
