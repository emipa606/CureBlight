using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace CureBlight;

public class JobDriver_CureBlight : JobDriver_PlantWork
{
    private readonly float workAmountFactor = 4f;
    private float workDone;

    protected override DesignationDef RequiredDesignation => CBDefOf.Vic_CureBlightDesign;

    protected override void Init()
    {
        xpPerTick = Plant.Blighted ? 0.085f : 0f;
    }

    protected override Toil PlantWorkDoneToil()
    {
        return Toils_General.RemoveDesignationsOnThing(TargetIndex.A, CBDefOf.Vic_CureBlightDesign);
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref workDone, "workDone");
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
        Init();
        yield return Toils_JobTransforms.MoveCurrentTargetIntoQueue(TargetIndex.A);
        var initExtractTargetFromQueue = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A,
            RequiredDesignation != null
                ? t => Map.designationManager.DesignationOn(t, RequiredDesignation) != null
                : null);
        yield return initExtractTargetFromQueue;
        yield return Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.A);
        yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.A);
        var toil = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch)
            .JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, initExtractTargetFromQueue);
        if (RequiredDesignation != null)
        {
            toil.FailOnThingMissingDesignation(TargetIndex.A, RequiredDesignation);
        }

        yield return toil;
        var cut = new Toil();
        cut.tickAction = delegate
        {
            var actor = cut.actor;
            actor.skills?.Learn(SkillDefOf.Plants, xpPerTick);

            var plant = Plant;
            workDone += WorkDonePerTick(actor, Plant);
            if (!(workDone >= plant.def.plant.harvestWork * workAmountFactor))
            {
                return;
            }

            Plant.Blight.Destroy();
            plant.def.plant.soundHarvestFinish.PlayOneShot(actor);
            workDone = 0f;
            ReadyForNextToil();
        };
        cut.FailOnDespawnedNullOrForbidden(TargetIndex.A);
        if (RequiredDesignation != null)
        {
            cut.FailOnThingMissingDesignation(TargetIndex.A, RequiredDesignation);
        }

        cut.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
        cut.defaultCompleteMode = ToilCompleteMode.Never;
        cut.WithEffect(Plant?.def.plant.IsTree ?? false ? EffecterDefOf.Harvest_Tree : EffecterDefOf.Harvest_Plant,
            TargetIndex.A);
        cut.WithProgressBar(TargetIndex.A, () => workDone / (Plant.def.plant.harvestWork * workAmountFactor), true);
        cut.PlaySustainerOrSound(() => Plant.def.plant.soundHarvesting);
        cut.activeSkill = () => SkillDefOf.Plants;
        yield return cut;
        var toil2 = PlantWorkDoneToil();
        if (toil2 != null)
        {
            yield return toil2;
        }

        yield return Toils_Jump.Jump(initExtractTargetFromQueue);
    }
}