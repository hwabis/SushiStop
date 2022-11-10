using osu.Framework.Graphics.Containers;
using SushiStop.Game.Cards.Drawables;

namespace SushiStop.Game.Cards
{
    public class DumplingCard : Card
    {
        public override CompositeDrawable CreateDrawableCard() => new DrawableDumplingCard(this);
    }
}
