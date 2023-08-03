using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Manages the display of different game elements on the Canvas, other than the text.
/// </summary>
public class CanvasManager : MonoBehaviour {

    [Header("Choice Elements")]
    [SerializeField]
    private GameObject choicePanel;
    [SerializeField]
    private ChoiceButton[] choiceButtons;

    [Header("Display Elements")]
    [SerializeField]
    private GameObject fishPane;
    [SerializeField]
    private AudioClip goodRatings;
    [SerializeField]
    private AudioClip badRatings;

    [Header("Ratings")]
    [SerializeField]
    private Image ratingsImage;
    [SerializeField]
    public Sprite[] ratingSprites;

    /// <summary>
    /// Shows and setup's the choices on the screen for the player to pick.
    /// </summary>
    public void DisplayChoices()
    {
        // Sets the Choice Panel as active.
        choicePanel.SetActive(true);

        // Prepares the actions to be assigned to their respective buttons.
        List<ChoiceButton> pendingButtons = new List<ChoiceButton>();
        pendingButtons.AddRange(choiceButtons);
        int randomChoice = 0;

        // Assigns positive information to the random button.
        randomChoice = Random.Range(0, pendingButtons.Count);
        pendingButtons[randomChoice].AssignChoice(ChoiceType.Positive);
        pendingButtons[randomChoice].GetComponentInChildren<TextMeshProUGUI>().text = GameManager.gameManager.currentDialog.positiveChoice;
        pendingButtons.Remove(pendingButtons[randomChoice]);

        // Assigns positive information to the random button.
        randomChoice = Random.Range(0, pendingButtons.Count);
        pendingButtons[randomChoice].AssignChoice(ChoiceType.Neutral);
        pendingButtons[randomChoice].GetComponentInChildren<TextMeshProUGUI>().text = GameManager.gameManager.currentDialog.neutralChoice;
        pendingButtons.Remove(pendingButtons[randomChoice]);

        // Assigns positive information to the random button.
        randomChoice = Random.Range(0, pendingButtons.Count);
        pendingButtons[randomChoice].AssignChoice(ChoiceType.Negative);
        pendingButtons[randomChoice].GetComponentInChildren<TextMeshProUGUI>().text = GameManager.gameManager.currentDialog.negativeChoice;
        pendingButtons.Remove(pendingButtons[randomChoice]);

        // De-Selects any selected Game Object.
        EventSystem.current.SetSelectedGameObject(null);
    }

    /// <summary>
    /// Shows the fish pane with the calling fish on it.
    /// </summary>
    public void ShowFishPane()
    {
        // Sets the picked fish character active.
        GameManager.gameManager.currentFish.gameObject.SetActive(true);

        // Plays the Canvas animation for the fish pane.
        fishPane.SetActive(true);
    }

    /// <summary>
    /// Hides the fish pane and the current fish after the animation is done.
    /// </summary>
    public IEnumerator HideFishPane()
    {
        // Plays the closing animation and waits for the animation to be done.
        GameManager.gameManager.textProcessor.ShowHideCaller(false);
        fishPane.GetComponent<Animator>().SetTrigger("ClosePane");
        yield return new WaitForSeconds(0.86f);

        // Sets the picked character as inactive.
        fishPane.SetActive(false);
        GameManager.gameManager.currentFish.gameObject.SetActive(false);
    }

    /// <summary>
    /// Forces the choice of a specific choice.
    /// </summary>
    public void ForceChoice(int button)
    {
        choiceButtons[button].PickedChoice();
    }

    /// <summary>
    /// Hide choices panel after the choices have been made.
    /// </summary>
    public void HideChoices()
    {
        choicePanel.SetActive(false);
    }

    /// <summary>
    /// Updates the display of the ratings depending on the Doctor's actions.
    /// </summary>
    public void UpdateRatingsDisplay(int currentRatings, int feedback)
    {
        // Depending on the number of ratings, show a different image.
        float ratingNumber = currentRatings / 5;
        int ratingSprite = (int)Mathf.Round(ratingNumber);

        // Checks if the game should display positive, negative or no ratings.
        switch(feedback)
        {
            case 0:
                ratingsImage.transform.GetChild(0).gameObject.SetActive(true);
                MusicManager.musicManager.PlayFeedbackAudio(goodRatings);
                break;
            case 1:
                ratingsImage.transform.GetChild(1).gameObject.SetActive(true);
                MusicManager.musicManager.PlayFeedbackAudio(badRatings);
                break;
        }

        // Shows the correct sprite on the image.
        ratingsImage.sprite = ratingSprites[ratingSprite];
        ratingsImage.gameObject.SetActive(true);
    }
}
