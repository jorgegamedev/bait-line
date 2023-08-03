using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Rewired;

/// <summary>
/// Keeps track of a Rewired Player Reference to be reused through multiple scripts.
/// </summary>
public static class GamePlayer {

    private static EventSystem eventSystem;

    /// <summary>
    /// Returns the player that's being used by Rewired.
    /// </summary>
    public static Player GetPlayer()
    {
        return ReInput.players.GetPlayer(0);
    }

    /// <summary>
    /// Checks if the player is currently using the mouse.
    /// </summary>
    public static bool IsMouse()
    {
        // Gets the controller type from the player.
        ControllerType controllerType = ReInput.controllers.GetLastActiveControllerType();

        // Checks the controller used.
        if (controllerType == ControllerType.Mouse)
        {
            return false;
        }
        else if (controllerType == ControllerType.Joystick || controllerType == ControllerType.Keyboard)
        {
            return true;
        }

        // Failsafe. Code shouldn't reach here.
        return true;
    }

    /// <summary>
    /// Checks if the player is currently using a controller.
    /// </summary>
    public static bool IsController()
    {
        // Gets the controller type from the player.
        ControllerType controllerType = ReInput.controllers.GetLastActiveControllerType();

        // Checks the controller used.
        if (controllerType == ControllerType.Mouse || controllerType == ControllerType.Keyboard)
        {
            return false;
        }
        else if (controllerType == ControllerType.Joystick)
        {
            return true;
        }

        // Failsafe. Code shouldn't reach here.
        return true;
    }

    /// <summary>
    /// Toggles the currently being used event system as on and off.
    /// </summary>
    public static void ToggleEventSystem(bool toggle)
    {
        if (eventSystem == null)
        {
            eventSystem = EventSystem.current;
        }

        // Toggles it accordingly.
        eventSystem.enabled = toggle;
    }

}
