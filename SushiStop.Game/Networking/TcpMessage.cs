namespace SushiStop.Game.Networking
{
    /// <summary>
    /// The client and the server communicate by sending a JSON serialized string of
    /// an instance of this class and checks TcpMessage.Type on both ends
    /// </summary>
    public class TcpMessage
    {
        public TcpMessageType Type;

        // TcpMessageType.PlayerNumber sets this
        public int PlayerNumber;
    }

    public enum TcpMessageType
    {
        // Client is requesting their player number
        PlayerNumberRequest,
        // Server is giving the client their player number
        PlayerNumber
    }
}
