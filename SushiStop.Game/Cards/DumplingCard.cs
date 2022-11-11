﻿using System;
using SushiStop.Game.Cards.Drawables;

namespace SushiStop.Game.Cards
{
    public class DumplingCard : Card
    {
        public override DrawableCard CreateDrawableCard(Action onClick = null) =>
            new DrawableDumplingCard(this, onClick);
    }
}
