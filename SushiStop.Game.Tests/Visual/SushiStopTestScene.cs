using osu.Framework.Testing;

namespace SushiStop.Game.Tests.Visual
{
    public class SushiStopTestScene : TestScene
    {
        protected override ITestSceneTestRunner CreateRunner() => new SushiStopTestSceneTestRunner();

        private class SushiStopTestSceneTestRunner : SushiStopGameBase, ITestSceneTestRunner
        {
            private TestSceneTestRunner.TestRunner runner;

            protected override void LoadAsyncComplete()
            {
                base.LoadAsyncComplete();
                Add(runner = new TestSceneTestRunner.TestRunner());
            }

            public void RunTestBlocking(TestScene test) => runner.RunTestBlocking(test);
        }
    }
}
