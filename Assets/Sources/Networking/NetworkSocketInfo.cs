﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public struct NetworkSocketInfo
{
    public int HostID;
    public int ReliableChannelID;
    public int UnreliableChannelID;
    public int FragmentedChannelID;
    public List<int> ConnectionIDs;

    public void RegisterConnectionID(int ID)
    {
        if (!ConnectionIDs.Contains(ID))
        {
            Debug.Log("Connection ID enregitrée : " + ID);
            ConnectionIDs.Add(ID);
        }
    }

    void OnConnectionTerminated(int coID)
    {
        ConnectionIDs.Remove(coID);
    }

    public NetworkSocketInfo(int maxConnections)
    {
        if (NetworkTransport.IsStarted == false)
            NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();
        ReliableChannelID = cc.AddChannel(QosType.Reliable);
        UnreliableChannelID = cc.AddChannel(QosType.Unreliable);
        FragmentedChannelID = cc.AddChannel(QosType.ReliableFragmented);
        HostTopology ht = new HostTopology(cc, maxConnections);
        HostID = NetworkTransport.AddHost(ht);
        ConnectionIDs = new List<int>();
        NetworkListener.RegisterOnDisconnectionCallback(OnConnectionTerminated);
        NetworkListener.RegisterOnConnectionCallback(RegisterConnectionID);
    }

    public NetworkSocketInfo(int maxConnections, int Port)
    {
        if (NetworkTransport.IsStarted == false)
            NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();
        ReliableChannelID = cc.AddChannel(QosType.Reliable);
        UnreliableChannelID = cc.AddChannel(QosType.Unreliable);
        FragmentedChannelID = cc.AddChannel(QosType.ReliableFragmented);
        HostTopology ht = new HostTopology(cc, maxConnections);
        HostID = NetworkTransport.AddHost(ht, Port);
        ConnectionIDs = new List<int>();
        NetworkListener.RegisterOnDisconnectionCallback(OnConnectionTerminated);
        NetworkListener.RegisterOnConnectionCallback(RegisterConnectionID);
    }

    public bool Connect(string IP, int Port)
    {
        byte error;
        int coID = NetworkTransport.Connect(HostID, IP, Port, 0, out error);
        if (error == 0)
        {
            Debug.Log("Demande de connection envoyée !");
            return true;
        }
        else
        {
             Debug.Log("Erreur lors de l'envoie de demande de connection ! Type d'erreur : " + (NetworkError)error);
            return false;
        }
    }

    public bool IsConnected()
    {
        return ConnectionIDs.Count > 0;
    }
    public bool Disconnect(int coIndex)
    {
        Debug.Log("Fermeture de la connexion ID : " + ConnectionIDs[coIndex]);
        byte error;
        NetworkTransport.Disconnect(HostID, ConnectionIDs[coIndex], out error);
        if ((NetworkError)error != NetworkError.Ok)
        {
            Debug.Log("Erreur lors de la déconnexion ! Type d'erreur : " + (NetworkError)error);
            return false;
        }
        else
        {
            ConnectionIDs.RemoveAt(coIndex);
            return true;
        }
    }
}