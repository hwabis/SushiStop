using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;

namespace SushiStop.Game.Cards.Drawables
{
    public class CardBackground : Container
    {
        private Box background;
        private Color4 color;

        private const int height = 136;
        private const int width = 92;

        public CardBackground(Color4 color)
        {
            Origin = Anchor.Centre;
            Masking = true;
            CornerRadius = 5;
            Height = height;
            Width = width;
            BorderColour = Color4.Brown;
            BorderThickness = 2;

            this.color = color;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChild = background = new Box
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Height = height,
                Width = width
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            background.Colour = color;
        }
    }
}
