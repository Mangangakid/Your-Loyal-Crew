using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    [Header("Attributes")]
    public string characterName;
    [Space]
    public GameObject selectionArrow;
    public bool selected = false;
    private NavMeshAgent agent;

    // Sets up the character when is loaded
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Selects or Unselects the character
    public void Select(bool value)
    {
        selected = value;
        selectionArrow.SetActive(value);
    }

    //Sets the NavMeshAgent to move the character to the target
    public void MoveTo(Vector3 target)
    {
        agent.SetDestination(target);
    }
}
