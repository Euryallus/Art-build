﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

//------------------------------------------------------\\
//  Used for the intro scene which shows some animated  \\
//  text to introcuce the game's story                  \\
//------------------------------------------------------\\
//   Written by Joe for prototype phase, replaced/      \\
//   partially adapted from IntroScene which            \\  
//   existed during the proof of concept phase          \\
//------------------------------------------------------\\

public class TextCutscene : MonoBehaviour
{
    //Set in inspector:
    [SerializeField]
    private Transform[] textElementContainers;      //Parent transform for each array of text elements
    [SerializeField]
    private string[] nextSceneNames;                //Name of the scene to load after each cutscene outcome ends
    [SerializeField]
    private TextMeshProUGUI textContinuePrompt;     //Text telling the player how to continue with the story
    [SerializeField]
    private float animationSpeed;                   //How quickly text is animated

    private List<TextMeshProUGUI[]> textElements;   //All text elements that may be shown in the cutscene.
                                                    //  The list represents each possible outcome/group of elements,
                                                    //  while the array contains each text element for the specific outcome

    private TextMeshProUGUI activeTextElement;  //Text element currently being shown/animated
    private int textElementIndex;               //Index of the current text element in the textElements array
    private string fullText;                    //Full text to be shown for the active text element
    private bool animatingText;                 //True = text is currently animating in, false = done animating
    private Coroutine animatingTextCoroutine;   //Coroutine used for text animation

    public static int storyIndex = 0;           //Index for the textElements list, used to determine which outcome is shown

    private void Start()
    {
        //Reset timeScale in case game was paused
        Time.timeScale = 1f;

        if (storyIndex < 0 || storyIndex >= textElementContainers.Length)
        {
            storyIndex = 0;
            Debug.LogError("Invalid story index for cutscene, changing to 0 from " + storyIndex);
        }

        textElements = new List<TextMeshProUGUI[]>();
        for (int i = 0; i < textElementContainers.Length; i++)
        {
            TextMeshProUGUI[] addedTextElements = new TextMeshProUGUI[textElementContainers[i].childCount];
            //Add all text elements within the current container (each container represents a different outcome)
            //  to the addedTextElements array
            for (int j = 0; j < textElementContainers[i].childCount; j++)
            {
                addedTextElements[j] = textElementContainers[i].GetChild(j).GetComponent<TextMeshProUGUI>();
                //Hide all text elements by default
                addedTextElements[j].gameObject.SetActive(false);
            }
            textElements.Add(addedTextElements);
        }

        //Hide the continue prompt by default
        textContinuePrompt.gameObject.SetActive(false);

        //Start animating the first text element
        textElementIndex = -1;
        ContinueStory();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Pressing the space key either skips the animation,
            //  or moves to the next text element if animating is done
            if (animatingText)
            {
                StopAnimatingText();
            }
            else
            {
                ContinueStory();
            }
        }
    }

    private void ContinueStory()
    {
        //Stop voice lines continuing to play into the next part of the cutscene
        AudioManager.instance.StopAllSoundEffects();

        if (textElementIndex < textElements[storyIndex].Length - 1)
        {
            //Start animating the next text element
            textElementIndex++;
            TextMeshProUGUI textElement = textElements[storyIndex][textElementIndex];
            StartAnimatingText(textElement);

            if (textElement.gameObject.name.Contains("Sound_"))
            {
                string voiceSoundName = textElement.gameObject.name.Substring(textElement.gameObject.name.IndexOf("Sound_") + 6);
                if (!string.IsNullOrWhiteSpace(voiceSoundName))
                {
                    AudioManager.instance.PlaySoundEffect2D(voiceSoundName);
                }
            }
        }
        else
        {
            //Done, continue to the next scene that was set based on storyIndex
            if(storyIndex < nextSceneNames.Length)
            {
                SaveLoadManager.instance.LoadSceneWithFade(nextSceneNames[storyIndex]);
            }
            else
            {
                //If an invalid story index was set, default to the first element of the nextSceneNames array
                SaveLoadManager.instance.LoadSceneWithFade(nextSceneNames[0]);
            }
        }
    }

    private void StartAnimatingText(TextMeshProUGUI textElement)
    {
        //If there was an element previously being shown, hide it
        //  to avoid overlapping text
        if (activeTextElement != null)
        {
            activeTextElement.gameObject.SetActive(false);
        }

        //Set fullText based on the text set in the editor,
        //  then reset the element's text to nothing ready to start animating
        activeTextElement = textElement;
        fullText = textElement.text;
        textElement.text = "";
        textElement.gameObject.SetActive(true);

        //Stop any existing animation
        if (animatingText)
        {
            StopAnimatingText();
        }
        //Start animating the new text
        animatingTextCoroutine = StartCoroutine(AnimateText());
    }

    private void StopAnimatingText()
    {
        //Stop animating and skip straight to showing the full text
        activeTextElement.text = fullText;
        StopCoroutine(animatingTextCoroutine);
        animatingText = false;
        //Show the continue prompt since the player can now continue to the next text element
        textContinuePrompt.gameObject.SetActive(true);
    }

    //Animates text with a typewriter effect, one letter at a time
    private IEnumerator AnimateText()
    {
        animatingText = true;
        textContinuePrompt.gameObject.SetActive(false);

        //triggerSound alternates bewteen true and false each time a character is added
        //  so sounds only play half of the time to avoid them playing too frequently
        bool triggerSound = true;

        //Used to add text one character at a time
        int charIndex = 0;

        //Continue adding text while the full text is not being shown
        while (activeTextElement.text.Length < fullText.Length)
        {
            //Add a character
            activeTextElement.text += fullText[charIndex];
            charIndex++;

            //Play a sound every other loop
            if (triggerSound)
                AudioManager.instance.PlaySoundEffect2D("Typewriter Key", 0.6f, 0.98f, 1.02f);
            triggerSound = !triggerSound;

            //Wait for a set amount of time based on animationSpeed before continuing the animaion
            yield return new WaitForSeconds(1f / animationSpeed);
        }

        //All characters are added, animating is done
        AudioManager.instance.PlaySoundEffect2D("Typewriter Ding", 0.8f, 0.98f, 1.02f);
        StopAnimatingText();
    }
}