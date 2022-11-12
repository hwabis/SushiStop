using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;
using osuTK.Graphics;

namespace SushiStop.Game.Cards.Drawables
{
    public class DrawableChopsticksCard : DrawableCard
    {
        public DrawableChopsticksCard(ChopsticksCard card)
            : base(card, Color4.SkyBlue, "chopsticks", "swap for 2", 1)
        {
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            OnLoad(textures);
        }
    }
}
