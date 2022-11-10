using osu.Framework.Graphics.Containers;
using SushiStop.Game.Cards.Drawables;

namespace SushiStop.Game.Cards
{
    public class WasabiCard : Card
    {
        public override CompositeDrawable CreateDrawableCard() => new DrawableWasabiCard(this);
    }
}
