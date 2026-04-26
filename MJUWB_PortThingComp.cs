using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace MindJackUniqueWeaponBind
{
    public class MJUWB_PortThingComp : ThingComp
    {
        public Pawn registeredPawn = null;
        public bool isRegistered = false;
        //Adds the Port display to the Weapon's Basics section
        public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
        {   
            if (!isRegistered)
            {   
                //Returns the text from the XML
                yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MJUWB_PortThingComp_PortName".Translate(), "MJUWB_PortThingComp_PortStateDisconnected".Translate(), "MJUWB_PortThingComp_PortDescriptionDisconnected".Translate(), 150000);
            }
            //Returns the text from the XML and inserts the Pawn's name
            else yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MJUWB_PortThingComp_PortName".Translate(), "MJUWB_PortThingComp_PortStateConnectedValid".Translate(registeredPawn.NameFullColored), "MJUWB_PortThingComp_PortDescriptionConnectedValid".Translate(registeredPawn.NameFullColored), 150000);

        }

        //When the pawn registers the Mind Jack to the weapon's Port
        public void ConnectToPort(Pawn pawn)
        {
            isRegistered = true;
            registeredPawn = pawn;
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