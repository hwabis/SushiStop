using System;
using SushiStop.Game.Cards.Drawables;

namespace SushiStop.Game.Cards
{
    public class SashimiCard : Card
    {
        public override DrawableCard CreateDrawableCard(Action onClick = null) =>
            new DrawableSashimiCard(this, onClick);
    }
}
