using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Used for destroying or disabling objects at the end of animations.
public class AnimationDestroy : MonoBehaviour {

    [Header("SetSelected()")]
    public GameObject selected;

    /// <summary>
    /// Called by the animation, destroys this object.
    /// </summary>
    public void DestroyAnimation()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Called by the animation, disables this object.
    /// </summary>
    public void DisableGameObject()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Disables All Game Objects and it's childs.
    /// </summary>
    public void DisableGameObjectAndChillds()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Called by the animation, disables the animator.
    /// </summary>
    public void DisableAnimator()
    {
        GetComponent<Animator>().enabled = false;
    }

    /// <summary>
    /// Sets a button as the selected.
    /// </summary>
    public void SetSelected()
    {
        EventSystem.current.SetSelectedGameObject(selected);
    }

    /// <summary>
    /// Shows the ending message after the player has completed the game.
    /// </summary>
    public void ShowEndingMessage()
    {
        GameManager.gameManager.endingProcessor.ShowEndingMessage();
    }

    /// <summary>
    /// Plays the sound attached to this object.
    /// </summary>
    public void PlaySound(AudioClip clip)
    {
        GetComponent<AudioSource>().PlayOneShot(clip);
    }
}
