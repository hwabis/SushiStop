using System;
using SushiStop.Game.Cards.Drawables;

namespace SushiStop.Game.Cards
{
    public class MakiRollCard : Card
    {
        // 1, 2, or 3 rolls
        public int Count { get; private set; }

        public MakiRollCard(int count)
        {
            if (count > 3 || count < 1)
                throw new ArgumentOutOfRangeException("Count must be between 1 and 3.");

            Count = count;
        }

        public override DrawableCard CreateDrawableCard() => new DrawableMakiRollCard(this);
    }
}
