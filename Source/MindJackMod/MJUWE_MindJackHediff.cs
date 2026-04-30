using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace MindJackMod
{
    public class MJUWE_MindJack : Hediff_Implant
    {
        public bool isRegistered;
        public Thing registeredWeapon;
        public override float Severity { get => base.Severity; set => base.Severity = value; }
        //I am jerry rigging the LabelInBrackets method in the Hediff_Addiction class
        //Purpose is to display the % in the brackets when Mind jack is formatting

        //This sets the label colour to be red while in the formatting stage
        public override Color LabelColor
        {
            get {
                Color color = base.LabelColor;
                if (Severity > 0.002 && Severity <= 1)
                {
                    color = Color.red;
                    return color;
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

        //This ticks every delta instead of every tick (still not 100% sure where the delta comes from) to reduce performance load. When the formatting starts, severity is set to 1, and this starts ticking
        //down until it hits Severity 0.001 where it changes to (unregistered). It also checks if it's in that Severity so as to unregister the weapon once the formatting process finishes
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

    }
}
