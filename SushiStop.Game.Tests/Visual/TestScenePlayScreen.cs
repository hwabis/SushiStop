using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osu.Framework.Allocation;
using SushiStop.Game.Screens;
using SushiStop.Game.Cards;
using System.Collections.Generic;

namespace SushiStop.Game.Tests.Visual
{
    [TestFixture]
    public class TestScenePlayScreen : SushiStopTestScene
    {
        [Cached]
        private ScreenStack screenStack = new ScreenStack { RelativeSizeAxes = Axes.Both };
        private PlayScreen playScreen;

        public TestScenePlayScreen()
        {
            screenStack.Push(playScreen = new PlayScreen(null, 3));
            Add(screenStack);

            playScreen.Players = new List<Player>()
            {
                new Player
                {
                    Number = 1,
                    PlayedCards = new List<Card>()
                    {
                        new SashimiCard(),
                        new TempuraCard()
                    }
                },
                new Player
                {
                    Number = 2,
                    PlayedCards = new List<Card>()
                    {
                        new WasabiCard(),
                        new NigiriCard(3),
                        new MakiRollCard(2)
                    }
                },
                new Player
                {
                    Number = 3,
                    PlayedCards = new List<Card>()
                    {
                        new WasabiCard(),
                        new ChopsticksCard(),
                        new NigiriCard(3),
                        new MakiRollCard(2)
                    }
                },
                new Player
                {
                    Number = 4,
                    PlayedCards = new List<Card>()
                    {
                        new WasabiCard(),
                        new NigiriCard(3),
                        new MakiRollCard(2)
                    }
                },
                 new Player
                {
                    Number = 5,
                    PlayedCards = new List<Card>()
                    {
                        new WasabiCard(),
                        new NigiriCard(3),
                        new MakiRollCard(3),
                        new PuddingCard(),
                        new TempuraCard(),
                        new SashimiCard(),
                        new MakiRollCard(1),
                        new NigiriCard(2),
                        new ChopsticksCard(),
                        new DumplingCard()
                    }
                }
            };
            playScreen.Player.Hand = new List<Card>()
            {
                // Can't fit all 12 here or else the FillFlowContainer will mask away the edge cards
                new TempuraCard(),
                new SashimiCard(),
                new DumplingCard(),
                // new MakiRollCard(2),
                // new MakiRollCard(3),
                new MakiRollCard(1),
                new NigiriCard(2),
                new NigiriCard(3),
                new NigiriCard(1),
                new PuddingCard(),
                new WasabiCard(),
                new ChopsticksCard()
            };
            playScreen.CreateDrawablePlayedCards();
            playScreen.CreateDrawableHand();
        }
    }
}
