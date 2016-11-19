using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour {

    public override void OnStartServer()
    {
        RegisterInMatch();
    }

    private void RegisterInMatch()
    {
        MatchManager.instance.RegisterPlayer(this);
    }
}
