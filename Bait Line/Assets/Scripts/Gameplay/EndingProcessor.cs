using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

/// <summary>
/// Processes the different endings you can get.
/// </summary>
public class EndingProcessor : MonoBehaviour {

    [Header("Good Ending")]
    public Message[] goodMessages;

    [Header("Bad Ending")]
    public Message[] badMessages;

    [Header("Neutral Ending")]
    public Message[] neutralMessages;

    [Header("Ending Elements")]
    public GameObject endingMessage;

    // Internal Variables
    private Ending _endingObtained;
    private bool _endingComplete;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        // Checks if the player can now re-click to restart.
#if !UNITY_ANDROID
        if (_endingComplete && GamePlayer.GetPlayer().GetButtonDown("UISubmit"))
#else
        if (_endingComplete && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
#endif
            SceneManager.LoadSceneAsync(0);
    }

    /// <summary>
    /// Processes and plays the good ending.
    /// </summary>
    public void ProcessGoodEnding()
    {
        // Adds the messages from the good ending and displays the dialog.
        GameManager.gameManager.textProcessor.AddMessage(goodMessages);
        GameManager.gameManager.textProcessor.DisplayText();
        _endingObtained = Ending.Good;
    }

    /// <summary>
    /// Processes and plays the bad ending.
    /// </summary>
    public void ProcessBadEnding()
    {
        // Adds the messages from the bad ending and displays the dialog.
        GameManager.gameManager.textProcessor.AddMessage(badMessages);
        GameManager.gameManager.textProcessor.DisplayText();
        _endingObtained = Ending.Bad;
    }

    /// <summary>
    /// Processes and displays the neutral ending. There's no more fish left to talk too.
    /// </summary>
    public void ProcessNeutralEnding()
    {
        // Adds the messages from the bad ending and displays the dialog.
        GameManager.gameManager.textProcessor.AddMessage(neutralMessages);
        GameManager.gameManager.textProcessor.DisplayText();
        _endingObtained = Ending.Neutral;
    }

    /// <summary>
    /// Continues the ending that was being played.
    /// </summary>
    public void ContinueEnding()
    {
        // Hides the Doctor's text box.
        GameManager.gameManager.textProcessor.ShowHideDoctor(false);
        TextMeshProUGUI endingText = endingMessage.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        // Checks which of the endings to continue.
        if (_endingObtained == Ending.Good)
        {
            GameManager.gameManager.backgroundObject.GetComponent<Animator>().SetTrigger("GoodEnding");
            GameManager.gameManager.doctorGill.GetComponent<Animator>().SetTrigger("GoodEnding");
            endingText.text = "GOOD ENDING\nThanks to Dr. Gill and Bait Line, the ocean is now a better place.\n";
        }
        else if (_endingObtained == Ending.Bad)
        {
            GameManager.gameManager.backgroundObject.GetComponent<Animator>().SetTrigger("BadEnding");
            GameManager.gameManager.doctorGill.GetComponent<Animator>().SetTrigger("BadEnding");
            endingText.text = "BAD ENDING\nBait Line was canceled as people began to doubt Dr. Gill's intentions.\n";
        }
        else if (_endingObtained == Ending.Neutral)
        {
            GameManager.gameManager.backgroundObject.GetComponent<Animator>().SetTrigger("NeutralEnding");
            endingText.text = "NEUTRAL ENDING\nBait Line kept it's daily ratings, as the program finished for another day.\n";
        }

        // Adds the platform specific message at the end.
#if !UNITY_ANDROID
        endingText.text += "Click to try again.";
#else
        endingText.text += "Tap to try again.";
#endif
    }

    /// <summary>
    /// Shows the ending message. 
    /// </summary>
    public void ShowEndingMessage()
    {
        // Shows the ending message.
        _endingComplete = true;
        endingMessage.SetActive(true);
    }
}

/// <summary>
/// States which ending the player obtained.
/// </summary>
public enum Ending
{
    Good,
    Bad,
    Neutral
}
