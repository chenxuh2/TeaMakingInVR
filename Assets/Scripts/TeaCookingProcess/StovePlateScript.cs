using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

//In this script, the plate starts to change its olor after the stove has been activated (to show that its hot). After the plate is hot, tea can be cooked (And the hand can be burned)
public class StovePlateScript : MonoBehaviour
{
    private bool stoveIsActive = false;


    //For color change and heatup process
    [SerializeField] private Material plateColdMaterial, plateHotMaterial;
    private float heatUpTimeSoFar = 0f;
    private float maxHeatUpTime = 10f;
    private MeshRenderer thisRenderer;
    private bool plateIsHot = false;


    //Variables for the status light
    [SerializeField] GameObject statusLight;
    MeshRenderer statusLightRenderer;

    //For water cooking
    GameObject waterKettle;

    // Start is called before the first frame update
    void Start()
    {
        thisRenderer = GetComponent<MeshRenderer>();

        statusLightRenderer = statusLight.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Stove is heating up (after activation)
        if (stoveIsActive && heatUpTimeSoFar <= maxHeatUpTime)
        {
            changeColorOfStovePlate(heatUpTimeSoFar);
            heatUpTimeSoFar += Time.deltaTime;
        } else if(!plateIsHot && stoveIsActive && heatUpTimeSoFar >= maxHeatUpTime)
        {
            setPlateIsHot(true);


            //Change the color of the status light
            statusLightRenderer.material.SetColor("_EmissionColor", Color.green);
        }

        //If the stove is turned of
        if(plateIsHot && !stoveIsActive && heatUpTimeSoFar >= 0f)
        {
            setPlateIsHot(false);
            //Change the color of the status light
            statusLightRenderer.material.SetColor("_EmissionColor", Color.red);
        } 
        else if (!stoveIsActive && heatUpTimeSoFar >= 0f)         //Cooldown phase
        {
            changeColorOfStovePlate(heatUpTimeSoFar);
            heatUpTimeSoFar -= Time.deltaTime;
        }
        else if (!stoveIsActive && heatUpTimeSoFar <= 0f && statusLightRenderer.material.IsKeywordEnabled("_EMISSION"))
        {
            statusLightRenderer.material.DisableKeyword("_EMISSION");
        }


    }

    public void setStoveActivity(bool val)
    {
        stoveIsActive = val;
    }

    public bool getIsStoveActive() 
    {
        return stoveIsActive; 
    }

    public float getPlateTemperature()
    {
        return heatUpTimeSoFar;
    }

    private void changeColorOfStovePlate(float timePassed)
    {
        float progression = Mathf.Clamp(timePassed / maxHeatUpTime, 0, 1);     //progression must be between 0 and 1 for Color.Lerp
        thisRenderer.material.color = Color.Lerp(plateColdMaterial.color, plateHotMaterial.color, progression);
    }

    public bool getIsPlateHot()
    {
        return plateIsHot;
    }

    public void setWaterKettle(GameObject obj)
    {
        waterKettle = obj;
    }

    public void setPlateIsHot(bool val)
    {
        plateIsHot = val;
        if (waterKettle != null)
        {
            try
            {
                waterKettle.GetComponent<ManageWaterflow>().setPlateIsHot(val);
            }
            catch
            {
                Debug.Log("The water kettle seems not to have the water kettle script ManageWaterflow.cs");
            }
        }
    }
}
