using osu.Framework.Platform;
using osu.Framework;
using SushiStop.Game;

namespace SushiStop.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableDesktopHost(@"SushiStop"))
            using (osu.Framework.Game game = new SushiStopGame())
                host.Run(game);
        }
    }
}
