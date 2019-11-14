using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomOptionsPanelScript : MonoBehaviour
{
    //Labels
    public Text AxisXLabel;
    public Text AxisYLabel;
    public Text AxisZLabel;

    //Sliders
    public Slider AxisXSlider;
    public Slider AxisYSlider;
    public Slider AxisZSlider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Updating labels from the sliders values
        AxisXLabel.text = "X Axis Offset : " + AxisXSlider.value.ToString();
        AxisYLabel.text = "Y Axis Offset : " + AxisYSlider.value.ToString();
        AxisZLabel.text = "Z Axis Offset : " + AxisZSlider.value.ToString();
    }
}
