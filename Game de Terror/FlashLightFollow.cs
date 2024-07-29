using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightFollow : MonoBehaviour
{
    public float speed;
    private Transform target;
   
    void Start()
    {
        target = Camera.main.transform;
    }

    void Update()
    {
        transform.position = target.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, speed * Time.deltaTime);
    }
}
