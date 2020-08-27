using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField]
    private string tutorialText;

    [SerializeField]
    private tutorial tutorialManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialManager.displayText(tutorialText);
            Destroy(gameObject);
        }
        
    }
}
