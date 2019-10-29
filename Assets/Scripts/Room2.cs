using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room2 : MonoBehaviour
{
    [Header("Public Variables")]
    public Transform[] Workstations = new Transform[5];
    public Transform[] Repairs = new Transform[3];
    public Transform[] Beds = new Transform[5];
    public RoomType Type=RoomType.Bridge;
    public int RoomLevel;
    [Space]
    [Header("Models Prefabs")]
    [Space]
    [Header("Workstation Prefabs")]
    public GameObject WorkstationInfirmaryModel;
    public GameObject WorkstationBridgeModel;
    public GameObject WorkstationWeaponsModel;
    public GameObject WorkstationFieldsModel;
    public GameObject WorkstationCoreModel;
    public GameObject WorkstationEnginesModel;
    [Space]
    [Header("Bed Prefabs")]
    public GameObject BedBarracksModel;
    public GameObject BedInfirmaryModel;
    [Space]
    [Header("Repair Prefabs")]
    public GameObject RepairModel;
    [Space]
    [Header("Setup Prefabs")]
    public GameObject BarracksSetupModel;
    public GameObject InfirmarySetupModel;
    public GameObject BridgeSetupModel;
    public GameObject WeaponsSetupModel;
    public GameObject FieldsSetupModel;
    public GameObject CoreSetupModel;
    public GameObject EnginesSetupModel;

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
    // Start is called before the first frame update
    void OnEnable()
    {
        InitializeRoom();
    }

    // This initializes the room
    private void InitializeRoom()
    {
        LoadPrefabs(3, RepairModel, Repairs);
        switch (Type)
        {
            case RoomType.Barracks:
                LoadPrefabs(2 + RoomLevel, BedBarracksModel, Beds);
                break;
            case RoomType.Infirmary:
                LoadPrefabs(2 + RoomLevel, WorkstationInfirmaryModel, Workstations); 
                LoadPrefabs(2 + RoomLevel, BedInfirmaryModel, Beds);
                break;
            case RoomType.Bridge:
                LoadPrefabs(2 + RoomLevel, WorkstationBridgeModel, Workstations);
                break;
            case RoomType.Core:
                LoadPrefabs(2 + RoomLevel, WorkstationCoreModel, Workstations);
                break;
            case RoomType.Engines:
                LoadPrefabs(2 + RoomLevel, WorkstationEnginesModel, Workstations);
                break;
            case RoomType.Fields:
                LoadPrefabs(2 + RoomLevel, WorkstationFieldsModel, Workstations);
                break;
            case RoomType.Weapons:
                LoadPrefabs(2 + RoomLevel, WorkstationWeaponsModel, Workstations);
                break;
        }
    }

    private void LoadPrefabs(int ammount, GameObject prefab, Transform[] destination )
    {
        if (prefab == null)
        {
            Debug.Log("Error: El prefab"+prefab.name+"no existe o no está cargado");
        }
        else
        {
            if (destination.Length < ammount)
            {
                Debug.Log("Error: El array de destinos no tiene suficientes elementos o no existe");
            }
            else
            {
                for (int i = 0; i < ammount; i++)
                {
                    GameObject temporal = Instantiate(prefab);
                    temporal.transform.SetParent(destination[i], false);
                }
            }

        }
    }

    public void OnRoomTouched()
    {
    /*    switch (Type)
        {
            case RoomType.Barracks:
                break;
            case RoomType.Infirmary:
                break;
            default:
                break;
        }*/
    }
}
