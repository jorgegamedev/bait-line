using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Used for displaying the Cursor in Game.
public class GameCursor : MonoBehaviour {

    [Header("Canvas Render")]
    private Vector2 _mousePixel;

    // Internal References
    private Image _cursorImage;
    private static GameCursor customCursor;

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start()
    {
        // Gets the local references.
        _cursorImage = GetComponent<Image>();
        _mousePixel = Input.mousePosition;
#if UNITY_ANDROID
        Destroy(transform.parent.gameObject);
#else
        if (customCursor == null)
        {
            customCursor = this;
            DontDestroyOnLoad(transform.parent.gameObject);
        }
        else
        {
            Destroy(transform.parent.gameObject);
        }
#endif
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        // Makes sure that the OS Cursor doesn't show.
        if (Cursor.visible)
        {
            Cursor.visible = false;
        }

        // Updates the cursor position.
        UpdateCursorPosition();
    }

    /// <summary>
    /// Updates the cursor RectTransform.
    /// </summary>
    private void UpdateCursorPosition()
    {
        // Gets the mouse in pixel size.
        _mousePixel = Input.mousePosition;

        // Updates the UI to reflect the new Mouse Rect.
        transform.position = _mousePixel;
    }
}
