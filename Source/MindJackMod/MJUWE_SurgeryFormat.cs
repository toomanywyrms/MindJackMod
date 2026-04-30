using MindJackUniqueWeaponBind;
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
        //in order for this to appear, the RecipeDef in the XML needs to be patched to be added to "Human" def, which lists all operations that can be performed on a pawn without outside influences
        public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
        {
            // gets hediff to make sure it exists
            Pawn pawn = thing as Pawn;
            if (pawn.health.hediffSet.TryGetHediff(MJUWE_DefOf.MJUWE_MindJackHediff, out Hediff hediff))
            {
                // If it's registered, it displays the required surgery. Otherwise no.
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
                //very simple, sets Severity to 1 which is what the formatting state is, and removes the weapon data. We also change the quality of the weapon in case it's equipped
                //The OnSurgerySuccess i think is just for the game to have relevant info about the successful operation
                pawn.health.hediffSet.TryGetHediff(MJUWE_DefOf.MJUWE_MindJackHediff, out Hediff hediff);
                MJUWE_MindJack mindjack = (MJUWE_MindJack)hediff;
                mindjack.Severity = 1f;
                CompQuality weaponQuality = mindjack.registeredWeapon.TryGetComp<CompQuality>();
                weaponQuality.SetQuality(QualityCategory.Good, null);
                mindjack.registeredWeapon = null;
                mindjack.isRegistered = false;
                OnSurgerySuccess(pawn, part, billDoer, ingredients, bill);
            }
        }
    }

}

