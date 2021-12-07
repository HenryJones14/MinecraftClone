using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MinecraftClone;

class Program
{
    public static int Main()
    {
        Thread mainThread = Thread.CurrentThread;
        mainThread.Name = "MainThread";

        using (MainGame game = new MainGame(1280, 720, "MinecraftClone"))
        {
            //Run takes a double, which is how many frames per second it should strive to reach.
            //You can leave that out and it'll just update as fast as the hardware will allow it.
            game.Run();
        }

        return 0;
    }
}
