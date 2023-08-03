using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Choice base. Defines a conversation that will be played on the show, and the choices to be played.
/// </summary>
[CreateAssetMenu(fileName = "New Choice", menuName = "Custom/Choice")]
public class Dialog : ScriptableObject {

    [Header("Choice Base")]
    public Message[] messages;

    [Header("Positive Choice")]
    public string positiveChoice;
    public Message[] positiveMessages;

    [Header("Neutral Choice")]
    public string neutralChoice;
    public Message[] neutralMessages;

    [Header("Negative Choice")]
    public string negativeChoice;
    public Message[] negativeMessages;
}
