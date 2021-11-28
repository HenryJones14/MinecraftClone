using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecraftClone;
using OpenTK;

class Program
{
    public static int Main()
    {
        using (MainGame game = new MainGame(1280, 720, "MinecraftClone"))
        {
            //Run takes a double, which is how many frames per second it should strive to reach.
            //You can leave that out and it'll just update as fast as the hardware will allow it.
            game.Run(144);
        }

        /*Console.WriteLine("Expected result = " + Convert.ToString((uint)32, 2) + Convert.ToString((uint)45, 2) + Convert.ToString((uint)63, 2));
        Console.WriteLine("Max vector size = " + Convert.ToString((uint)63, 2) + "\n");

        Console.WriteLine(Convert.ToString(uint.MaxValue, 2));
        Console.WriteLine(Convert.ToString((uint)63, 2) + Convert.ToString((uint)63, 2) + Convert.ToString((uint)63, 2)+ 0 + Convert.ToString((uint)100, 2));
        Console.WriteLine((uint)(false ? 1 : 0));
        Console.WriteLine((uint)(true ? 1 : 0));

        Console.WriteLine("\n");
        Vector3 val = new Vector3(32, 45, 63);
        uint res;

        Console.WriteLine("Value: " + val);
        res = PackPosition(val);
        Console.WriteLine("Result: " + Convert.ToString(res, 2));
        val = UnpackPosition(res);
        Console.WriteLine("Value: " + val);

        Console.ReadKey();*/

        return 0;
    }
}
