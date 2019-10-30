using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RoomPanel : MonoBehaviour
{
    public GameObject Panel;
    public Text RoomName;
    public GameObject WorkButton;
    public GameObject SleepButton;
    public GameObject HealButton;
    public GameObject RepairButton;
    public RectTransform HealthBar;
    public TouchController MyTouchController;

    private Room ActualRoom;
    private bool ShowRoomPanel = false;
    private GameManager gm;


    // OpenRoomPanel is called when you touch a Room to show the room options
    public void OpenRoomPanel(Room ThisRoom)
    {
        Panel.SetActive(true);
        RoomName.text = ThisRoom.Type.ToString();
        ActualRoom = ThisRoom;
        ShowRoomPanel = true;
        switch (ThisRoom.Type)
        {
            case Room.RoomType.Barracks:
                SleepButton.SetActive(true);
                break;
            case Room.RoomType.Infirmary:
                HealButton.SetActive(true);
                WorkButton.SetActive(true);
                break;
            default:
                WorkButton.SetActive(true);
                break;
        }
    }

    public void CloseRoomPanel()
    {
        ShowRoomPanel = false;
        Panel.SetActive(false);
        WorkButton.SetActive(false);
        SleepButton.SetActive(false);
        HealButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (ShowRoomPanel)
        {
            HealthBar.localScale = new Vector3(ActualRoom.Integrity / 100f, 1, 1);
            if (ActualRoom.RepairingCharacters.Count<3 && ActualRoom.Integrity < 100)
            {
                RepairButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                RepairButton.GetComponent<Button>().interactable = false;
            }
            if (ActualRoom.WorkingCharacters.Count < ActualRoom.Level + 2)
            {
                WorkButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                WorkButton.GetComponent<Button>().interactable = false;
            }
            if (ActualRoom.SleepingCharacters.Count < ActualRoom.Level + 2)
            {
                SleepButton.GetComponent<Button>().interactable = true;
                HealButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                SleepButton.GetComponent<Button>().interactable = false;
                HealButton.GetComponent<Button>().interactable = false;
            }
        }     
    }
    private void Start()
    {
        gm = GameManager.instance;
        ShowRoomPanel = false;
    }
    public void WorkButtonPressed()
    {
        gm.SetBond(gm.Characters[gm.SelectedCharacter - 1], ActualRoom, 1);
        MyTouchController.WalkTo(ActualRoom.Workstations[ActualRoom.WorkingCharacters.Count - 1].GetComponent<RoomObject>().AnimationSpot.position);
        gm.UnselectAll();
        CloseRoomPanel();
    }
    public void SleepButtonPressed()
    {
        gm.SetBond(gm.Characters[gm.SelectedCharacter - 1], ActualRoom, 2);
        MyTouchController.WalkTo(ActualRoom.Beds[ActualRoom.SleepingCharacters.Count - 1].GetComponent<RoomObject>().AnimationSpot.position);
        gm.UnselectAll();
        CloseRoomPanel();
    }
    public void HealButtonPressed()
    {
        gm.SetBond(gm.Characters[gm.SelectedCharacter - 1], ActualRoom, 2);
        MyTouchController.WalkTo(ActualRoom.Beds[ActualRoom.SleepingCharacters.Count - 1].GetComponent<RoomObject>().AnimationSpot.position);
        gm.UnselectAll();
        CloseRoomPanel();
    }
    public void RepairButtonPressed()
    {
        gm.SetBond(gm.Characters[gm.SelectedCharacter - 1], ActualRoom, 3);
        MyTouchController.WalkTo(ActualRoom.Repairs[ActualRoom.RepairingCharacters.Count - 1].GetComponent<RoomObject>().AnimationSpot.position);
        gm.UnselectAll();
        CloseRoomPanel();
    }
}
