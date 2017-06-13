﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class HumorLevels {
    
    public int Blood { get; private set; }
    public int Phlegm { get; private set; }
    public int BlackBile { get; private set; }
    public int YellowBile { get; private set; }

    public HumorLevels(int blood, int phlegm, int black, int yellow)
    {
        Blood = blood;
        Phlegm = phlegm;
        BlackBile = black;
        YellowBile = yellow;
    }


    /// <summary>
    /// Add a certain quantity of a humor.
    /// </summary>
    /// <param name="humor">ID number of the humor : 0-Blood, 1-Phlegm, 2-Black Bile, 3-Yellow Bile</param>
    /// <param name="quantity"></param>
    public void GainHumor(int humor, int quantity)
    {
        switch(humor)
        {
            case 0: Blood += quantity; break;
            case 1: Phlegm += quantity; break;
            case 2: BlackBile += quantity; break;
            case 3: YellowBile += quantity; break;

        }
    }

    /// <summary>
    /// Remove a certain quantity of a humor.
    /// </summary>
    /// <param name="humor">ID number of the humor : 0-Blood, 1-Phlegm, 2-Black Bile, 3-Yellow Bile</param>
    /// <param name="quantity"></param>
    public void LoseHumor(int humor, int quantity)
    {
        switch (humor)
        {
            case 0: Blood -= quantity; break;
            case 1: Phlegm -= quantity; break;
            case 2: BlackBile -= quantity; break;
            case 3: YellowBile -= quantity; break;

        }
    }

    static public HumorLevels operator +(HumorLevels hl1, HumorLevels hl2)
    {
        return new HumorLevels(hl1.Blood + hl2.Blood, hl1.Phlegm + hl2.Phlegm, hl1.YellowBile + hl2.YellowBile, hl1.BlackBile + hl2.BlackBile);
    }

    public override string ToString()
    {
        return "Blood : " + Blood + " Phlegm : " + Phlegm + " Black : " + BlackBile + " Yellow : " + YellowBile;
    }
}

public enum Humor
{
    BLOOD,
    PHLEGM,
    BLACKBILE,
    YELLOWBILE
}