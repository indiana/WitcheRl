using System.Text;
using RogueSharp;
using RogueSharp.DiceNotation;
using WitcheRl.Core;
using WitcheRl.Interfaces;

namespace WitcheRl.Systems
{
    public class CommandSystem
    {
        // Return value is true if the player was able to move
        // false when the player couldn't move, such as trying to move into a wall
        public bool MovePlayer(Direction direction)
        {
            var x = Game.Player.X;
            var y = Game.Player.Y;

            switch (direction)
            {
                case Direction.Up:
                {
                    y = Game.Player.Y - 1;
                    break;
                }
                case Direction.Down:
                {
                    y = Game.Player.Y + 1;
                    break;
                }
                case Direction.Left:
                {
                    x = Game.Player.X - 1;
                    break;
                }
                case Direction.Right:
                {
                    x = Game.Player.X + 1;
                    break;
                }
                default:
                {
                    return false;
                }
            }

            if (Game.DungeonMap.SetActorPosition(Game.Player, x, y)) return true;

            var monster = Game.DungeonMap.GetMonsterAt(x, y);

            if (monster != null)
            {
                Attack(Game.Player, monster);
                return true;
            }

            return false;
        }

        public void Attack(Actor attacker, Actor defender)
        {
            var attackMessage = new StringBuilder();
            var defenseMessage = new StringBuilder();

            var hits = ResolveAttack(attacker, defender, attackMessage);

            var blocks = ResolveDefense(defender, hits, attackMessage, defenseMessage);

            Game.MessageLog.Add(attackMessage.ToString());
            if (!string.IsNullOrWhiteSpace(defenseMessage.ToString())) Game.MessageLog.Add(defenseMessage.ToString());

            var damage = hits - blocks;

            ResolveDamage(defender, damage);
        }

        // The attacker rolls based on his stats to see if he gets any hits
        private static int ResolveAttack(Actor attacker, Actor defender, StringBuilder attackMessage)
        {
            var hits = 0;

            attackMessage.AppendFormat("{0} atakuje {1} i wyrzuca: ", attacker.Name, defender.NameWhom);

            // Roll a number of 100-sided dice equal to the Attack value of the attacking actor
            DiceExpression attackDice = new DiceExpression().Dice(attacker.Attack, 100);
            DiceResult attackResult = attackDice.Roll();

            // Look at the face value of each single die that was rolled
            foreach (TermResult termResult in attackResult.Results)
            {
                attackMessage.Append(termResult.Value + ", ");
                // Compare the value to 100 minus the attack chance and add a hit if it's greater
                if (termResult.Value >= 100 - attacker.AttackChance) hits++;
            }

            return hits;
        }

// The defender rolls based on his stats to see if he blocks any of the hits from the attacker
        private static int ResolveDefense(Actor defender, int hits, StringBuilder attackMessage,
            StringBuilder defenseMessage)
        {
            var blocks = 0;

            if (hits > 0)
            {
                attackMessage.AppendFormat("zadajac ciosy w ilosci: {0} ", hits);
                defenseMessage.AppendFormat("  {0} broni sie i wyrzuca: ", defender.Name);

                // Roll a number of 100-sided dice equal to the Defense value of the defendering actor
                DiceExpression defenseDice = new DiceExpression().Dice(defender.Defense, 100);
                DiceResult defenseRoll = defenseDice.Roll();

                // Look at the face value of each single die that was rolled
                foreach (TermResult termResult in defenseRoll.Results)
                {
                    defenseMessage.Append(termResult.Value + ", ");
                    // Compare the value to 100 minus the defense chance and add a block if it's greater
                    if (termResult.Value >= 100 - defender.DefenseChance) blocks++;
                }

                defenseMessage.AppendFormat("skutkujac iloscia blokow: {0}.", blocks);
            }
            else
            {
                attackMessage.Append("i kompletnie pudluje.");
            }

            return blocks;
        }

        // Apply any damage that wasn't blocked to the defender
        private static void ResolveDamage(Actor defender, int damage)
        {
            if (damage > 0)
            {
                defender.Health = defender.Health - damage;

                Game.MessageLog.Add($"  {defender.Name} zostal trafiony za {damage} obrazen");

                if (defender.Health <= 0) ResolveDeath(defender);
            }
            else
            {
                Game.MessageLog.Add($"  {defender.Name} zablokowal wszystkie obrazenia");
            }
        }

        // Remove the defender from the map and add some messages upon death.
        private static void ResolveDeath(Actor defender)
        {
            if (defender is Player)
            {
                Game.MessageLog.Add($"  {defender.Name} nie zyje, GRATZ!");
            }
            else if (defender is Monster)
            {
                Game.DungeonMap.RemoveMonster((Monster)defender);

                Game.MessageLog.Add($"  {defender.Name} umiera i upuszcza zloto w ilosci: {defender.Gold}");
            }
        }
        public bool IsPlayerTurn { get; set; }
 
        public void EndPlayerTurn()
        {
            IsPlayerTurn = false;
        }
 
        public void ActivateMonsters()
        {
            IScheduleable scheduleable = Game.SchedulingSystem.Get();
            if ( scheduleable is Player )
            {
                IsPlayerTurn = true;
                Game.SchedulingSystem.Add( Game.Player );
            }
            else
            {
                Monster monster = scheduleable as Monster;
 
                if ( monster != null )
                {
                    monster.PerformAction( this );
                    Game.SchedulingSystem.Add( monster );
                }
 
                ActivateMonsters();
            }
        }
 
        public void MoveMonster( Monster monster, ICell cell )
        {
            if ( !Game.DungeonMap.SetActorPosition( monster, cell.X, cell.Y ) )
            {
                if ( Game.Player.X == cell.X && Game.Player.Y == cell.Y )
                {
                    Attack( monster, Game.Player );
                }
            }
        }
    }
}