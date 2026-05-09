using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace MindJackMod
{
    //I'm like 90% sure I need this to trigger my Mindjack raid.
    public class MJUWE_RaidStrategyWorker_ImmediateAttackMindjack : RaidStrategyWorker_ImmediateAttack
    {
        protected bool MatchesRequiredPawnKind(PawnKindDef kind) => kind.HasModExtension<MJUWE_DefModExtension>();
        protected int MinRequiredPawnsForPoints(float pointsTotal, Faction faction = null)
        {
            return 1;
        }

    }
}
