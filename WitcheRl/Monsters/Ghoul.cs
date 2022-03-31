using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueSharp.DiceNotation;
using WitcheRl.Core;

namespace WitcheRl.Monsters
{
    public class Ghoul : Monster
    {
        public static Ghoul Create( int level )
        {
            int health = Dice.Roll( "2D5" );
            return new Ghoul {
                Attack = Dice.Roll( "1D3" ) + level / 3,
                AttackChance = 40,
                Awareness = 10,
                Speed = 12,
                Color = Colors.GhoulColor,
                Defense = Dice.Roll( "1D3" ) + level / 3,
                DefenseChance = 20,
                Gold = Dice.Roll( "5D5" ),
                Health = health,
                MaxHealth = health,
                Name = "Ghoul",
                NameWhom = "ghoula",
                Symbol = 'g'
            };
        }
    }
}
