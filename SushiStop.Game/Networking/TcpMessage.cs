﻿using System.Collections.Generic;
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

        // Server messages Startround and NextTurn sets this
        // (client needs to see every Player to see their PlayedCards)
        public List<Player> Players;
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

        // Client played a card from their hand (or two for chopsticks),
        // and sends their Player instance object to the server
        EndTurn,
        // All players have played a card. Server sends everybody a list of every Player
        // (1. their new hands and 2. the updated list of played cards of every player)
        NextTurn
    }
}
