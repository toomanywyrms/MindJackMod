using MindJackMod;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace MindJackUniqueWeaponBind
{
    public class MJUWE_PortThingComp : ThingComp
    {
        public Pawn registeredPawn = null;
        public bool isRegistered = false;
        //Adds the Port display to the Weapon's Basics section
        public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
        {   
            if (!isRegistered)
            {   
                //Returns the text from the XML
                yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MJUWE_PortThingComp_PortName".Translate(), "MJUWE_PortThingComp_PortStateDisconnected".Translate(), "MJUWE_PortThingComp_PortDescriptionDisconnected".Translate(), 150000);
            }
            //Returns the text from the XML and inserts the Pawn's name
            else yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MJUWE_PortThingComp_PortName".Translate(), "MJUWE_PortThingComp_PortStateConnectedValid".Translate(registeredPawn.NameFullColored), "MJUWE_PortThingComp_PortDescriptionConnectedValid".Translate(registeredPawn.NameFullColored), 150000);

        }

        //When the pawn registers the Mindjack to the weapon's Port
        public void ConnectToPort(Pawn pawn, MJUWE_MindJack mindjack)
        {
            isRegistered = true;
            registeredPawn = pawn;
            mindjack.Severity = 4;

        }
        //this is called when the Pawn equips a weapon. It checks if the current weapon is the registered one one. If yes, buff
        public override void Notify_Equipped(Pawn pawn)
        {
            if (pawn == registeredPawn)
            {
                CompQuality wepQuality = parent.TryGetComp<CompQuality>();
                pawn.health.hediffSet.TryGetHediff(MJUWE_DefOf.MJUWE_MindJackHediff, out Hediff hediff);
                MJUWE_MindJack mindjack = (MJUWE_MindJack)hediff;

                //double checks that the registered weapon in the mindjack is the same as this weapon
                //if this check isn't done, a pawn could have 2 bonded weapons, as the weapon does not get formatted when the mindjack does
                if (mindjack?.registeredWeapon == parent)
                {
                    wepQuality.SetQuality(QualityCategory.Legendary, null);
                    mindjack.Severity = 3;
                }
                

            }
        }
        //this is called when the Pawn unequips or drops by down. It checks if the weapon dropped is the registered one one. If yes, remove buff.
        public override void Notify_Unequipped(Pawn pawn)
        {
            if (pawn == registeredPawn)
            {
                CompQuality wepQuality = parent.TryGetComp<CompQuality>();
                wepQuality.SetQuality(QualityCategory.Good, null);
                pawn.health.hediffSet.TryGetHediff(MJUWE_DefOf.MJUWE_MindJackHediff, out Hediff hediff);
                MJUWE_MindJack mindjack = (MJUWE_MindJack)hediff;          
                //see above
                if (mindjack?.registeredWeapon == parent)
                {
                    wepQuality.SetQuality(QualityCategory.Good, null);
                    mindjack.Severity = 4;
                }

            }
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            registeredPawn.health.hediffSet.TryGetHediff(MJUWE_DefOf.MJUWE_MindJackHediff, out Hediff hediff);
            MJUWE_MindJack mindjack = (MJUWE_MindJack)hediff;
            mindjack.Severity = 5;
        }

        //this saves relevant info when user saves their game
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref isRegistered, "isRegistered");
            Scribe_References.Look(ref registeredPawn, "registeredPawn");
        }

     }

    

}