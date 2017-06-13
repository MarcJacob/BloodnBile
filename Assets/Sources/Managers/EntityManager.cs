﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EntityManager {

    public List<Entity> Entities = new List<Entity>(); // Ensemble des entités
    public List<Unit> Units = new List<Unit>(); // Ensemble des unités. NOTE : Entities contient Units.
    WeakReference MatchWeakRef; // Le match auquel cet EntityManager est lié.
    public BnBMatch Match { get { return ((BnBMatch)(MatchWeakRef.Target)); } }
    public Entity[] GetAllEntities()
    {
        return Entities.ToArray();
    }

    public EntityManager(BnBMatch match)
    {
        MatchWeakRef = new WeakReference(match);
        Unit.RegisterOnUnitDiedCallback(OnUnitDeath);
    }

    public Unit GetUnitFromID(int ID)
    {
        foreach(Unit u in Units)
        {
            if (u.ID == ID)
            {
                return u;
            }
        }
        return null;
    }


    float EntityPositionsUpdateCooldown = 0.1f;
    float currentEntityPositionUpdateCooldown = 0f;
    public void UpdateEntities()
    {
        List<Entity> DeadEntities = new List<Entity>();
        foreach(Entity e in Entities)
        {
            if (e.Alive)
                e.UpdateEntity();
            else
                DeadEntities.Add(e);
        }

        foreach(Entity e in DeadEntities)
        {
            Entities.Remove(e);
        }

        if (currentEntityPositionUpdateCooldown >= EntityPositionsUpdateCooldown)
        {
            List<EntityPositionRotationUpdate> updateList = new List<EntityPositionRotationUpdate>();
            foreach(Entity e in Entities)
            {
                updateList.Add(new EntityPositionRotationUpdate(e.ID, e.Pos, e.Rot));
            }
            Match.SendMessageToPlayers(12, updateList.ToArray(), true, true);
            currentEntityPositionUpdateCooldown = 0f;
        }
        else
        {
            currentEntityPositionUpdateCooldown += Time.deltaTime;
        }
    }

    public Entity CreateEntity(Vector3 pos, Quaternion rot, string name)
    {
        Debugger.LogMessage("Création d'une entité : " + name);
        Entity ent = new Entity(Match, Entities.Count, pos, rot, name);
        Entities.Add(ent);
        return ent;
    }

    public Unit CreateUnit(Vector3 pos, Quaternion rot, string name, int mesh, float size, Faction fac, float speed, HumorLevels humors) // Surcharge pour les entités de type Unit.
    {
        Debugger.LogMessage("Création d'une unité : " + name);
        Unit newUnit = new Unit(Match, Entities.Count, pos, rot, name, mesh, size, fac, speed, humors);

        OnUnitCreated(newUnit);
        return newUnit;
    }

    public void OnUnitCreated(Unit unit, bool sendNetworkMessage = true)
    {
        Units.Add(unit);
        Entities.Add(unit);
        if (sendNetworkMessage)
        Match.SendMessageToPlayers(10, unit, false, true);
        if (OnUnitCreatedCallback != null)
        {
            OnUnitCreatedCallback(unit);
        }
    }

    Action<Unit> OnUnitCreatedCallback;
    public void RegisterOnUnitCreatedCallback(Action<Unit> cb)
    {
        OnUnitCreatedCallback += cb;
    }

    void OnUnitDeath(Unit unit)
    {
        if (unit.MatchID == Match.ID)
            if (Units.Contains(unit))
            {
                Debugger.LogMessage(unit.Name + " est morte.");
                Match.SendMessageToPlayers(11, unit.ID, false, false);
                Units.Remove(unit);

                if (OnUnitDeathCallback != null)
                {
                    OnUnitDeathCallback(unit);
                }
            }
    }

    Action<Unit> OnUnitDeathCallback;
    public void RegisterOnUnitDeathCallback(Action<Unit> cb)
    {
        OnUnitDeathCallback += cb;
    }
}