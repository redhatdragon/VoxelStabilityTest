using SDL2;
using System;

partial class App {
    class Program {

        static void Main(string[] args) {
            Console.WriteLine("Initializing SDL2...");
            HAL.init();
            Console.WriteLine("Done!");

            Console.WriteLine("Initializing app...");
            start();
            Console.WriteLine("Done!");

            Console.WriteLine("Looping...");
            while (HAL.isRunning()) {
                TimeSpan startTime = new TimeSpan();
                HAL.tickStart();
                tick();
                HAL.tickEnd();
                //while(startTime.TotalMilliseconds < 60) {
                    
                //}
            }
            Console.WriteLine("Ending loop!");
            end();
            HAL.end();
        }
    }



    public static partial void start();
    public static partial void tick();
    public static partial void end();
}
