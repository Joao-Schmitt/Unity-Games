using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMoviment : MonoBehaviour
{
    public float speed;
    void Update()
    {
        var inputX = Input.GetAxis("Horizontal");
        transform.Translate(new Vector3(inputX, 0, 0) * Time.deltaTime * speed);
    }
}
