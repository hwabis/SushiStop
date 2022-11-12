using System;
using SushiStop.Game.Cards.Drawables;

namespace SushiStop.Game.Cards
{
    public abstract class Card
    {
        public abstract DrawableCard CreateDrawableCard();
    }
}
