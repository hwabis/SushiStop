using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;
using osuTK.Graphics;

namespace SushiStop.Game.Cards.Drawables
{
    public class DrawableWasabiCard : DrawableCard
    {
        public DrawableWasabiCard(WasabiCard card)
            : base(card, Color4.Gold, "wasabi", "x3 next stick", 1)
        {
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            OnLoad(textures);
        }
    }
}
