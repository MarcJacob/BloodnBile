﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Entity  {

    public int ID { get; private set; }
    public SerializableVector3 Pos { get; protected set; }
    public SerializableQuaternion Rot { get; protected set; }
    public string Name { get; protected set; }
    public int MatchID { get; private set; } // Match dans lequel cette entité se trouve.

    public Entity(int matchID, int ID, Vector3 pos, Quaternion rot, string name)
    {
        this.ID = ID;
        Pos = pos;
        Rot = rot;
        Name = name;
        MatchID = matchID;
    }

    public override bool Equals(object u)
    {
        if (u == null)
        {
            return false;
        }
        Entity e = (Entity)u;
        if (e == null)
        {
            return false;
        }
        if (this.ID == e.ID && this.MatchID == e.MatchID) return true;
        else return false;
    }

    public bool Alive = true;
    virtual public void Die()
    {
        Alive = false;
    }

    public virtual void UpdateEntity (float deltaTime)
    {

    }

    public void SetPos(Vector3 vect)
    {
        this.Pos = vect;
    }

    public virtual void SetRot(Quaternion quat)
    {
        Rot = quat;
    }


}
