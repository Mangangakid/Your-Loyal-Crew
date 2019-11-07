using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomSelectionPanel : MonoBehaviour
{
    public Room ActiveRoom;
    private GameManager gm;
    public GameObject Panel;
    public Text RoomTypeText;
    public Image RoomImage;
    public Text PowerText;
    public GameObject WorkersLine;
    public Text WorkersText;
    public GameObject SleepersLine;
    public Text SleepersText;
    public Text RepairmenText;
    
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
    }

    public void OpenSelectionPanel(Room NewRoom)
    {
        if (NewRoom == ActiveRoom)
        {
            CloseRoomSelectionPanel();
            ActiveRoom = null;
        }
        else
        {
            ActiveRoom = NewRoom;
            Panel.SetActive(true);
            RoomTypeText.text = ActiveRoom.Type.ToString();
            RoomImage.sprite = ActiveRoom.RoomThumbnail;
            switch (ActiveRoom.Type)
            {
                case Room.RoomType.Barracks:
                    WorkersLine.SetActive(false);
                    SleepersLine.SetActive(true);
                    break;
                case Room.RoomType.Infirmary:
                    WorkersLine.SetActive(true);
                    SleepersLine.SetActive(true);
                    break;
                default:
                    WorkersLine.SetActive(true);
                    SleepersLine.SetActive(false);
                    break;
            }

        }
    }
    

    public void CloseRoomSelectionPanel()
    {
        Panel.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Panel.activeInHierarchy)
        {
            PowerText.text = ActiveRoom.Power.ToString() + " %";
            WorkersText.text = ActiveRoom.WorkingCharacters.Count.ToString();
            SleepersText.text = ActiveRoom.SleepingCharacters.Count.ToString();
            RepairmenText.text = ActiveRoom.RepairingCharacters.Count.ToString();
        }
    }
}
