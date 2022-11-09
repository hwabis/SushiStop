using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;
using osuTK.Graphics;

namespace SushiStop.Game.Cards.Drawables
{
    public class DrawableTempura : CompositeDrawable, IDrawableCard
    {
        public Card Card => Card.Tempura;

        private Container cardContainer;
        private Sprite sprite;

        public DrawableTempura()
        {
            Origin = Anchor.Centre;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            InternalChild = cardContainer = new CardBackground(Color4.Purple);

            sprite = new Sprite
            {
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                Texture = textures.Get("tempura")
            };
            float originalRatio = sprite.Width / sprite.Height;
            sprite.Width = cardContainer.Width - 10;
            sprite.Height = sprite.Width / originalRatio;
            sprite.Y = 5;

            cardContainer.Add(sprite);
        }
    }
}
