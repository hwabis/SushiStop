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

        // Server message PlayerNumber sets this
        public int PlayerNumber;

        // Server message StartRound and NextTurn set this
        public List<Card> Hand;

        // PlayedCard sets this
        public Card PlayedCard;
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

        // Client starts a round. Three rounds per game. The client expects their new starting hand
        StartRoundRequest,
        // Set up and give starting hands to everyone
        StartRound,

        // Client played a card from their hand
        PlayedCard,
        // All players have played a card. Server sends everybody their new hands
        // and what everybody just played
        NextTurn
    }
}
