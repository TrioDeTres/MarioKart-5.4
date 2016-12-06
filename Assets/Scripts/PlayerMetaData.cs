public class PlayerMetadata
{
    public int connectionId;
    public string playername;
    public YoshiSkin skin;
    public int coinCount;
    public float trackTimer;
    public float trackTimerWithCoins;
    public int endPosition;

    public PlayerMetadata(int connectionId, string playername, YoshiSkin skin = 0)
    {
        this.connectionId = connectionId;
        this.playername = playername;
        this.skin = skin;
    }
}
