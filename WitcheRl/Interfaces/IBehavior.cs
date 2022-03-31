using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WitcheRl.Core;
using WitcheRl.Systems;

namespace WitcheRl.Interfaces
{
    public interface IBehavior
    {
        bool Act( Monster monster, CommandSystem commandSystem );
    }
}
