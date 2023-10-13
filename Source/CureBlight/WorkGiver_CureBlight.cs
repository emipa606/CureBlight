using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace CureBlight;

public class WorkGiver_CureBlight : WorkGiver_PlantsCut
{
    public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
    {
        var desList = pawn.Map.designationManager.AllDesignations;
        foreach (var designation in desList)
        {
            if (designation.def == CBDefOf.Vic_CureBlightDesign)
            {
                yield return designation.target.Thing;
            }
        }
    }

    public override bool ShouldSkip(Pawn pawn, bool forced = false)
    {
        if (!base.ShouldSkip(pawn, forced))
        {
            return false;
        }

        return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(CBDefOf.Vic_CureBlightDesign);
    }

    public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
    {
        if (t.def.category != ThingCategory.Plant)
        {
            return null;
        }

        if (!pawn.CanReserve(t, 1, -1, null, forced))
        {
            return null;
        }

        if (t.IsForbidden(pawn))
        {
            return null;
        }

        if (t.IsBurning())
        {
            return null;
        }

        if (!PlantUtility.PawnWillingToCutPlant_Job(t, pawn))
        {
            return null;
        }

        foreach (var item in pawn.Map.designationManager.AllDesignationsOn(t))
        {
            if (item.def == CBDefOf.Vic_CureBlightDesign)
            {
                return JobMaker.MakeJob(CBDefOf.Vic_CureBlightJob, t);
            }
        }

        return null;
    }

    public override string PostProcessedGerund(Job job)
    {
        return def.gerund;
    }
}