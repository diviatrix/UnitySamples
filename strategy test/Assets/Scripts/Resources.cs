﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Resource
{
    gold,
    wood,
    stone,
    food,
    copper,
    citizen,
    maxCitizen
}

[System.Serializable]
public struct Resources
{
    public int gold;
    public int wood;
    public int stone;
    public int food;
    public int copper;
    public int citizen;

    public Resources(int copper,int citizen,int food,int gold,int stone,int wood) 
    {
        this.copper = copper;
        this.citizen = citizen;
        this.food = food;
        this.gold = gold;
        this.stone = stone;
        this.wood = wood;
    }
    
    public static bool operator ==(Resources r1, Resources r2) 
    {
        return r1.Equals(r2);
    }

    public static bool operator !=(Resources r1, Resources r2) 
    {
        return !r1.Equals(r2);
    }
    public static bool operator >=(Resources r1, Resources r2) 
    {
        bool isBigger = false;
        if 
        (
            r1.copper >= r2.copper &&
            r1.citizen >= r2.citizen &&
            r1.food >= r2.food &&
            r1.gold >= r2.gold &&
            r1.stone >= r2.stone &&
            r1.wood >= r2.wood
        )
        {
            isBigger = true;
        }
        return isBigger;
    }
    public static bool operator <=(Resources r1, Resources r2) 
    {
        bool isLess = false;
        if 
        (
            r1.copper <= r2.copper &&
            r1.citizen <= r2.citizen &&
            r1.food <= r2.food &&
            r1.gold <= r2.gold &&
            r1.stone <= r2.stone &&
            r1.wood <= r2.wood
        )
        {
            isLess = true;
        }
        return isLess;
    }   

    public static Resources operator +(Resources r1, Resources r2) 
    {
        return new Resources 
        (
        r1.copper + r2.copper,
        r1.citizen + r2.citizen,
        r1.food + r2.food,
        r1.gold + r2.gold,
        r1.stone + r2.stone,
        r1.wood + r2.wood
        );
    }
    public static Resources operator -(Resources r1, Resources r2) 
    {
        return new Resources 
        (
        r1.copper - r2.copper,
        r1.citizen - r2.citizen,
        r1.food - r2.food,
        r1.gold - r2.gold,
        r1.stone - r2.stone,
        r1.wood - r2.wood
        );
    }
}