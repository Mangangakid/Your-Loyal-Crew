using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    [Header("Attributes")]
    public string characterName;
    public float HitPoints = 100f;
    public float Energy=100f;
    public int Strenght = 0; //Weapons
    public int Dextrecity = 0; //Engines
    public int Endurance = 0; //Shields
    public int Perception = 0; //Navigation
    public int Intelligence = 0; //Medicine
    public Sprite Thumbnail;
    [Space]
    public GameObject selectionArrow;
    public bool selected = false;
    public int Job = 0; //0:none,1:Work,2:Sleep,3:Repair
    public Room boundedRoom;
    private NavMeshAgent agent;
    private GameManager gm;
    private SelectionPanel selectionPanel;
    public int CharacterId;

    // Sets up the character when is loaded
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        gm = GameManager.instance;
        selectionPanel = gm.gameObject.GetComponent<SelectionPanel>();
        gm.AddCharacter(this);
        CharacterId = gm.Characters.Count;
    }
    private void Update()
    {
        if (Job == 2)
        {
            RecoverEnergy();
            RecoverHP();
        }
        else
        {
            EnergyLoss();
        }
    }
    public void RecoverHP()
    {
        if (boundedRoom.Type == Room.RoomType.Infirmary)
        {
            if (HitPoints<100f)
            {
                HitPoints += boundedRoom.Power / gm.HitPointsRecoveryDelay * Time.deltaTime;
            }
            else
            {
                HitPoints = 100f;
                GoIdle();
            }
            
        }
    }
    public void RecoverEnergy()
    {
        if (boundedRoom.Type == Room.RoomType.Infirmary)
        {
            if (Energy < 100f)
            {
                Energy += boundedRoom.Level / gm.EnergyRecoveryDelay * Time.deltaTime;
            }
            if (Energy > 100f)
            {
                Energy = 100f;
            }
        }
        else
        {
            if (Energy < 100f)
            {
                Energy += 0.3f * boundedRoom.Level / gm.EnergyRecoveryDelay * Time.deltaTime;
            }
            else
            {
                Energy = 100f;
                GoIdle();
            }
        }
    }
    public void EnergyLoss()
    {
            Energy -= (gm.IdleEnergyLoss +gm.WorkingEnergyLoss*Job)/ (40+Endurance) * Time.deltaTime;
    }
    // Selects or Unselects the character
    public void Select(bool value)
    {
        selected = value;
        selectionArrow.SetActive(value);
        gm.SelectedCharacter = CharacterId;
        selectionPanel.OnCharacterSelected(this);
    }

    //Sets the NavMeshAgent to move the character to the target
    public void MoveTo(Vector3 target)
    {
        agent.SetDestination(target);
    }

    public void GoIdle()
    {
        MoveTo(gm.IdlePosition());
        gm.SetBond(this, null, 0);
    }

}
