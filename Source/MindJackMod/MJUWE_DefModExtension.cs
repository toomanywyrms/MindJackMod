using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace MindJackMod
{
    //this is used to attach it to the PawnkindDef in the XML, because some functions can return objects that use a DefModExtension
    //so for raids as well as changing the hediff on the raider, this can be used to identify them
    public class MJUWE_DefModExtension : DefModExtension
    {
        public bool isMindjackRaider = true;
    }
}
