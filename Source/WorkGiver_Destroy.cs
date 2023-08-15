using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace MarkForDestruction;
public abstract class WorkGiver_Destroy : WorkGiver_Scanner {
    protected abstract JobDef JobDef { get; }

    protected virtual bool Accepts(Thing x) 
        => !x.Destroyed;

    protected virtual Thing Destination(Pawn pawn, Thing target)
        => null;

    public override ThingRequest PotentialWorkThingRequest 
        => ThingRequest.ForGroup(ThingRequestGroup.HaulableEverOrMinifiable);

    public override int MaxRegionsToScanBeforeGlobalSearch 
        => 1;

    public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn) 
        => pawn.Map.designationManager.designationsByDef[MyDefOf.Destroy]
               .Select(x => x.target.Thing)
               .Where(Accepts);

    public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        => t.Map.designationManager.DesignationOn(t, MyDefOf.Destroy) != null
        && t.Spawned
        && t.Position.InAllowedArea(pawn)
        && Accepts(t);

    public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false) {
        var job = JobMaker.MakeJob(JobDef, t, Destination(pawn, t));
        job.count = t.stackCount;
        job.maxNumMeleeAttacks = 5;
        return job;
    }
}
