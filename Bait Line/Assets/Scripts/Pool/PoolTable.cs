using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Scriptable Object that holds the different variables that can be generated.
/// </summary>
[CreateAssetMenu(fileName = "New Pool Table", menuName = "Custom/Pool Table", order = 2)]
public class PoolTable : ScriptableObject {

    // List of Variables that can be picked up from the generation.
    public List<PoolVariable> poolVariables = new List<PoolVariable>();
    
    /// <summary>
    /// Used for adding a new variable to this list.
    /// </summary>
    public void AddVariable(PoolVariable newVariable)
    {
        // Adds a new variable to the pool chance.
        if(newVariable.variable != null)
        {
            poolVariables.Add(newVariable);
        }
        else
        {
            Debug.Log(newVariable.name + " has no variable assigned!");
        }
    }

    /// <summary>
    /// Checks if an action has already been added to the list.
    /// </summary>
    public bool Contains(PoolVariable checkVariable)
    {
        // Checks each variable in the list until it finds the Remove Variable.
        Object checkedObject = checkVariable.variable;
        foreach (PoolVariable variable in poolVariables)
        {
            if (variable.variable == checkedObject)
            {
                return true;
            }
        }

        // Action is not in the list, let's go back.
        return false;
    }

    /// <summary>
    /// Used for removing a variable from the pool.
    /// </summary>
    public void RemoveVariable(GameObject removeVariable)
    {
        // Checks each variable in the list until it finds the Remove Variable.
        foreach(PoolVariable variable in poolVariables)
        {
            if(variable.variable == removeVariable)
            {
                poolVariables.Remove(variable);
                return;
            }
        }
    }
}
