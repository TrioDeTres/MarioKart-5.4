public class PlayerMetadata
{
    public int connectionId;
    public string playername;
    public YoshiSkin skin;

    public PlayerMetadata(int connectionId, string playername, YoshiSkin skin = 0)
    {
        this.connectionId = connectionId;
        this.playername = playername;
        this.skin = skin;
    }
}
