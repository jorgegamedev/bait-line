using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Defines a messages that will be displayed and the sounds associated to it.
/// </summary>
[System.Serializable]
public class Message
{
    [TextArea]  
    public string text;
    public Fish fish;
    public AudioClip eventClip;
}

/// <summary>
/// Defines which of the fish is talking at the moment. Will display the different text messages at different places.
/// </summary>
public enum Fish
{
    DoctorGill,
    Caller
}