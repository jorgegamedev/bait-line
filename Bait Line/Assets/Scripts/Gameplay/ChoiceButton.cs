using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds the type of choice that is selected when this button is pressed.
/// </summary>
public class ChoiceButton : MonoBehaviour {

    // Internal Variables
    private ChoiceType _type;

    /// <summary>
    /// Assigns the choice type to the button.
    /// </summary>
    public void AssignChoice(ChoiceType type)
    {
        _type = type;
    }

    /// <summary>
    /// Called when this button choice is called.
    /// </summary>
    public void PickedChoice()
    {
        GameManager.gameManager.PickedChoiceOption(_type);
    }
}

/// <summary>
/// Handles the different type of choices. These are used to keep track of the action type you've picked.
/// </summary>
public enum ChoiceType
{
    Positive,
    Neutral,
    Negative
}