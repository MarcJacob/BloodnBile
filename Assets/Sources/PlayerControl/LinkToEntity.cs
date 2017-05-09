﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cette classe permet de "lier" un GameObject à une entité d'un certain identifiant contenu dans l'objet EntityRenderer.
/// Plus précisement, cette classe force le GameObject à conserver la position que lui dicte le serveur, tout en étant libre de ses mouvements
/// (côté client) entre deux de ces mises à jours. Si le client et le serveur sont synchronisés alors le mouvement du GameObject devrait
/// correspondre à celui de l'entitée liée côté serveur et donc avoir un déplacement fluide.
/// </summary>
public class LinkToEntity : MonoBehaviour {

    public Entity LinkedEntity;

    public void LinkEntity(Entity e)
    {
        LinkedEntity = e;
        OnEntityPositionUpdated(e);
    }

    public void Initialize(Entity e, EntityRenderer renderer)
    {
        LinkEntity(e);
        renderer.RegisterOnUnitPositionUpdatedCallback(OnEntityPositionUpdated);
        renderer.RegisterOnUnitRemovedCallback(OnEntityDied);
        Initialized = true;
    }

    public void OnEntityDied(Entity e)
    {
        if (e.Equals(LinkedEntity))
        {
            Debug.Log("Mort de l'entité " + e.Name);
            Destroy(gameObject);
        }
    }

    public void OnEntityPositionUpdated(Entity unit)
    {
        if (LinkedEntity == unit)
        {
            transform.position = unit.Pos;
        }
    } 

    bool Initialized = false;

    void Update()
    {
        if (Initialized)
        {

        }
    }
}