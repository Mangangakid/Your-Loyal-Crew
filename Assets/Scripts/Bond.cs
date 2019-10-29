using System.Collections;
using System;
using UnityEngine;

public class Bond : IComparable<Bond>
{
    public Character character;
    public Room room;
    public int activity;
 
    public Bond(Character newCharacter,Room newRoom,int newActivity)
    {
        character = newCharacter;
        room = newRoom;
        activity = newActivity;
    }

  
    public int CompareTo(Bond other)
    {
        if (other == null)
        {
            return 1;
        }

        //Return the difference in power.
        return room.RoomId - other.room.RoomId;
    }
}
