using System.Collections.Generic;
using SushiStop.Game.Cards;

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

        // TcpMessageType.StartRound sets this
        public List<Card> StartingHand;
    }

    public enum TcpMessageType
    {
        // Client is requesting their player number
        PlayerNumberRequest,
        // Server is giving the client their player number
        PlayerNumber,

        // Client is requesting to start
        StartGameRequest,
        // Server tells everyone to go to play screen
        StartGame,

        // Three rounds per game
        StartRoundRequest,
        // Distribute starting hand to everyone
        StartRound,
    }
}
