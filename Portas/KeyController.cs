using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    public int id;
    public float distanceToDetect;
    public string textKey;

    public GameObject objTextKey;
    private Transform player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) <= distanceToDetect)
        {
            objTextKey.GetComponent<TextMeshProUGUI>().text = textKey;
            objTextKey.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                player.GetComponent<InventoryController>().AddItem(id);
                objTextKey.SetActive(false);
                Destroy(gameObject);
            }
        }
        else
        {
            objTextKey.SetActive(false);
        }
    }
}
