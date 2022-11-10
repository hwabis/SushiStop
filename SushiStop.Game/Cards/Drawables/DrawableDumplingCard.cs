using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;
using osuTK.Graphics;

namespace SushiStop.Game.Cards.Drawables
{
    public class DrawableDumplingCard : DrawableCard
    {
        public DrawableDumplingCard(DumplingCard card)
            : base(card, Color4.CornflowerBlue, "dumpling", "1 3 6 10 15", 1)
        {
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            OnLoad(textures);
        }
    }
}
