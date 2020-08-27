using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossHealthPool : MonoBehaviour
{
    [SerializeField]
    private float damagePerSec = 0.5f;

    [SerializeField]
    private float slowPercentage = 0.25f;
    [SerializeField]
    private float slowTime = 1.5f;

    private Vector3 originalPos;
    private float randomOffset;
    private float mag = 0.15f;

    private float time;
    private bossEnemy parent;

    private void Start()
    {
        originalPos = transform.position;
        randomOffset = Random.Range(0, 360);
    }

    private void FixedUpdate()
    {
        time += Time.deltaTime;

        transform.position = originalPos + ((Mathf.Sin(time + randomOffset) * Vector3.up) * mag);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<playerHealth>().takeDamage(damagePerSec);
            parent.externalRegenCall();
            other.gameObject.GetComponent<playerMovement>().slowEffect(slowPercentage, slowTime);
        }
    }



    public void setParent(bossEnemy source)
    {
        parent = source;
    }
}
