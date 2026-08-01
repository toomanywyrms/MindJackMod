using MindJackUniqueWeaponBind;
using UnityEngine;
using Verse;

namespace MindJackMod
{
    public class MJUWE_MindJack : Hediff_Implant
    {
        public bool isRegistered;
        public Thing registeredWeapon;

        //This sets the label colour to be red while in the formatting stage
        public override Color LabelColor
        {
            get {
                Color color = base.LabelColor;
                if (Severity > 0.002 && Severity <= 1)
                {
                    color = Color.red;
                }
                return color;

            } 
        }
        //this is used so that I can insert the weapon's name directly into the label by just taking the String variable. The Label in the XML will have something like "{0}, disconnected" where {0} is replaced with 
        //registeredWeapon.Label
        public override string LabelInBrackets
        {
            get
            {
                //I am jerry rigging the LabelInBrackets method in the Hediff_Addiction class
                //Purpose is to display the % in the brackets when Mindjack is formatting
                string labelInBrackets = base.LabelInBrackets;
                if (Severity > 0.002 && Severity <= 1)
                {
                    string text = (1f - Severity).ToStringPercent("F0");
                    if (def.CompProps<HediffCompProperties_SeverityPerDay>() != null)
                    {
                        if (!labelInBrackets.NullOrEmpty())
                        {
                            return labelInBrackets + ", " + text;
                        }
                        return text;
                      
                    }
                }
                else if (Severity == 3 || Severity == 4)
                {
                    return registeredWeapon.Label + ", " + labelInBrackets;
                }
                return labelInBrackets;
            }
        }
        
        //changing description to add a variation for the corrupted stage

        public override string Description
        {
            get
            {
                string description = base.Description;
                if (Severity == 5)
                {
                    return description + "\n\n" + "MJUWE_Heddiff_CorruptedDescription".Translate();
                }

                return description;
            }
        }


        //In order to have my mindjack raider all set up, this methods needs to be called when the Hediff is spawned. If the owner of the Hediff has my mod extention
        //(which I use to identify them), it runs this method to have them linked with the weapon the exact second they spawn
        public override void Notify_Spawned()
        {
            base.Notify_Spawned();
            if (pawn.kindDef.HasModExtension<MJUWE_DefModExtension>())
            {
                isRegistered = true;
                registeredWeapon = pawn.equipment.Primary;
                MJUWE_PortThingComp port = registeredWeapon.TryGetComp<MJUWE_PortThingComp>();
                port.isRegistered = true;
                port.registeredPawn = pawn;
                port.Notify_Equipped(pawn);
            }
        }

        //This ticks every delta instead of every tick (still not 100% sure where the delta comes from) to reduce performance load. When the formatting starts, severity is set to 1, and this starts ticking
        //down until it hits Severity 0.001 where it changes to (unregistered). It also checks if it's in that Severity so as to unregister the weapon once the formatting process finishes.
        public override void PostTickInterval(int delta)
        {
            if (comps != null)
            {
                float severityAdjustment = 0f;
                for (int i = 0; i < comps.Count; i++)
                {
                    comps[i].CompPostTickInterval(ref severityAdjustment, delta);
                }
                if (Severity > 0.002 && Severity <= 1 )
                {
                    Severity += severityAdjustment;
                }
                if (Severity == 0.001)
                {
                    registeredWeapon = null;
                    isRegistered = false;
                }
            }
        }

        //for saving information into the save file
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref isRegistered, "isRegistered");
            Scribe_References.Look(ref registeredWeapon, "registeredWeapon");
        }

    }
}
