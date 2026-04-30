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
            }
        }

    }
}
