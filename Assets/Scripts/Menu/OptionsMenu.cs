using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class OptionsMenu : MonoBehaviour
{
    //The InputFields, used to set the text
    public TMP_InputField minSpeedInput;


    // Start is called before the first frame update. Set the default value for the InputFields (get them from the static class)
    void Start()
    {
        try
        {
            minSpeedInput.text = StaticGameOptions.minSpeed.ToString();

        }  
        catch
        {
            Debug.Log("Error in Options menu by setting the deafult values of the options. Check that all Input Fields are assigned in the OptionMenuScript");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //used to read in the value of min speed in the options menu
    public void setMinSpeed(string minSpeedGivenString)
    {
        int minSpeedGiven = Int32.Parse(minSpeedGivenString);
        //Only set the value if it is smaller than the max value, otherwise set it to the max
        if (minSpeedGiven > 8) // 8 = StaticGameOptions.maxSpeed default
        {
            StaticGameOptions.minSpeed = 8;
            minSpeedInput.text = StaticGameOptions.minSpeed.ToString();
        }
        else
        {
            StaticGameOptions.minSpeed = minSpeedGiven;
        }
    }

}
