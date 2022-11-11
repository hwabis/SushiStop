using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;
using osuTK.Graphics;

namespace SushiStop.Game.Cards.Drawables
{
    public class DrawableMakiRollCard : DrawableCard
    {
        public DrawableMakiRollCard(MakiRollCard card, Action onClick = null)
            : base(card, onClick, Color4.DarkRed, "maki_roll", "most: 6/3", card.Count)
        {
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            OnLoad(textures);
        }
    }
}
