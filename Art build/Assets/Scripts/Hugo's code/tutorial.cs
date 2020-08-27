using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    private Text tutorialText;

    private bool shownSSText = false;

    private starStoneManager starStone;

    void Start()
    {
        starStone = GameObject.FindGameObjectWithTag("GeneratorManager").GetComponent<starStoneManager>();
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

        if(starStone.returnActive() != starStoneManager.starStones.None && shownSSText == false)
        {
            displayText("Each stone gives your prototype weapon a different effect");
            shownSSText = true;
        }
    }

    public void displayText(string textToDisp)
    {
        tutorialText.text = textToDisp;
    }
}
