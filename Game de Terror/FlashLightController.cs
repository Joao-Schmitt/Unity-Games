using TMPro;
using UnityEngine;

public class FlashLightController : MonoBehaviour
{
    public TextMeshProUGUI tmpBattery;
    public AudioSource turnLightSound;
    public float minSpotAngle = 5;
    public float maxSpotAngle = 70;
    public float multiplier = 5;
    public Material materialFlash;

    public float reduceBattery = 10;

    private float batteryValue = 100;

    [HideInInspector]
    public Light light;

    void Start()
    {
        light = GetComponentInChildren<Light>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            TurnFlashLight();

        if (light.enabled)
        {
            SetFocusLight();
            ReduceBattery();
            materialFlash.EnableKeyword("_EMISSION");
        }
        else
        {
            materialFlash.DisableKeyword("_EMISSION");
        }


        SetUI();
    }

    void TurnFlashLight()
    {
        if (batteryValue != 0)
            light.enabled = !light.enabled;
        else
            light.enabled = false;

        turnLightSound.Play(); 
    }

    void SetFocusLight()
    {
        light.spotAngle = Mathf.Clamp(light.spotAngle += Input.GetAxis("Mouse ScrollWheel") * multiplier, minSpotAngle, maxSpotAngle);
    }
    
    void ReduceBattery()
    {
        batteryValue = Mathf.Clamp(batteryValue -= reduceBattery * Time.deltaTime, 0, 100);
    }

    void SetUI()
    {
        tmpBattery.text = batteryValue.ToString("N0");
    }

    public void AddBattery(float value)
    {
        batteryValue = Mathf.Clamp(batteryValue += value, 0, 100);
    }
}
