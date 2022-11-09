using osu.Framework.Graphics;
using NUnit.Framework;
using SushiStop.Game.Elements;
using osuTK.Graphics;

namespace SushiStop.Game.Tests.Visual
{
    [TestFixture]
    public class TestSceneCardBackground : SushiStopTestScene
    {
        public TestSceneCardBackground()
        {
            Add(new CardBackground(Color4.DarkRed)
            {
                Anchor = Anchor.Centre
            });
        }
    }
}
