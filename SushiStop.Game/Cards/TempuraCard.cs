using System;
using SushiStop.Game.Cards.Drawables;

namespace SushiStop.Game.Cards
{
    public class TempuraCard : Card
    {
        public override DrawableCard CreateDrawableCard(Action onClick = null) =>
            new DrawableTempuraCard(this, onClick);
    }
}
