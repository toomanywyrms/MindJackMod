using MindJackMod;
using RimWorld;
using System;
using Verse;
using Verse.AI;

namespace MindJackUniqueWeaponBind
{
    public class MJUBW_RegisterToWeapon : FloatMenuOptionProvider
    {
        protected override bool Drafted => true;
        protected override bool Undrafted => true;
        protected override bool Multiselect => false;

        //Adding the context menu item to allow Binding
        protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
        {
            Pawn currentPawn = context.FirstSelectedPawn;
            //This checks if the selected weapon has a Port and Pawn has a mindjack installed
            if (clickedThing.HasComp<MJUWE_PortThingComp>() && currentPawn.health.hediffSet.TryGetHediff(MJUWE_DefOf.MJUWE_MindJackHediff, out Hediff hediff))
                {
                //Sets the port and mindjack so we can modify them later
                //Adds context menu "Register to (Weapon Name)"
                //TOADD: Context menues for: PAWN W/O Mindjack, PAWN W/ Registered Mindjack. Weapon already registered
                return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("MJUWE_Bind".Translate(clickedThing.Label), () =>
                {

                    //This calls the job that bonds upon completion

                    Job job = JobMaker.MakeJob(MJUWE_DefOf.MJUWE_MindJackRegistration, new LocalTargetInfo(clickedThing));
                    context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                }), context.FirstSelectedPawn, clickedThing);
            }
            return null;
        }
    }
}
       