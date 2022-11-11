using System;
using SushiStop.Game.Cards.Drawables;

namespace SushiStop.Game.Cards
{
    public class PuddingCard : Card
    {
        public override DrawableCard CreateDrawableCard(Action onClick = null) =>
            new DrawablePuddingCard(this, onClick);
    }
}
