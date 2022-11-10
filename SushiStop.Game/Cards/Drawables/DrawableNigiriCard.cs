using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;
using osuTK.Graphics;

namespace SushiStop.Game.Cards.Drawables
{
    public class DrawableNigiriCard : DrawableCard
    {
        public DrawableNigiriCard(NigiriCard card, int value)
            : base(card, Color4.Gold, getTextureNameFromValue(value), value.ToString(), 1)
        {
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            OnLoad(textures);
        }

        private static string getTextureNameFromValue(int value)
        {
            switch (value)
            {
                case 1:
                    return "nigiri1";
                case 2:
                    return "nigiri2";
                case 3:
                    return "nigiri3";
                default:
                    return "";
            }
        }
    }
}
