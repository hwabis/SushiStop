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
            screenStack.Push(playScreen = new PlayScreen(null, 1));
            Add(screenStack);

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
            playScreen.CreateDrawableHand();
        }
    }
}
