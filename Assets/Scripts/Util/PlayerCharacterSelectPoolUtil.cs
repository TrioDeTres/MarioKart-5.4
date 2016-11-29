using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCharacterSelectPoolUtil
{
    private static PlayerCharacterSelectPoolUtil instance;
    private readonly List<PlayerCharacterSelect> players;

    public Action<PlayerCharacterSelect> OnPlayerRegistered;
    public Action<PlayerCharacterSelect> OnLocalPlayerRegistered;

    private PlayerCharacterSelectPoolUtil()
    {
        this.players = new List<PlayerCharacterSelect>();
    }

    public static PlayerCharacterSelectPoolUtil Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerCharacterSelectPoolUtil();
            }
            return instance;
        }
    }

    public PlayerCharacterSelect AddPlayer(PlayerCharacterSelect player)
    {
        players.Add(player);

        if (OnPlayerRegistered != null)
        { 
            OnPlayerRegistered(player);
        }

        PlayerCharacterSelect local = FindLocalPlayer(players);

        if (local != null && OnLocalPlayerRegistered != null)
        {
            OnLocalPlayerRegistered(local);
        }

        return player;
    }

    public void Clear()
    {
        players.Clear();
    }

    public List<PlayerCharacterSelect> GetPlayers()
    {
        return players;
    }

    public PlayerCharacterSelect FindLocalPlayer(List<PlayerCharacterSelect> players)
    {
        return players.FirstOrDefault(p => p.isLocalPlayer);
    }

    public PlayerCharacterSelect FindLocalPlayer()
    {
        return FindLocalPlayer(GetPlayers());
    }
}
