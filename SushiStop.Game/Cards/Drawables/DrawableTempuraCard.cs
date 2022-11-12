using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;
using osuTK.Graphics;

namespace SushiStop.Game.Cards.Drawables
{
    public class DrawableTempuraCard : DrawableCard
    {
        public DrawableTempuraCard(TempuraCard card)
            : base(card, Color4.MediumPurple, "tempura", "x2=5", 1)
        {
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            OnLoad(textures);
        }
    }
}
