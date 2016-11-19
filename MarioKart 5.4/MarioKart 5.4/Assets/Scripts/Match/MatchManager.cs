using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MatchManager : NetworkBehaviour
{
    public static MatchManager instance;

    [SerializeField]
    private Transform spawnPointGroup;

    private List<Transform> spawnPoints;
    private List<PlayerManager> players;

    public void Awake()
    {
        instance = this;
        players = new List<PlayerManager>();
    }

	public void Start()
	{
        spawnPoints = GetAllChildrenFromTransform(spawnPointGroup);
	    UpdatePlayerPositionToSpawnPoints(spawnPoints, players);
	}

    public void RegisterPlayer(PlayerManager player)
    {
        players.Add(player);
    }

    private void UpdatePlayerPositionToSpawnPoints(List<Transform> spawnPoints, List<PlayerManager> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].transform.position = spawnPoints[i].transform.position;
        }
    }

    private List<Transform> GetAllChildrenFromTransform(Transform transform)
    {
        List<Transform> transforms = new List<Transform>();

        for (int i = 0; i < transform.childCount; i++)
        {
            transforms.Add(transform.GetChild(i));
        }

        return transforms;
    }
}
