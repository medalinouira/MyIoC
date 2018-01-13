/// http://www.sweetmit.com
/// https://github.com/medalinouira
/// Copyright © Mohamed Ali NOUIRA. All rights reserved.
using System;

namespace MyIoCSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Register IPlayer to PlayerOne
            MyIoC.MyIoC.Instance.Register<IPlayer, PlayerOne>();

            if (MyIoC.MyIoC.Instance.IsRegistered<IPlayer>())
            {
                MyIoC.MyIoC.Instance.Unregister<IPlayer>();
            }

            //Register IPlayer to PlayerOne
            MyIoC.MyIoC.Instance.Register<IPlayer, PlayerTwo>();

            //Register Match to Match
            MyIoC.MyIoC.Instance.Register<Match, Match>();

            //Resolve the Match class
            var match = MyIoC.MyIoC.Instance.Resolve<Match>();

            //Show the message of the registred class
            match.Shoot();

            Console.Read();
        }

        public static IPlayer player { get; set; }
    }

    public class Match
    {
        private readonly IPlayer player;
        public Match(IPlayer player)
        {
            this.player = player;
        }
        public void Shoot()
        {
            var shootMessage = player.Shoot();
            Console.WriteLine(shootMessage);
        }
    }

    public class PlayerOne : IPlayer
    {
        public string Shoot()
        {
            return "PlayerOne shoot the ball !";
        }
    }

    public class PlayerTwo : IPlayer
    {
        public string Shoot()
        {
            return "PlayerTwo shoot the ball!";
        }
    }

    public interface IPlayer
    {
        string Shoot();
    }
}
