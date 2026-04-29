using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public override string LabelInBrackets
        {
            get
            {
                string labelInBrackets = base.LabelInBrackets;
                if (Severity > 1 && Severity < 2) { 
                    string text = (1f - Severity).ToStringPercent("F0");
                    if (def.CompProps<HediffCompProperties_SeverityPerDay>() != null)
                    {
                        if (!labelInBrackets.NullOrEmpty())
                        {
                            return labelInBrackets + " " + text;
                        }
                        return text;
                    }
                }
                else if (Severity == 2 || Severity == 3)
                {
                    return registeredWeapon.Label + ", " + labelInBrackets;
                }
                    return labelInBrackets;
            }
        }

    }
}
