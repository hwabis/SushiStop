using System;
using SushiStop.Game.Cards.Drawables;

namespace SushiStop.Game.Cards
{
    public class ChopsticksCard : Card
    {
        public override DrawableCard CreateDrawableCard() => new DrawableChopsticksCard(this);
    }
}
