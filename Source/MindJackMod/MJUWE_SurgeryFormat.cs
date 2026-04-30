using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace MindJackMod
{
    internal class MJUWE_SurgeryFormat : Recipe_Surgery
    {
        //this makes sure the Format operation is only available to pawns with a registered mindjack. If it's unregistered or formatting, it doesn't appear
        public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
        {
            Pawn pawn = thing as Pawn;
            if (pawn.health.hediffSet.TryGetHediff(MJUWE_DefOf.MJUWE_MindJackHediff, out Hediff hediff))
            {
                MJUWE_MindJack mindjack = (MJUWE_MindJack)hediff;
               if (mindjack.Severity > 1)
                {
                    return base.AvailableOnNow(thing, part);
                }
                return false;
            }
            return false;
        }
        //sets the minjack to "Formatting". The acual hediff handles the progression of the formatting
        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            if (billDoer != null)
            {
                pawn.health.hediffSet.TryGetHediff(MJUWE_DefOf.MJUWE_MindJackHediff, out Hediff hediff);
                MJUWE_MindJack mindjack = (MJUWE_MindJack)hediff;
                mindjack.Severity = 1f;
                OnSurgerySuccess(pawn, part, billDoer, ingredients, bill);
            }
        }
    }

}

