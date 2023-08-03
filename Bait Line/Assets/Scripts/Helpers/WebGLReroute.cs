using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Used in WebGL to reroute buttons when they are deleted. 
/// </summary>
public class WebGLReroute : MonoBehaviour {

    [Header("Re-Routes")]
    public Button upNavigation;
    public Button downNavigation;
    public Button leftNavigation;
    public Button rightNavigation;

#if UNITY_WEBGL
	// Use this for initialization
	void Start () {
        // Gets this button as a component.
        Button thisButton = GetComponent<Button>();
        Navigation navigation = thisButton.navigation;

        // For each of the directions checks if there's a button assigned.
        if(upNavigation != null)
        {
            navigation.selectOnUp = upNavigation;
        }
        if (downNavigation != null)
        {
            navigation.selectOnDown = downNavigation;
        }
        if (rightNavigation != null)
        {
            navigation.selectOnRight = rightNavigation;
        }
        if (leftNavigation != null)
        {
            navigation.selectOnLeft = leftNavigation;
        }

        // Assigns it all up!
        thisButton.navigation = navigation;
    }
#endif
}
