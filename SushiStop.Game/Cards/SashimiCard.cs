using osu.Framework.Graphics.Containers;
using SushiStop.Game.Cards.Drawables;

namespace SushiStop.Game.Cards
{
    public class SashimiCard : Card
    {
        public override CompositeDrawable CreateDrawableCard() => new DrawableTempura(this);
    }
}
