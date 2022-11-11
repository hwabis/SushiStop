using SushiStop.Game.Cards.Drawables;

namespace SushiStop.Game.Cards
{
    public class NigiriCard : Card
    {
        // 1, 2, or 3 (egg, salmon, or squid)
        public int Value { get; private set; }

        public NigiriCard(int count)
        {
            Value = count;
        }

        public override DrawableCard CreateDrawableCard() => new DrawableNigiriCard(this, Value);
    }
}
