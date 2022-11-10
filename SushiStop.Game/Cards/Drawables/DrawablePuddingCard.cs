using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;
using osuTK.Graphics;

namespace SushiStop.Game.Cards.Drawables
{
    public class DrawablePuddingCard : DrawableCard
    {
        public DrawablePuddingCard(PuddingCard card)
            : base(card, Color4.LightPink, "pudding", "end: 6/-6", 1)
        {
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            OnLoad(textures);
        }
    }
}
