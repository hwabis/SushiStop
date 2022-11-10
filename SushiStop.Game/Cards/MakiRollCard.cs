﻿using osu.Framework.Graphics.Containers;
using SushiStop.Game.Cards.Drawables;

namespace SushiStop.Game.Cards
{
    public class MakiRollCard : Card
    {
        // 1, 2, or 3 rolls
        public int Count { get; private set; }

        public MakiRollCard(int count)
        {
            Count = count;
        }

        public override CompositeDrawable CreateDrawableCard() => new DrawableMakiRollCard(this);
    }
}
