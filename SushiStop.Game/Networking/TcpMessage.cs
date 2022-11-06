namespace SushiStop.Game.Networking
{
    /// <summary>
    /// The client and the server communicate by sending a JSON serialized string of
    /// an instance of this class TcpMessage.
    /// </summary>
    public class TcpMessage
    {
        public TcpMessageType Type;

        // TcpMessageType.PlayerNumber sets this
        public int PlayerNumber;
    }

    public enum TcpMessageType
    {
        // Client requests their player number when joining lobby
        PlayerNumberRequest,
        // Server gives the client their player number
        PlayerNumber
    }
}
