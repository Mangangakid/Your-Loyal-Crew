using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    [Header("Values")]
    public float maxEnergy = 100f;
    public float energy = 0f;
    public float engines = 0f;
    public float shields = 0f;
    public float weapons = 0f;
    [Space]
    [Header("UI components")]
    public Transform energyBar;
    public Scrollbar enginesScrollbar;
    public Scrollbar shieldsScrollbar;
    public Scrollbar weaponsScrollbar;
    public int status = 0;
    private float summatory = 0;

    private void Start()
    {
        status = 4;
        summatory = engines + shields + weapons;
        energy = maxEnergy - summatory;
        showAll();
    }

    private void loadValues()
    {
        engines = enginesScrollbar.value * 100f;
        shields = shieldsScrollbar.value * 100f;
        weapons = weaponsScrollbar.value * 100f;
        summatory = engines + shields + weapons;
        energy = maxEnergy - summatory;
    }

    public void ChangeEngines()
    {
        if (status != 4)
        {
            if (status == 0)
            {
                status = 1;
                loadValues();
                if (energy >= 0)
                {
                    showAll();
                }
                else
                {
                    energy = 0;
                    if(status == 1)
                    {
                        float remains = summatory - maxEnergy;
                        if ((shields>=remains /2f) && (weapons >=remains/2f))
                        {
                            shields -= remains / 2f;
                            weapons -= remains / 2f;
                        }
                        else
                        {
                            if(shields < remains / 2f)
                            {
                                weapons -= shields;
                                remains -= shields;
                                weapons -= remains;
                                shields = 0f;
                            }
                            else
                            {
                                shields -= weapons;
                                remains -= weapons;
                                shields -= remains;
                                weapons = 0f;
                            }
                        }
                        showAll();
                    }               
                }

            }

        }
    }

    public void ChangeShields()
    {
        if (status != 4)
        {
            if (status == 0)
            {
                status = 2;
                loadValues();
                if (energy >= 0)
                {
                    showAll();
                }
                else
                {
                    energy = 0;
                    if (status == 2)
                    {
                        float remains = summatory - maxEnergy;
                        if ((engines >= remains / 2f) && (weapons >= remains / 2f))
                        {
                            engines -= remains / 2f;
                            weapons -= remains / 2f;
                        }
                        else
                        {
                            if (engines < remains / 2f)
                            {
                                weapons -= engines;
                                remains -= engines;
                                weapons -= remains;
                                engines = 0f;
                            }
                            else
                            {
                                engines -= weapons;
                                remains -= weapons;
                                engines -= remains;
                                weapons = 0f;
                            }
                        }
                        showAll();
                    }
                }
            }
        }
    }

    public void ChangeWeapons()
    {
        if (status != 4)
        {
            if (status == 0)
            {
                status = 3;
                loadValues();
                if (energy >= 0)
                {
                    showAll();
                }
                else
                {
                    energy = 0;
                    if (status == 3)
                    {
                        float remains = summatory - maxEnergy;
                        if ((shields >= remains / 2f) && (engines >= remains / 2f))
                        {
                            shields -= remains / 2f;
                            engines -= remains / 2f;
                        }
                        else
                        {
                            if (engines < remains / 2f)
                            {
                                shields -= engines;
                                remains -= engines;
                                shields -= remains;
                                engines = 0f;
                            }
                            else
                            {
                                engines -= shields;
                                remains -= shields;
                                engines -= remains;
                                shields = 0f;
                            }
                        }
                        showAll();
                    }
                }
            }
        }
    }

    private void showAll()
    {
        energyBar.localScale = new Vector3(energy / 100f, 1f, 1f);
        shieldsScrollbar.value = shields / 100f;
        enginesScrollbar.value = engines / 100f;
        weaponsScrollbar.value = weapons / 100f;
        status = 0;
    }
}
