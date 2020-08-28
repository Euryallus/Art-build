using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class introDoors : MonoBehaviour
{
    public GameObject doors;

    private void Start()
    {
        doors.SetActive(false);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            doors.SetActive(true);
        }
    }
}
