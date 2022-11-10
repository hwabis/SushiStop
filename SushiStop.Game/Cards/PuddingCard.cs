using osu.Framework.Graphics.Containers;
using SushiStop.Game.Cards.Drawables;

namespace SushiStop.Game.Cards
{
    public class PuddingCard : Card
    {
        public override CompositeDrawable CreateDrawableCard() => new DrawablePuddingCard(this);
    }
}
