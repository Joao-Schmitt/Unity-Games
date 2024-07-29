using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryController : MonoBehaviour
{
    public float distanceToDetect = 1;
    public float value = 30;
    public GameObject txtGetBattery;
    private Transform flashLight;
    void Start()
    {
        flashLight = GameObject.FindGameObjectWithTag("Flashlight").transform;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, flashLight.position) <= distanceToDetect)
        {
            txtGetBattery.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                flashLight.GetComponent<FlashLightController>().AddBattery(value);
                txtGetBattery.SetActive(false);
                Destroy(gameObject);
            }
        }
        else
        {
            txtGetBattery.SetActive(false);
        }
    }
}
