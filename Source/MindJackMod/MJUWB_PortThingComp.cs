using MindJackMod;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        //When the pawn registers the Mind Jack to the weapon's Port
        public void ConnectToPort(Pawn pawn, MJUWE_MindJack mindjack)
        {
            isRegistered = true;
            registeredPawn = pawn;
            mindjack.Severity = 3;

        }
        //this is called when the Pawn equips a weapon. It checks if the current weapon is the bonded one
        public override void Notify_Equipped(Pawn pawn)
        {
            if (pawn == registeredPawn)
            {
                CompQuality wepQuality = parent.TryGetComp<CompQuality>();
                wepQuality.SetQuality(QualityCategory.Legendary, null);
                pawn.health.hediffSet.TryGetHediff(MJUWE_DefOf.MJUWE_MindJackHediff, out Hediff hediff);
                MJUWE_MindJack mindjack = (MJUWE_MindJack)hediff;
                mindjack.Severity = 2;

            }
        }
        //this is called when the Pawn unequips or drops by down
        public override void Notify_Unequipped(Pawn pawn)
        {
            if (pawn == registeredPawn)
            {
                CompQuality wepQuality = parent.TryGetComp<CompQuality>();
                wepQuality.SetQuality(QualityCategory.Good, null);
                pawn.health.hediffSet.TryGetHediff(MJUWE_DefOf.MJUWE_MindJackHediff, out Hediff hediff);
                MJUWE_MindJack mindjack = (MJUWE_MindJack)hediff;
                mindjack.Severity = 3;

            }
        }

        //saving relevant info
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref isRegistered, "bool");
            Scribe_References.Look(ref registeredPawn, "pawn");
        }

     }

    

}