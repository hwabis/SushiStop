using System;
using SushiStop.Game.Cards.Drawables;

namespace SushiStop.Game.Cards
{
    public class NigiriCard : Card
    {
        // 1, 2, or 3 (egg, salmon, or squid)
        public int Value { get; private set; }

        public NigiriCard(int value)
        {
            if (value > 3 || value < 1)
                throw new ArgumentOutOfRangeException("Value must be between 1 and 3.");

            Value = value;
        }

        public override DrawableCard CreateDrawableCard() => new DrawableNigiriCard(this, Value);
    }
}
