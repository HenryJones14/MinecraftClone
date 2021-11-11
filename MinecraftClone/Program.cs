using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecraftClone;

class Program
{
    public static int Main()
    {
        using (MainGame game = new MainGame(720, 720, "MinecraftClone"))
        {
            //Run takes a double, which is how many frames per second it should strive to reach.
            //You can leave that out and it'll just update as fast as the hardware will allow it.
            game.Run(144);
        }


        return 0;
    }
}
