using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;

namespace SushiStop.Game.Screens
{
    public class LobbyScreen : Screen
    {
        private ScreenStack screenStack;

        [BackgroundDependencyLoader]
        private void load(ScreenStack screenStack)
        {
            InternalChild = new SpriteText
            {
                Anchor = Anchor.Centre,
                Text = "yeah boi"
            };

            this.screenStack = screenStack;
        }
    }
}
