using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DigitalDoorController : MonoBehaviour
{
    public Transform panel;
    public float distanceToDetect = 1;
    public float distanceToDetectOpenDoor = 1;
    public MonoBehaviour[] classesToDisable;
    public GameObject groupPanel, textKey;
    public string password;
    public bool unlocked = false;
    public TMP_InputField inputKey;

    private Transform player;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (unlocked)
        {
            textKey.SetActive(false);
            OpenCloseDoor();
        }
        else
        {
            if (CheckProximty(panel, distanceToDetect))
            {
                textKey.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                    ShowPanel();

                if (Input.GetKeyDown(KeyCode.Escape))
                    ClosePanel();
            }
            else
            {
                textKey.SetActive(false);
            }
        }
    }

    void OpenCloseDoor()
    {
        anim.SetBool("open", CheckProximty(transform, distanceToDetectOpenDoor));
    }

    public void ConfirmPassword()
    {
        if (inputKey.text == password)
        {
            unlocked = true;
            ClosePanel();
        }
    }

    void ShowPanel()
    {
        groupPanel.SetActive(true);
        SetClassesActive(false);
        Cursor.lockState = CursorLockMode.None;
    }

    public void ClosePanel()
    {
        groupPanel.SetActive(false);
        SetClassesActive(true);
        Cursor.lockState = CursorLockMode.Locked;
    }

    void SetClassesActive(bool active)
    {
        foreach (MonoBehaviour _class in classesToDisable)
        {
            _class.enabled = active;
        }
    }

    private bool CheckProximty(Transform reference, float distance)
    {
        return Vector3.Distance(reference.position, player.position) <= distance;
    }
}
