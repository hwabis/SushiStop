using System.Collections.Generic;
using SushiStop.Game.Cards;

namespace SushiStop.Game.Networking
{
    /// <summary>
    /// The client and the server communicate by sending a JSON serialized string of
    /// an instance of this class and checks TcpMessage.Type on both ends
    /// </summary>
    public sealed class TcpMessage
    {
        public TcpMessageType Type;

        // Server message PlayerNumber sets this
        public int PlayerNumber;

        // Client message EndTurn sets this
        public Player Player;

        // Server message NextTurn sets this
        public List<Player> Players;
    }

    public enum TcpMessageType
    {
        // Client is requesting their player number. This sends nothing else
        PlayerNumberRequest,
        // Server is sending the client their player number
        PlayerNumber,

        // Client is requesting to start game (round 1). This sends nothing else
        StartGameRequest,
        // Server tells everyone to go to play screen. This sends nothing else
        StartGame,

        // Client starts a round. This sends nothing else
        // (Three rounds per game.
        // The client expects their new starting hand by expecting NextTurn from Server)
        StartRoundRequest,

        // Client played a card from their hand (or two for chopsticks),
        // and sends their Player instance object to the server
        EndTurn,
        // All players have played a card. Server sends this to everybody and a list of every Player
        // (giving 1. their new hands and 2. the updated list of played cards of every player)
        NextTurn
    }
}
