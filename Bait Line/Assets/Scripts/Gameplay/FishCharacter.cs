using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds specific character functionallity and animatinos.
/// </summary>
public class FishCharacter : MonoBehaviour
{
    [Header("Fish Profile")]
    public string fishName;

    [Header("Possible Choices")]
    [SerializeField]
    private List<Dialog> dialogs;

    [Header("Audio Track")]
    public AudioClip audioTrack;

    // Unity Components
    private Animator _animator;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        // Gets any necessary Unity Components.
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Returns a Random Dialog from this specific fish.
    /// </summary>
    public Dialog ReturnRandomDialog()
    {
        // Picks a random dialog.
        int randomDialog = Random.Range(0, dialogs.Count);

        // Removes the dialog from the list and returns it.
        Dialog returnDialog = dialogs[randomDialog];
        dialogs.Remove(returnDialog);

        // Returns the dialog after checking if the fish should also be removed.
        RemoveFish();
        return returnDialog;
    }

    /// <summary>
    /// Removes this fish from the fish pool in case it no longer has dialogs.
    /// </summary>
    public void RemoveFish()
    {
        if (dialogs.Count == 0)
            GameManager.gameManager.fishPool.Remove(this);
        
    }
}
