using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Handles the Tittle Screen and starting the game.
/// </summary>
public class TitleScreen : MonoBehaviour {

    private bool _hasStarted;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        // Checks if the player has clicked anywhere, which starts the game anyways.
        if(!_hasStarted)
        {
#if !UNITY_ANDROID
        if (GamePlayer.GetPlayer().GetButtonDown("UISubmit"))
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
#endif
            StartGame();
        }
    }

    /// <summary>
    /// Starts the game by performing screen animations.
    /// </summary>
    public void StartGame()
    {
        // Marks the game as having started.
        if(!_hasStarted)
        {
            _hasStarted = true;
            StartCoroutine(GameTransition());
        }
    }

    /// <summary>
    /// Does the transition for the actual game over a set of animations and lerps.
    /// </summary>
    IEnumerator GameTransition()
    {
        // Turns on the animator on both the main camera and tittle screen.
        GetComponent<Animator>().enabled = true;
        Camera.main.GetComponent<Animator>().enabled = true;

        // Waits before all of the animators are done showing up animations.
        yield return new WaitForSecondsRealtime(5.5f);

        // Begins the starting sequence.
        GameManager.gameManager.StartGame();
        gameObject.SetActive(false);
    }
}
