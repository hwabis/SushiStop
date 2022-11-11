using System;
using SushiStop.Game.Cards.Drawables;

namespace SushiStop.Game.Cards
{
    public class WasabiCard : Card
    {
        public override DrawableCard CreateDrawableCard(Action onClick = null) =>
            new DrawableWasabiCard(this, onClick);
    }
}
