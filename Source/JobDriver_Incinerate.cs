using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace MarkForDestruction;
public class JobDriver_Incinerate : JobDriver {
    public override bool TryMakePreToilReservations(bool errorOnFailed) 
        => pawn.Reserve(TargetA, job, errorOnFailed: errorOnFailed) 
        && pawn.Reserve(TargetB, job, errorOnFailed: errorOnFailed);

    protected override IEnumerable<Toil> MakeNewToils() {
        Toil temp;
        var item  = TargetIndex.A;
        var dest  = TargetIndex.B;
        var place = TargetIndex.C;

        var thing = TargetThingA;
        var recipe = thing.def.IsCorpse ? MyDefOf.CremateCorpse : MyDefOf.BurnApparel;

        this.FailOnThingMissingDesignation(item, MyDefOf.Destroy);
        this.FailOnBurningImmobile(item);
        yield return Toils_Goto.GotoThing(item, PathEndMode.ClosestTouch)
            .FailOnDespawnedNullOrForbidden(item)
            .FailOnSomeonePhysicallyInteracting(item);
        yield return Toils_Haul.StartCarryThing(item);
        yield return Toils_Goto.GotoThing(dest, PathEndMode.InteractionCell)
            .FailOnDestroyedOrNull(dest);
        yield return temp = Toils_JobTransforms.SetTargetToIngredientPlaceCell(dest, item, place);
        yield return Toils_Haul.PlaceHauledThingInCell(place, temp, storageMode: false);
        yield return Toils_Goto.GotoThing(dest, PathEndMode.InteractionCell);
        yield return new Incineration(recipe, TargetThingB).MakeToil(item);
        yield return Toils_General.DoAtomic(() => {
            pawn.Map.designationManager.RemoveAllDesignationsOn(thing);
            thing.DeSpawnOrDeselect();
        });
    }

    private class Incineration {
        private readonly RecipeDef recipe;
        private readonly Building_WorkTable facility;
        private Toil toil;
        private float workDone = 0f;
        private int ticks = 0;

        public Incineration(RecipeDef recipe, Thing facility) {
            this.recipe = recipe;
            this.facility = facility as Building_WorkTable;
        }

        public Toil MakeToil(TargetIndex item) {
            toil = ToilMaker.MakeToil("Incinerate");
            toil.tickAction = Tick;
            toil.defaultCompleteMode = ToilCompleteMode.Never;
            return toil
                .WithEffect(Effect, item)
                .PlaySustainerOrSound(Sound)
                .WithProgressBar(item, Progress);
        }

        private void Tick() {
            ticks++;
            facility?.UsedThisTick();

            float speed = 1f;
            if (recipe.workSpeedStat      != null) speed *= toil.actor.GetStatValue(recipe.workSpeedStat);
            if (recipe.workTableSpeedStat != null) speed *= facility.GetStatValue(recipe.workTableSpeedStat);
            if (DebugSettings.fastCrafting)        speed *= 30f;
            workDone += speed;

            toil.actor.GainComfortFromCellIfPossible(chairsOnly: true);

            var jobs = toil.actor.jobs;
            if (Progress() >= 1f) {
                jobs.curDriver.ReadyForNextToil();
            } else if (ticks >= 2000 && ticks % 200 == 0) {
                jobs.CheckForJobOverride();
            }
        }

        private EffecterDef Effect()   => recipe.effectWorking;
        private SoundDef    Sound()    => recipe.soundWorking;
        private float       Progress() => workDone / recipe.workAmount;
    }
}
