using MindJackMod;
using RimWorld;
using Verse;
using Verse.AI;

namespace MindJackUniqueWeaponBind
{
    public class MJUWE_RegisterToWeapon : FloatMenuOptionProvider
    {
        public override bool Drafted => true;
        public override bool Undrafted => true;
        public override bool Multiselect => false;

        //Adding the context menu item to allow Registration
        public override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
        {
            Pawn currentPawn = context.FirstSelectedPawn;
            //This checks if the selected weapon has a Port and Pawn has a mindjack installed
            if (clickedThing.HasComp<MJUWE_PortThingComp>() && currentPawn.health.hediffSet.TryGetHediff(MJUWE_DefOf.MJUWE_MindJackHediff, out Hediff hediff))
            {

                //We need to check if the Pawn can actually register
                MJUWE_MindJack mindjack = (MJUWE_MindJack)hediff;
                MJUWE_PortThingComp port = clickedThing.TryGetComp<MJUWE_PortThingComp>();

                if (currentPawn.WorkTagIsDisabled(WorkTags.Violent))
                {   
                    return new FloatMenuOption("MJUWE_PortThingComp_ErrorPawnNoViolent".Translate(clickedThing.Label, currentPawn.LabelShort), null);
                }
                //in case of pawn formatting
                if(mindjack.Severity > 0.002 && mindjack.Severity <= 1)
                {
                    return new FloatMenuOption("MJUWE_PortThingComp_ErrorFormat".Translate(clickedThing.Label), null);
                }
                
                //in case the weapon is already registered to someone else (including the same user if they formatted)
                if (port.isRegistered)
                {
                    if (port.registeredPawn != currentPawn || (port.registeredPawn != currentPawn && !mindjack.isRegistered))
                    {
                        return new FloatMenuOption("MJUWE_PortThingComp_ErrorWeaponReg".Translate(clickedThing.Label), null);
                    }

                }
                //in case the mindjack is already registered to another weapon
                else if (mindjack.isRegistered && mindjack.registeredWeapon != null)
                {

                    if (mindjack.registeredWeapon != clickedThing)
                    {
                        return new FloatMenuOption("MJUWE_PortThingComp_ErrorPawnReg".Translate(clickedThing.Label), null);
                    }
                }
                //if the registration can happen
                else if (!mindjack.isRegistered)
                {
                    //Adds context menu "Register to (Weapon Name)"
                    return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(
                        "MJUWE_Bind".Translate(clickedThing.Label), () =>
                        {
                            //This calls the job that registers upon completion

                            Job job = JobMaker.MakeJob(MJUWE_DefOf.MJUWE_MindJackRegistration,
                                new LocalTargetInfo(clickedThing));
                            context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                        }), context.FirstSelectedPawn, clickedThing);
                }
            }
            return null;
            }
        }
    }
       