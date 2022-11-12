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

        // PlayedCard sets this (if Count == 2 then the server knows to move a chopstick card
        // from played cards to hand)
        public List<Card> PlayedCards;
    }

    public enum TcpMessageType
    {
        // Client is requesting their player number
        PlayerNumberRequest,
        // Server is giving the client their player number
        PlayerNumber,

        // Client is requesting to start game (round 1)
        StartGameRequest,
        // Server tells everyone to go to play screen
        StartGame,

        // Client starts a round. Three rounds per game. The client expects their new starting hand
        StartRoundRequest,
        // Set up and give starting hands to everyone
        StartRound,

        // Client played a card from their hand. Could also be PlayedCards (for chopsticks),
        // but in most cases, just one card is played.
        PlayedCard,
        // All players have played a card. Server sends everybody their new hands
        // and what everybody just played
        NextTurn
    }
}
