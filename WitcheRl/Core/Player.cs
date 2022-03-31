using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;

namespace WitcheRl.Core
{
    public class Player : Actor
    {
        public Player()
        {
            Name = "Gelart";
            NameWhom = "Gelarta";
            Awareness = 15;
            Speed = 10;
            Attack = 2;
            AttackChance = 50;
            Defense = 1;
            DefenseChance = 50;
            Health = 10;
            MaxHealth = 10;
            Gold = 0;

            Color = Colors.Player;
            Symbol = '@';
            X = 10;
            Y = 10;
        }
        public void DrawStats( RLConsole statConsole )
        {
            statConsole.Print( 1, 1, $"Imie:    {Name}", Colors.Text );
            statConsole.Print( 1, 3, $"Zdrowie: {Health}/{MaxHealth}", Colors.Text );
            statConsole.Print( 1, 5, $"Atak:    {Attack} ({AttackChance}%)", Colors.Text );
            statConsole.Print( 1, 7, $"Obrona:  {Defense} ({DefenseChance}%)", Colors.Text );
            statConsole.Print( 1, 9, $"Zloto:   {Gold}", Colors.Gold );
        }
    }
}
