using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public void MenuButtonClick(int index)
    {
        switch (index)
        {
            case 0:
                print("Clicked button " + index);
                break;
            case 1:
                print("Clicked button " + index);
                break;
            case 2:
                print("Clicked button " + index);
                break;
            default:
                break;
        }
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
