using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;
using osuTK.Graphics;

namespace SushiStop.Game.Cards.Drawables
{
    public class DrawableSashimiCard : DrawableCard
    {
        public DrawableSashimiCard(SashimiCard card)
            : base(card, Color4.YellowGreen, "sashimi", "x3=10", 1)
        {
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            OnLoad(textures);
        }
    }
}
