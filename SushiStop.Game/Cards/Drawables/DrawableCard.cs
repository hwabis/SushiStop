using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK.Graphics;
using static System.Net.Mime.MediaTypeNames;

namespace SushiStop.Game.Cards.Drawables
{
    public abstract class DrawableCard : CompositeDrawable
    {
        public Card Card { get; protected set; }

        private Color4 backgroundColor;
        private string textureName;
        private string descriptionText;
        private int cornerSpriteCount;

        public DrawableCard(Card card, Color4 backgroundColor, string textureName, string descriptionText, int cornerSpriteCount)
        {
            Origin = Anchor.Centre;
            Card = card;

            this.backgroundColor = backgroundColor;
            this.textureName = textureName;
            this.descriptionText = descriptionText;
            this.cornerSpriteCount = cornerSpriteCount;
        }

        /// <summary>
        /// Please call this in your [BackgroundDependencyLoader] method
        /// (I'm not sure if there's a way so I don't have to call this for every DrawableCard)
        /// </summary>
        /// <param name="textures">From your [BackgroundDependencyLoader] method parameter</param>
        protected void OnLoad(TextureStore textures)
        {
            Container cardContainer;
            InternalChild = cardContainer = new CardBackground(backgroundColor);

            Sprite centerSprite = new Sprite
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Texture = textures.Get(textureName)
            };
            float originalRatio = centerSprite.Width / centerSprite.Height;
            centerSprite.Width = cardContainer.Width - 46;
            centerSprite.Height = centerSprite.Width / originalRatio;
            cardContainer.Add(centerSprite);

            FillFlowContainer cornerContainer;
            cardContainer.Add(cornerContainer = new FillFlowContainer
            {     
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                Direction = FillDirection.Vertical,
                X = 2,
                Y = 2,
                AutoSizeAxes = Axes.Both
            });
            for (int i = 0; i < cornerSpriteCount; i++)
            {
                cornerContainer.Add(new Sprite
                {
                    Texture = textures.Get(textureName),
                    Width = cardContainer.Width / 4,
                    Height = cardContainer.Width / 4 / originalRatio
                });
            }

            cardContainer.Add(new Container
            {
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.DarkGoldenrod,
                    },
                    new SpriteText
                    {
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.BottomCentre,
                        Text = descriptionText,
                        Font = FontUsage.Default.With(size: 20)
                    }
                }
            });
        }
    }
}
