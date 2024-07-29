using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class NightVisionController : MonoBehaviour
{
    public PostProcessProfile normal, nightVision;
    public AudioSource nightVisionSound;
    public TextMeshProUGUI textBattery;
    public float speedReduce, speedIncrement;

    private float batteryValue;
    private PostProcessVolume volume;
    private bool nightVisionActive;
    void Start()
    {
        volume = GetComponent<PostProcessVolume>();
        batteryValue = 100;  
    }

    void Update()
    {
        textBattery.text = batteryValue.ToString("N0");

        if (nightVisionActive)
            batteryValue = Mathf.Clamp(batteryValue -= speedReduce * Time.deltaTime, 0, 100);
        else
            batteryValue = Mathf.Clamp(batteryValue += speedIncrement * Time.deltaTime, 0, 100);


        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!nightVisionActive)
            {
                nightVisionActive = true;
                volume.profile = nightVision;
                nightVisionSound.Play();
            }
            else
            {
                nightVisionActive = false;
                volume.profile = normal;
            }
        }
    }
}
