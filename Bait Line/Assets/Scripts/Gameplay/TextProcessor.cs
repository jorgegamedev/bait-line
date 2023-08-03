using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles the display of text elements on screen, and all the message stack.
/// </summary>
public class TextProcessor : MonoBehaviour {

    [Header("Text Status")]
    [ReadOnly]
    public bool isDisplaying;
    [ReadOnly]
    public bool isWaitingForInput;
    private bool _forceSkip, _jumpMessage;

    [Header("Text Display")]
    [SerializeField]
    private GameObject doctorTextBox;
    [SerializeField]
    private GameObject callerTextBox;
    [SerializeField]
    public AudioClip[] bubbleSounds;
    private AudioSource _carretAudio;

    [Header("Pending Messages")]
    [SerializeField]
    private List<Message> messageStack;

    // Internal Status
    private TextMeshProUGUI doctorMessage;
    private TextMeshProUGUI callerName;
    private TextMeshProUGUI callerMessage;
    private TextMeshProUGUI textDisplay;
    private GameObject _activeTextBox;
    private GameObject _activeIndicator;

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        // Gets the Unity components related to the different text entries.
        doctorMessage = doctorTextBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        callerMessage = callerTextBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        callerName = callerTextBox.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _carretAudio = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Used for adding a single message to the message stack.
    /// </summary>
    public void AddMessage(Message message)
    {
        messageStack.Add(message);
    }

    /// <summary>
    /// Used for adding a array of messages to the message stack.
    /// </summary>
    public void AddMessage(Message[] messages)
    {
        // Adds all messages to the stack.
        foreach (Message message in messages)
        {
            messageStack.Add(message);
        }
    }

    /// <summary>
    /// Displays the text in the expected text box.
    /// </summary>
    public void DisplayText()
    {
        StartCoroutine(DisplayTextEnumerator());
    }

    /// <summary>
    /// Used to display text per-character.
    /// </summary>
    IEnumerator DisplayTextEnumerator()
    {
        // Gets the first message on the stack.
        isDisplaying = true;

        // Checks if there's an active indicator.
        if (_activeIndicator)
            _activeIndicator.SetActive(false);

        // Prepares the message stack.
        switch (messageStack[0].fish)
        {
            case Fish.DoctorGill:
                _activeTextBox = doctorTextBox;
                _activeIndicator = _activeTextBox.transform.Find("Indicator").gameObject;
                textDisplay = doctorMessage;
                GameManager.gameManager.doctorGill.GetComponent<Animator>().SetBool("Talking", true);
                break;
            case Fish.Caller:
                _activeTextBox = callerTextBox;
                _activeIndicator = _activeTextBox.transform.Find("Indicator").gameObject;
                textDisplay = callerMessage;

                // Sets the text box as active.
                if (callerTextBox.activeInHierarchy == false)
                    ShowHideCaller(true);
                break;
        }

        // Displays the message.
        Message message = messageStack[0];
        string messageString = message.text;
        string display = "";
        int currentBeep = 0;

        // Plays audio associated with this message if it exists.
        if (message.eventClip != null)
            MusicManager.musicManager.PlayEventAudio(message.eventClip);

        // Displays the message letter by letter.
        foreach (char letter in messageString)
        {
            if (!_forceSkip)
            {
                if (!_forceSkip)
                {
                    // Displays the letter and plays the caret sound.
                    display += letter;
                    textDisplay.text = display;

                    // Does the beeping.
                    currentBeep++;
                    if (currentBeep == 3)
                    {
                        int randomSound = Random.Range(0, bubbleSounds.Length);
                        _carretAudio.PlayOneShot(bubbleSounds[randomSound]);
                        currentBeep = 0;
                    }
                }

                // Checks if it's a period, waits longer in that case.
                if (letter == '.' || letter == '!' || letter == '?')
                    yield return new WaitForSeconds(0.25f);
                else if (letter == ',')
                    yield return new WaitForSeconds(0.1f);
                else
                    yield return new WaitForSeconds(0.02f);
            }
        }

        // Since a skip has been forced, let's show all the messages.
        if (_forceSkip && !_jumpMessage)
        {
            textDisplay.text = messageString;
            _forceSkip = false;
        }
        else if (!_forceSkip && _jumpMessage)
        {
            _jumpMessage = false;
        }

        // Prepares the next message.
        CompleteMessage(message);
    }

    /// <summary>
    /// Completes the message, adding it to the stack.
    /// </summary>
    void CompleteMessage(Message message)
    {
        // Removes the message from the current message stack.
        messageStack.Remove(message);
        _activeIndicator.SetActive(true);

        // Completes the animations depending on the message fish.
        switch(message.fish)
        {
            case Fish.DoctorGill:
                GameManager.gameManager.doctorGill.GetComponent<Animator>().SetBool("Talking", false);
                break;
        }

        // Checks if the player has asked a skip.
        if (_forceSkip)
        {
            isDisplaying = false;
            isWaitingForInput = false;
        }
        else
        {
            isWaitingForInput = true;
        }
    }

    /// <summary>
    /// Clears the text-box and turns off the next indicator.
    /// </summary>
    public void ClearTextBox()
    {
        textDisplay.text = "";
    }

    /// <summary>
    /// Used for skipping the message and not wait for input.
    /// </summary>
    public void SkipMessage()
    {
        // Updates the text and ignores the letter by letter execution.
        _forceSkip = true;
    }

    /// <summary>
    /// Displays the next message in the stack.
    /// </summary>
    /// <returns>If there are more messages to be displayed or not.</returns>
    public bool NextMessage()
    {
        // Turns text processing flags off.
        isWaitingForInput = false;
        isDisplaying = false;

        // Starts the next function.
        if (messageStack.Count > 0)
        {
            DisplayText();
            return true;
        }
        else
        {
            _activeIndicator.SetActive(false);
            return false;
        }
    }

    /// <summary>
    /// Shows the Doctor text box. Can be used by the Game Manager.
    /// </summary>
    public void ShowHideDoctor(bool show)
    {
        if(show)
            doctorTextBox.SetActive(show);
        else
            doctorTextBox.GetComponent<Animator>().SetTrigger("FadeOut");
    }

    /// <summary>
    /// Shows and Hides the Caller text box. Can be used by the Canvas Controller.
    /// </summary>
    public void ShowHideCaller(bool show)
    {
        if(show)
        {
            callerName.text = GameManager.gameManager.currentFish.fishName;
            callerTextBox.SetActive(show);
        }
        else
            callerTextBox.GetComponent<Animator>().SetTrigger("FadeOut");
    }
}
