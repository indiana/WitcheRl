using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp;
using WitcheRl.Interfaces;

namespace WitcheRl.Core
{
    public class Actor : IActor, IDrawable, IScheduleable
    {
        private string _name;
        private int _awareness;
        private int _attack;
        private int _defense;
        private int _health;
        private int _maxHealth;
        private int _gold;
        private int _attackChance;
        private int _defenseChance;
        private string _nameWhom;
        private int _speed;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string NameWhom
        {
            get => _nameWhom;
            set => _nameWhom = value;
        }

        public int Awareness
        {
            get => _awareness;
            set => _awareness = value;
        }

        public int Speed
        {
            get => _speed;
            set => _speed = value;
        }

        public int Attack
        {
            get => _attack;
            set => _attack = value;
        }

        public int AttackChance
        {
            get => _attackChance;
            set => _attackChance = value;
        }

        public int Defense
        {
            get => _defense;
            set => _defense = value;
        }

        public int DefenseChance
        {
            get => _defenseChance;
            set => _defenseChance = value;
        }

        public int Health
        {
            get => _health;
            set => _health = value;
        }

        public int MaxHealth
        {
            get => _maxHealth;
            set => _maxHealth = value;
        }

        public int Gold
        {
            get => _gold;
            set => _gold = value;
        }

        public RLColor Color { get; set; }
        public char Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public void Draw(RLConsole console, IMap map)
        {
            // Don't draw actors in cells that haven't been explored
            if ( !map.GetCell( X, Y ).IsExplored )
            {
                return;
            }
 
            // Only draw the actor with the color and symbol when they are in field-of-view
            if ( map.IsInFov( X, Y ) )
            {
                console.Set( X, Y, Color, Colors.FloorBackgroundFov, Symbol );
            }
            else
            {
                // When not in field-of-view just draw a normal floor
                console.Set( X, Y, Colors.Floor, Colors.FloorBackground, '.' );
            }
        }

        public int Time => Speed;
    }
}
