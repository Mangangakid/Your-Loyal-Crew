using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Public Variables")]
    public GameObject[] Workstations = new GameObject[5];
    public GameObject[] Repairs = new GameObject[3];
    public GameObject[] Beds = new GameObject[5];
    public RoomType Type = RoomType.Bridge;
    public int Power = 0;
    public float Integrity=100;
    public int Level = 1;
    public List<Character> WorkingCharacters = new List<Character>();
    public List<Character> SleepingCharacters = new List<Character>();
    public List<Character> RepairingCharacters = new List<Character>();

    private GameManager gm;
    public int RoomId;
    public Animator Anim;


    public enum RoomType
    {
        Barracks,
        Infirmary,  
        Bridge,     
        Weapons,    
        Fields,     
        Core,        
        Engines     
    }

    private void Start()
    {
        gm = GameManager.instance;
        Anim = gameObject.GetComponent<Animator>();
        SetLevel(Level);
        gm.AddRoom(this);
        RoomId = gm.Rooms.Count;
    }

    //Opens the room panel loading this room in it
    public void OnRoomTouched()
    {
        gm.GetComponent<RoomPanel>().OpenRoomPanel(this);
    }
    public void OnRoomSelected()
    {
        Debug.Log("Hey! I'm a room and I've been selected! Yeyy!!!");
    }
    private void Update()
    {
        Repairing();
        Working();
    }

    //Shows the right ammount of assets inside the room depending on it's tipe
    public void SetLevel(int NewLevel)
    {
        Level = NewLevel;
        switch (Type)
        {
            case RoomType.Barracks:
                switch (Level)
                {
                    case 1:
                        Beds[3].SetActive(false);
                        Beds[4].SetActive(false);
                        break;
                    case 2:
                        Beds[3].SetActive(true);
                        Beds[4].SetActive(false);
                        break;
                    case 3:
                        Beds[3].SetActive(true);
                        Beds[4].SetActive(true);
                        break;
                }
                break;
            case RoomType.Infirmary:
                switch (Level)
                {
                    case 1:
                        Beds[3].SetActive(false);
                        Beds[4].SetActive(false);
                        Workstations[3].SetActive(false);
                        Workstations[4].SetActive(false);
                        break;
                    case 2:
                        Beds[3].SetActive(true);
                        Beds[4].SetActive(false);
                        Workstations[3].SetActive(true);
                        Workstations[4].SetActive(false);
                        break;
                    case 3:
                        Beds[3].SetActive(true);
                        Beds[4].SetActive(true);
                        Workstations[3].SetActive(true);
                        Workstations[4].SetActive(true);
                        break;
                }
                break;
            default:
                switch (Level)
                {
                    case 1:
                        Workstations[3].SetActive(false);
                        Workstations[4].SetActive(false);
                        break;
                    case 2:
                        Workstations[3].SetActive(true);
                        Workstations[4].SetActive(false);
                        break;
                    case 3:
                        Workstations[3].SetActive(true);
                        Workstations[4].SetActive(true);
                        break;
                }
                break;
        }
    }

    //Effect of Characters repairing the room
    private void Repairing()
    {
        if ((Integrity < 100f) && (RepairingCharacters.Count > 0))
        {
            Integrity += RepairingCharacters.Count * Time.deltaTime*0.3f;
            if (Integrity >= 100f)
            {
                Integrity = 100f;
                DismissRepairs();
            }
        }
    }

    //Effect of Characters working in the room
    private void Working()
    {

        if (WorkingCharacters.Count > 0)
        {
            Power = 0;
            switch (Type)
            {
                case RoomType.Barracks:
                    Debug.Log("ERROR: characters cannot work in the barracks!");
                    break;
                case RoomType.Infirmary:
                    foreach(Character Item in WorkingCharacters)
                    {
                        Power += Item.Intelligence;
                    }
                    break;
                case RoomType.Bridge:
                    foreach (Character Item in WorkingCharacters)
                    {
                        Power += Item.Perception;
                    }
                    break;
                case RoomType.Core:
                    foreach (Character Item in WorkingCharacters)
                    {
                        Power += Item.Intelligence;
                    }
                    break;
                case RoomType.Engines:
                    foreach (Character Item in WorkingCharacters)
                    {
                        Power += Item.Dextrecity;
                    }
                    break;
                case RoomType.Fields:
                    foreach (Character Item in WorkingCharacters)
                    {
                        Power += Item.Endurance;
                    }
                    break;
                case RoomType.Weapons:
                    foreach (Character Item in WorkingCharacters)
                    {
                        Power += Item.Strenght;
                    }
                    break;
            }
        }
    }

    //Clears the repairing action from the repairing characters and the room
    public void DismissRepairs()
    {
        int rcCount = RepairingCharacters.Count - 1;
        for (int i = rcCount; i > -1; i--)
        {
            RepairingCharacters[i].GoIdle();
        }
    }
    
}