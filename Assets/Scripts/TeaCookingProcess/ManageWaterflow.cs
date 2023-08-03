using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageWaterflow : MonoBehaviour
{
    [SerializeField] private GameObject waterParticleSystemObject;
    [SerializeField] private GameObject steamParticleSystemObject;
    private bool particleSystemIsActive = false;

    private ParticleSystem waterParticleSystem;
    private ParticleSystem steamParticleSystem;

    //For water cooking
    private bool isOnPlate = false;
    private bool plateIsHot;
    private bool waterIsCooking = false;


    private float cookingTimeSoFar;
    private float maxCookingTime = 10;


    // Start is called before the first frame update
    void Start()
    {
        waterParticleSystem = waterParticleSystemObject.GetComponent<ParticleSystem>();
        waterParticleSystem.Stop();

        steamParticleSystem = steamParticleSystemObject.GetComponent<ParticleSystem>();
        steamParticleSystem.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        float eulerRotationAroundZ = waterParticleSystemObject.transform.rotation.eulerAngles.z % 360;
        if (particleSystemIsActive == false && (eulerRotationAroundZ) > 35 && (eulerRotationAroundZ) < 180)
        {
            //particleSystemObject.SetActive(true);
            waterParticleSystem.Play();
            particleSystemIsActive = true;
        }

        if (particleSystemIsActive == true && ((eulerRotationAroundZ) < 30 || (eulerRotationAroundZ) > 185))
        {
            //particleSystemObject.SetActive(false);
            waterParticleSystem.Stop();
            particleSystemIsActive = false;
        }


        //Water is heating up (after plate is hot)
        if (isOnPlate && plateIsHot && cookingTimeSoFar <= maxCookingTime)
        {
            cookingTimeSoFar += Time.deltaTime;
        }
        else if (!waterIsCooking && isOnPlate && plateIsHot && cookingTimeSoFar >= maxCookingTime)
        {
            waterIsCooking = true;

            //Activate the steam
            steamParticleSystem.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("StovePlate"))
        {
            isOnPlate = true;
            other.gameObject.GetComponent<StovePlateScript>().setWaterKettle(this.gameObject);

            //Get the plate, see if it is hot
            plateIsHot = other.gameObject.GetComponent<StovePlateScript>().getIsPlateHot();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("StovePlate"))
        {
            isOnPlate = false;
            cookingTimeSoFar = 0f;
        }
    }


    public void setPlateIsHot(bool val)
    {
        plateIsHot = val;
    }

    public bool getIsWaterCooking()
    {
        return waterIsCooking;
    }
}
