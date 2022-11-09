using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osu.Framework.Allocation;
using SushiStop.Game.Screens;

namespace SushiStop.Game.Tests.Visual
{
    [TestFixture]
    public class TestSceneHomeScreen : SushiStopTestScene
    {
        // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
        // You can make changes to classes associated with the tests and they will recompile and update immediately.

        [Cached]
        private ScreenStack screenStack = new ScreenStack { RelativeSizeAxes = Axes.Both };

        public TestSceneHomeScreen()
        {
            screenStack.Push(new HomeScreen());
            Add(screenStack);
        }
    }
}
