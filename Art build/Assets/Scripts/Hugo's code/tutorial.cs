using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorial : MonoBehaviour
{
    [SerializeField]
    private Door enemyDoor;

    [SerializeField]
    private Door generatorDoor;

    [SerializeField]
    private GeneratorRepair generator;

    [SerializeField]
    private Door notesDoor;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if(enemies.Length == 0)
        {
            enemyDoor.SetLocked(false);
        }

        if(generator.GetGeneratorRepaired())
        {
            generatorDoor.SetLocked(false);
        }

        GameObject[] notes = GameObject.FindGameObjectsWithTag("PickUp");
        if(notes.Length == 0)
        {
            notesDoor.SetLocked(false);
        }
    }
}
