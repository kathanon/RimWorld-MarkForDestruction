using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace MarkForDestruction;
public class JobDriver_Smash : JobDriver_AttackMelee {
    private class ActiveSet : List<Verse.WeakReference<Job>> {
        public void Start(Job job) 
            => Add(new(job));

        public void Stop(Job job) 
            => RemoveAll(x => !x.IsAlive || ReferenceEquals(x.Target, job));

        public bool IsActive
            => this.Any(x => x.IsAlive);
    }

    private static readonly ConditionalWeakTable<Thing, ActiveSet> active = new();

    public static bool ActiveFor(Thing thing)
        => active.GetOrCreateValue(thing).IsActive;

    private ActiveSet Active
        => active.GetOrCreateValue(TargetThingA);

    protected override IEnumerable<Toil> MakeNewToils() {
        this.FailOnThingMissingDesignation(TargetIndex.A, MyDefOf.Destroy);
        AddFinishAction(() => Active.Stop(job));
        Active.Start(job);
        return base.MakeNewToils();
    }
}
