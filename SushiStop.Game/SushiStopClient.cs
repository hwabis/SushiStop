using System.Net;
using NetCoreServer;
using osu.Framework.Screens;
using SushiStop.Game.Screens;

namespace SushiStop.Game
{
    public class SushiStopClient : TcpClient
    {
        private ScreenStack screenStack;

        public SushiStopClient(IPAddress address, int port, ScreenStack screenStack)
            : base(address, port)
        {
            this.screenStack = screenStack;
        }

        protected override void OnDisconnected()
        {
            while (screenStack.CurrentScreen is not HomeScreen)
                screenStack.Exit();
        }
    }
}
