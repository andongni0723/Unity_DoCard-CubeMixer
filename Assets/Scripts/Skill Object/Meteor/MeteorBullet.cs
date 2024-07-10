using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorBullet : MonoBehaviour
{
    [Header("Component")]
    public GameObject hitVFX;
    [Header("Settings")] 
    public float speed = 10;
    //[Header("Debug")]

    private void Update()
    {
        transform.Translate(Vector3.forward * (speed * Time.deltaTime), Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Plane"))
        {
            Instantiate(hitVFX, transform.position, Quaternion.identity);
            CameraShake.Instance.Shake(5, 0.1f);
            Destroy(gameObject);
        }
    }
}
