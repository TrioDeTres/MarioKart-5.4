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
    public PlayerCharacterSelect GetPlayerByName(string p_name)
    {
        foreach (PlayerCharacterSelect __player in players)
            if (__player.playerName == p_name)
                return __player;
        return null;
    }
    public PlayerCharacterSelect GetPlayerBySkin(int p_skin)
    {
        foreach (PlayerCharacterSelect __player in players)
            if ((int)__player.selectedSkin == p_skin)
                return __player;
        return null;
    }
    public PlayerCharacterSelect FindLocalPlayer(List<PlayerCharacterSelect> players)
    {
        return players.FirstOrDefault(p => p.isLocalPlayer);
    }

    public PlayerCharacterSelect FindLocalPlayer()
    {
        return FindLocalPlayer(GetPlayers());
    }

    public void Clean()
    {
        instance = null;
    }
}
