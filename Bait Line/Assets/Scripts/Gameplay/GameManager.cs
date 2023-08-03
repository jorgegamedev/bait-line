using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Manages the over all Game State of the game, like the current messages on the stack, and everything else related to game execution.
/// </summary>
public class GameManager : MonoBehaviour {

    // Singleton Reference
    public static GameManager gameManager;

    // Service Locators
    [HideInInspector]
    public TextProcessor textProcessor;
    [HideInInspector]
    public CanvasManager canvasManager;
    [HideInInspector]
    public EndingProcessor endingProcessor;

    // Public Variables/References
    [Header("Game Stats")]
    [ReadOnly]
    public GameState state;
    [ReadOnly]
    [SerializeField]
    private int ratings;
    private bool _isEnding;

    [Header("Default Messages")]
    [SerializeField]
    private Message[] startingMessages;
    [SerializeField]
    private Message[] intermissionMessages;

    [Header("Elements")]
    public GameObject backgroundObject;
    public GameObject doctorGill;

    [Header("Choices")]
    public List<FishCharacter> fishPool;
    [ReadOnly]
    public FishCharacter currentFish;
    [ReadOnly]
    public Dialog currentDialog;
    private FishCharacter _previousFish;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        // Sets this object as the singleton to rule them all.
        if (gameManager == null)
            gameManager = this;

