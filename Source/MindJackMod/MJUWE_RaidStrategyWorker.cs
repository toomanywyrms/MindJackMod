using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI.Group;


namespace MindJackMod
{
    //This *should* force the raid to generate only when a mindkjack user is present. I'm not 
    public class MJUWE_RaidStrategyWorker_MindjackRaid : RaidStrategyWorker_WithRequiredPawnKinds
    {


        //These two methids should check if at least 1 Mindjack Pawnkind is present in the raid
        protected override bool MatchesRequiredPawnKind(PawnKindDef kind)
        {
            return kind.HasModExtension<MJUWE_DefModExtension>();
        }
        protected override int MinRequiredPawnsForPoints(float pointsTotal, Faction faction = null)
        {
            return 1;
        }

        // The raid needs a lord job, so I copied the one from "RaidStrategyWorker_ImmediateAttack"
        protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
        {
            IntVec3 originCell = (parms.spawnCenter.IsValid ? parms.spawnCenter : pawns[0].PositionHeld);
            if (parms.attackTargets != null && parms.attackTargets.Count > 0)
            {
                return new LordJob_AssaultThings(parms.faction, parms.attackTargets);
            }
            if (parms.faction.HostileTo(Faction.OfPlayer))
            {
                return new LordJob_AssaultColony(parms.faction, canTimeoutOrFlee: parms.canTimeoutOrFlee, canKidnap: parms.canKidnap, sappers: false, useAvoidGridSmart: false, canSteal: parms.canSteal);
            }
            RCellFinder.TryFindRandomSpotJustOutsideColony(originCell, map, out var result);
            return new LordJob_AssistColony(parms.faction, result);

        }

    }
}

