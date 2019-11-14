using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOptionsPanelScript : MonoBehaviour
{
    //Labels
    public Text PlayerSizeLabel;

    //Sliders
    public Slider PlayerSizeSlider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Updating labels from the sliders values
        PlayerSizeLabel.text = "Player Size : " + PlayerSizeSlider.value.ToString();
    }
}
