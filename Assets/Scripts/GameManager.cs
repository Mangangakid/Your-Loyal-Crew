using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
    public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    public List<Character> Characters = new List<Character>();
    public List<Room> Rooms = new List<Room>();
    public List<Bond> Bonds = new List<Bond>();
    public Transform[] IdleSpots;
    public int NextSpot;
    public int SelectedCharacter=0;
    public SelectionPanel selectionPanel;
    [Space]
    [Header("Delays")]
    [Range(0.1f, 5f)]
    public float EnergyRecoveryDelay = 1;
    [Range(40f, 120f)]
    public float HitPointsRecoveryDelay = 60f;
    [Range(0.05f, 1f)]
    public float IdleEnergyLoss = 0.1f;
    [Range(0.5f, 10f)]
    public float WorkingEnergyLoss = 1f;


    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        InitGame();
    }

    //Initializes the game for each level.
    void InitGame()
    {
        NextSpot = 0;
        selectionPanel = gameObject.GetComponent<SelectionPanel>();
    }

    public void UnselectAll()
    {
        foreach (Character item in Characters)
        {
            item.Select(false);
        }
        SelectedCharacter = 0;
        selectionPanel.OnUnselection();
    }
    public void AddCharacter(Character newItem)
    {
        Characters.Add(newItem);
        Bonds.Add(new Bond(newItem,null,0)); //Creates null bonds
    }
    public void AddRoom(Room newItem)
    {
        Rooms.Add(newItem);
    }
    public Vector3 IdlePosition()
    {
        Vector3 output = IdleSpots[NextSpot].position;
        NextSpot++;
        if (NextSpot > 9)
        {
            NextSpot = 0;
        }
        return output;
    }
    public void SetBond(Character _character,Room _room, int _activity)
    {
        Bonds[_character.CharacterId-1] = new Bond(_character, _room,_activity);
        UpdateRoomActivity(_character.boundedRoom,_character.Job);
        UpdateRoomActivity(_room, _activity);
        _character.boundedRoom = _room;
        _character.Job = _activity;
        if (_room != null)
        {
            _room.Anim.SetTrigger("Selected");
        }
    }
    private void UpdateRoomActivity(Room _room, int _activity)
    {
        List<Character> auxList = new List<Character>();
        auxList.Clear();
        foreach (Bond item in Bonds)
        {
            if ((item.room == _room) && (item.activity == _activity))
            {
                auxList.Add(item.character);
            }
        }
        if (_room != null)
        {
            switch (_activity)
            {
                case 0:
                    break;
                case 1:
                    _room.WorkingCharacters = auxList;
                    break;
                case 2:
                    _room.SleepingCharacters = auxList;
                    break;
                case 3:
                    _room.RepairingCharacters = auxList;
                    break;
            }
        }
    }
}
