﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faction : MonoBehaviour {
    
    string FactionName;
    int FactionColor = 0;  // 0 = no faction, 1 = red, 2 = blue (ceci est un exemple en vrai on s'en bat les couilles des couleurs)

    Faction(string name, int color)
    {
        FactionName = name;
        FactionColor = color;
    }

    public bool equals(Faction faction)
    {
        if (this.FactionName == faction.FactionName) return true;
        else return false;
    }
	
	void Start () {
		
	}
	
	
	void Update () {
		
	}
}