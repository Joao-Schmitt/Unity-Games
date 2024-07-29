using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public int id;
    public float distanceToDetect;
    public GameObject objTextDoor;
    public string textLockedDoor, textOpenedDoor, textUnlockedDoor;
    public bool unlocked;


    private TextMeshProUGUI textDoor;
    private Transform player;
    private InventoryController inventoryController;
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        textDoor = objTextDoor.GetComponent<TextMeshProUGUI>();
        inventoryController = player.GetComponent<InventoryController>();
    }
    void Update()
    {
        if (CheckProximity())
        {
            objTextDoor.SetActive(true);

            if (unlocked)
            {
                textDoor.text = textUnlockedDoor;
                
                if (Input.GetKeyDown(KeyCode.E))
                    ChangeDoor();
            }
            else
            {
                var key = inventoryController.keys.Where(x => x == id).FirstOrDefault();

                if (key != 0)
                {
                    textDoor.text = textOpenedDoor;

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        unlocked = true;
                        ChangeDoor();
                    }
                }
                else
                {
                    textDoor.text = textLockedDoor;
                }
            }
        }
        else
        {
            objTextDoor.SetActive(false);
        }
    }

    void ChangeDoor()
    {
        anim.SetTrigger("change");
    }

    bool CheckProximity()
    {
        return Vector3.Distance(transform.position, player.position) <= distanceToDetect;
    }
}
