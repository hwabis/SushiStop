﻿using System.Collections.Generic;
using Newtonsoft.Json;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;
using osuTK;
using SushiStop.Game.Cards;
using SushiStop.Game.Networking;

namespace SushiStop.Game.Screens
{
    public class PlayScreen : Screen
    {
        // Player-related info
        public List<Card> Hand { get; set; }

        private FillFlowContainer drawableHand { get; set; }

        private SushiStopClient client;

        public PlayScreen(SushiStopClient client)
        {
            this.client = client;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChildren = new Drawable[]
            {
                drawableHand = new FillFlowContainer
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    AutoSizeAxes = Axes.Both,
                    Spacing = new Vector2(92, 0),
                    Y = -5
                }
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            client.SendAsync(JsonConvert.SerializeObject(new TcpMessage
            {
                Type = TcpMessageType.StartRoundRequest
            }));
        }

        public void CreateDrawableHand()
        {
            foreach (Card card in Hand)
                Schedule(() => drawableHand.Add(card.CreateDrawableCard()));
        }
    }
}
