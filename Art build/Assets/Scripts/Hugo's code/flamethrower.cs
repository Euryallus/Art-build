using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flamethrower : MonoBehaviour
{
    [SerializeField]
    private float timePlayerOnFire = 3f;
    [SerializeField]
    private int damageDonePerInterval = 2;
    [SerializeField]
    private float intervalLength = 0.5f;

    [SerializeField]
    private float damageDoneWhenInFlames = 0.5f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<playerHealth>().setOnFire(timePlayerOnFire, damageDonePerInterval, intervalLength);

            other.gameObject.GetComponent<playerHealth>().takeDamage(damageDoneWhenInFlames);
        }
    }
}
