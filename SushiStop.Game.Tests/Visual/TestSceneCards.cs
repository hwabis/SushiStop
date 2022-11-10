using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using osuTK.Graphics;
using SushiStop.Game.Cards.Drawables;

namespace SushiStop.Game.Tests.Visual
{
    [TestFixture]
    public class TestSceneCards : SushiStopTestScene
    {
        public TestSceneCards()
        {
            Add(new FillFlowContainer
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                AutoSizeAxes = Axes.Both,
                Spacing = new Vector2(92, 0),
                Children = new Drawable[]
                {
                    // Why the gap ??
                    new DrawableTempura(),
                    new DrawableTempura(),
                    new CardBackground(Color4.DarkRed),
                    new DrawableTempura(),
                    new DrawableTempura(),
                }
            });
        }
    }
}
