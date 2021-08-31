using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum RACE
{
    MORTAL,
    BONE,
    GHOST,
    DEMON,
    VOID
}

public enum CLASS
{
    NONE,
    SAMURAI,
    NINJA,
    MONK
}

[Serializable]
public struct Status 
{
    public RaceStatus raceStatus;
    public CLASS jobClass;

    public Status(RaceStatus race, CLASS job)
    {
        raceStatus = race;
        jobClass = job;
    }
}

[Serializable]

public struct RaceStatus
{
    public string name;
    public RACE race;
    public float STR; // Controls damage
    public float DEX; // Controls projectile speed, attack cooldown
    public float INT; // Controls projectile range
    public Color weaponColor;

    public RaceStatus(string name, RACE race, float STR, float DEX, float INT, Color color)
    {
        this.name = name;
        this.race = race;
        this.STR = STR;
        this.DEX = DEX;
        this.INT = INT;
        this.weaponColor = color;
    }
}

public static class RaceConfig{
    public static RaceStatus BONE = new RaceStatus("BONE", RACE.BONE, 0.8f, 2.0f, 1.5f, Color.white);
    public static RaceStatus GHOST = new RaceStatus("GHOST", RACE.GHOST, 1.5f, 1.0f, 1.8f, Color.blue);
    public static RaceStatus DEMON = new RaceStatus("DEMON", RACE.DEMON, 2.0f, 1.5f, 1.0f, Color.red);
    public static RaceStatus VOID = new RaceStatus("VOID", RACE.VOID, 2.0f, 2.0f, 1.8f, Color.magenta);
    public static RaceStatus MORTAL = new RaceStatus("MORTAL", RACE.MORTAL, 1.0f, 1.0f, 1.0f, Color.green);

    public static RaceStatus GetRaceStatus(RACE race)
    {
        switch (race)
        {
            case RACE.BONE:
                return BONE;
            case RACE.DEMON:
                return DEMON;
            case RACE.VOID:
                return VOID;
            case RACE.MORTAL:
                return MORTAL;
        }
        return MORTAL;
    }
}
