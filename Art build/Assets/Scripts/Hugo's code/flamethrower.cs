using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flamethrower : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<playerHealth>().setOnFire(3, 2, 0.5f);
            other.gameObject.GetComponent<playerHealth>().takeDamage(0.5f);
        }
    }
}
