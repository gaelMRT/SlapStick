using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AudioPanelScript : MonoBehaviour
{

    //Labels
    public Text MasterVolumeLabel;
    public Text MusicVolumeLabel;
    public Text SFXVolumeLabel;

    //Sliders
    public Slider MasterVolumeSlider;
    public Slider MusicVolumeSlider;
    public Slider SFXVolumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Updating the labels according to the slider values
        MasterVolumeLabel.text = "Master Volume : " + MasterVolumeSlider.value.ToString();
        MusicVolumeLabel.text = "Music Volume : " + MusicVolumeSlider.value.ToString();
        SFXVolumeLabel.text = "Sound Effects Volume : " + SFXVolumeSlider.value.ToString();
    }
}