        // Gets any necessary Unity Components.
        textProcessor = GetComponent<TextProcessor>();
        canvasManager = FindObjectOfType<CanvasManager>();
        endingProcessor = GetComponent<EndingProcessor>();
    }

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        state = GameState.Menu;
    }

    /// <summary>
    /// Starts the game itself.
    /// </summary>
    public void StartGame()
    {
        // Sets the Game State as starting and sets up variables.
        MusicManager.musicManager.FadeOutMusic();
        state = GameState.Start;
        ratings = 22;

        // Queues starting messages.
        textProcessor.ShowHideDoctor(true);
        textProcessor.AddMessage(startingMessages);
        textProcessor.DisplayText();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
#if !UNITY_ANDROID  
        if (GamePlayer.GetPlayer().GetButtonDown("UISubmit") && !EventSystem.current.IsPointerOverGameObject())
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject())
#endif
        {
            // Checks if a message is already displaying, as if is waiting for input.
            if (textProcessor.isDisplaying && !textProcessor.isWaitingForInput)
                textProcessor.SkipMessage();

            // Checks if the text processor is still waiting for input.
            if (textProcessor.isWaitingForInput)
            {
                bool nextMessage = textProcessor.NextMessage();

                // If there are no more messages in the stack, we instead check what we should be doing next.
                if(!nextMessage)
                {
                    switch(state)
                    {
                        case GameState.Calling:
                            DisplayChoiceOptions();
                            break;
                        case GameState.Start:
                            StartCoroutine(NextDialog());
                            break;
                        case GameState.Afterwards:
                            StartCoroutine(FishComplete());
                            break;
                        case GameState.Intermission:
                            StartCoroutine(NextDialog());
                            break;
                        case GameState.Ending:
                            state = GameState.Done;
                            endingProcessor.ContinueEnding();
                            break;
                    }
                }
            }
        }

        // Checks controller for controller input for the different choices.
        if(state == GameState.Choice)
        {
            if (GamePlayer.GetPlayer().GetButtonDown("UIChoice1"))
                canvasManager.ForceChoice(0);
            if (GamePlayer.GetPlayer().GetButtonDown("UIChoice2"))
                canvasManager.ForceChoice(1);
            if (GamePlayer.GetPlayer().GetButtonDown("UIChoice3"))
                canvasManager.ForceChoice(2);
        }

        // Checks if the player wants to quit the game.
        if (GamePlayer.GetPlayer().GetButtonDown("UICancel"))
            Application.Quit();
    }

    /// <summary>
    /// Randomly draws the next choice from the choice pool.
    /// </summary>
    public IEnumerator NextDialog()
    {
        // Changes the game state before drawing a new choice.
        state = GameState.Calling;

        // Stores the current fish as the previous fish.
        if(currentFish != null)
            _previousFish = currentFish;

        // Gets a random fish to be the caller. Unless there's only one fish remaining, the fish calling
        // will always be different.
        if(fishPool.Count == 1)
        {
            currentFish = fishPool[0];
        }
        else
        {
            while(currentFish == _previousFish || currentFish == null)
            {
                int randomFish = Random.Range(0, fishPool.Count);
                currentFish = fishPool[randomFish];
            }
        }

        // Gets a random dialog from the fish to be the fish.
        currentDialog = currentFish.ReturnRandomDialog();

        // Prepares and shows the fish pane.
        canvasManager.ShowFishPane();
        yield return new WaitForSeconds(1.25f);

        // Adds the Current Choice to the message stack.
        if (currentFish.audioTrack != null)
            MusicManager.musicManager.PlayMusicTrack(currentFish.audioTrack);
        textProcessor.AddMessage(currentDialog.messages);

        // Begins display
        textProcessor.DisplayText();
    }

    /// <summary>
    /// Displays the different choices on screen that the player can pick.
    /// </summary>
    public void DisplayChoiceOptions()
    {
        state = GameState.Choice;
        canvasManager.DisplayChoices();
    }

    /// <summary>
    /// Called when a choice is picked. Stacks their replying choices on their 
    /// </summary>
    public void PickedChoiceOption(ChoiceType type)
    {
        // Changes the Game State to Afterwards, and hides the choices.
        state = GameState.Afterwards;
        canvasManager.HideChoices();

        // Checks which choices the player has made and queues the messages of that choice.
        switch(type)
        {
            case ChoiceType.Positive:
                ratings += 3;
                ratings = Mathf.Clamp(ratings, 0, 50);
                textProcessor.AddMessage(currentDialog.positiveMessages);
                canvasManager.UpdateRatingsDisplay(ratings, 0);
                break;
            case ChoiceType.Neutral:
                textProcessor.AddMessage(currentDialog.neutralMessages);
                canvasManager.UpdateRatingsDisplay(ratings, -1);
                break;
            case ChoiceType.Negative:
                ratings -= 4;
                ratings = Mathf.Clamp(ratings, 0, 50);
                textProcessor.AddMessage(currentDialog.negativeMessages);
                canvasManager.UpdateRatingsDisplay(ratings, 1);
                break;
        }

        // Displays the final text and updates the current ratings.
        textProcessor.DisplayText();
    }

    /// <summary>
    /// Completes the fish currently shown on screen. 
    /// </summary>
    public IEnumerator FishComplete()
    {
        // Hides the existing fish pane.
        StartCoroutine(canvasManager.HideFishPane());
        yield return new WaitForSeconds(1.5f);

        // Starts the next fish.
        MusicManager.musicManager.FadeOutMusic();

        // Checks if the game should queue an en ending or the intermission.
        // Checks if the player has gotten enough points to get an ending.
        if (ratings <= 0)
        {
            state = GameState.Ending;
            endingProcessor.ProcessBadEnding();
        }
        else if (ratings >= 50)
        {
            state = GameState.Ending;
            endingProcessor.ProcessGoodEnding();
        }
        else if (fishPool.Count == 0)
        {
            state = GameState.Ending;
            endingProcessor.ProcessNeutralEnding();
        }
        else
            Intermission();
    }

    /// <summary>
    /// Used for the intermission between phone conversations.
    /// </summary>
    public void Intermission()
    {
        // Changes the Game State.
        state = GameState.Intermission;

        // Rools a random phrase for the doctor to say between patients!
        int randomInteremission = Random.Range(0, intermissionMessages.Length);
        textProcessor.AddMessage(intermissionMessages[randomInteremission]);

        // Begins display
        textProcessor.DisplayText();
    }
}

/// <summary>
/// Keeps track of the current state of the game.
/// </summary>
public enum GameState
{
    Menu,
    Start,
    Calling,
    Choice,
    Afterwards,
    Intermission,
    Ending,
    Done,
}