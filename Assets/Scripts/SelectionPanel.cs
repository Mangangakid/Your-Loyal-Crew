using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionPanel : MonoBehaviour
{
    public GameObject ThePanelItself;
    public Image Thumbnail;
    public Text Name;
    public Text Task;
    public Text Strenght;
    public Text Dextrecity;
    public Text Endurance;
    public Text Perception;
    public Text Intelligence;
    public RectTransform HitPointsBar;
    public RectTransform EnergyBar;
    public Character ActualCharacter;
    private GameManager gm;

    public void OnUnselection()
    {
        ActualCharacter = null;
        ThePanelItself.SetActive(false);
    }

    public void OnCharacterSelected(Character SelectedCharacter)
    {
        Thumbnail.sprite = SelectedCharacter.Thumbnail;
        Name.text = SelectedCharacter.characterName;
        Strenght.text = SelectedCharacter.Strenght.ToString();
        Dextrecity.text = SelectedCharacter.Dextrecity.ToString();
        Endurance.text = SelectedCharacter.Endurance.ToString();
        Perception.text = SelectedCharacter.Perception.ToString();
        Intelligence.text = SelectedCharacter.Intelligence.ToString();
        ActualCharacter = SelectedCharacter;
        ThePanelItself.SetActive(true);
    }
    private void Start()
    {
        gm = GameManager.instance;
    }
    private void Update()
    {
        ShowHitPoints();
        ShowEnergy();
        ShowTask();
    }

    private void ShowEnergy()
    {
        if (ActualCharacter != null)
        {
            EnergyBar.localScale = new Vector3(1f,ActualCharacter.Energy / 100f, 1f);
        }
    }

    private void ShowHitPoints()
    {
        if (ActualCharacter != null)
        {
            HitPointsBar.localScale = new Vector3(1f,ActualCharacter.HitPoints / 100f, 1f);
        }
    }
    private void ShowTask()
    {
        if (ActualCharacter != null)
        {
            string _task = "";
            switch (ActualCharacter.Job)
            {
                case 0:
                    _task = "Idle";
                    break;
                case 1:
                    switch (ActualCharacter.boundedRoom.Type)
                    {
                        case Room.RoomType.Barracks:
                            _task = "ERROR! wrong job!";
                            break;
                        case Room.RoomType.Infirmary:
                            _task = "Medic";
                            break;
                        case Room.RoomType.Bridge:
                            _task = "Pilot";
                            break;
                        case Room.RoomType.Core:
                            _task = "Core Engineer";
                            break;
                        case Room.RoomType.Engines:
                            _task = "Engineer";
                            break;
                        case Room.RoomType.Fields:
                            _task = "Defense Unit";
                            break;
                        case Room.RoomType.Weapons:
                            _task = "Gunner";
                            break;
                    }
                    break;
                case 2:
                    switch (ActualCharacter.boundedRoom.Type)
                    {
                        case Room.RoomType.Barracks:
                            _task = "Sleeping";
                            break;
                        case Room.RoomType.Infirmary:
                            _task = "Healing";
                            break;
                    }
                    break;
                case 3:
                    _task = "Repairing";
                    break;
            }
            Task.text = _task;
        }  
    }
}
