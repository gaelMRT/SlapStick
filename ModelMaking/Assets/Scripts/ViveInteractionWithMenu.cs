using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;


public class Menu_Controller_Interaction : MonoBehaviour
{
    void Awake()
    {
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;
    }

    public void PointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.name == "AudioButton")
        {
            Debug.Log("Start was clicked");
        }
        else if (e.target.name == "PlayerOptionsButton")
        {
            Debug.Log("Player Options was clicked");
        }
    }

    public void PointerInside(object sender, PointerEventArgs e)
    {
        if (e.target.name == "AudioButton")
        {
            Debug.Log("Start was entered");
        }
        else if (e.target.name == "PlayerOptionsButton")
        {
            Debug.Log("Player Options was entered");
        }
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {
        if (e.target.name == "AudioButton")
        {
            Debug.Log("Start was exited");
        }
        else if (e.target.name == "PlayerOptionsButton")
        {
            Debug.Log("Player Options was exited");
        }
    }
}
