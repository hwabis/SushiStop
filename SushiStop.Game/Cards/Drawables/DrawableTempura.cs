using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
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
            // I'm not sure if there's a way to use inheritance so I don't have to copy this for every DrawableCard
            // but then the [BackgroundDependencyLoader] method would be weird... can't pass in the parameters I need
            InternalChild = cardContainer = new CardBackground(Color4.Purple);

            sprite = new Sprite
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Texture = textures.Get("tempura"),
                Y = 10
            };
            float originalRatio = sprite.Width / sprite.Height;
            sprite.Width = cardContainer.Width - 20;
            sprite.Height = sprite.Width / originalRatio;
            cardContainer.Add(sprite);

            cardContainer.Add(new Sprite
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                Texture = textures.Get("tempura"),
                Width = cardContainer.Width / 3,
                Height = cardContainer.Width / 3 / originalRatio,
                X = 2,
                Y = 2
            });

            cardContainer.Add(new SpriteText
            {
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
                Text = "x2=5",
                Font = FontUsage.Default.With(size: 16)
            });
        }
    }
}
