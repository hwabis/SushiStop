using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using SushiStop.Game.Screens;

namespace SushiStop.Game
{
    public class SushiStopGame : SushiStopGameBase
    {
        [Cached]
        private ScreenStack screenStack = new ScreenStack { RelativeSizeAxes = Axes.Both };

        [BackgroundDependencyLoader]
        private void load()
        {
            // Add your top-level game components here.
            // A screen stack and sample screen has been provided for convenience, but you can replace it if you don't want to use screens.
            Child = screenStack;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            screenStack.Push(new HomeScreen());
        }
    }
}
