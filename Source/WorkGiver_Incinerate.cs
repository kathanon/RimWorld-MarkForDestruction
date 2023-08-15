using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
using static UnityEngine.GraphicsBuffer;

namespace MarkForDestruction;
public class WorkGiver_Incinerate : WorkGiver_Destroy {
    private static readonly ThingFilter filter = new();

    static WorkGiver_Incinerate() {
        filter.SetAllow(ThingCategoryDefOf.Corpses, true);
        filter.SetAllow(ThingCategoryDefOf.Apparel, true);
        filter.SetAllow(ThingCategoryDefOf.Weapons, true);
        filter.SetAllow(ThingCategoryDefOf.Drugs,   true);

        filter.SetAllow(ThingCategoryDefOf.CorpsesMechanoid, false);

        filter.SetAllow(MyDefOf.NonBurnableApparel, false);
        filter.SetAllow(MyDefOf.NonBurnableWeapons, false);
    }

    private int cachedTick = 0;
    private Pawn cachedPawn = null;
    private Thing cachedTarget = null;
    private Thing cachedDest;

    private int cachedTickList = 0;
    private Map cachedMap = null;
    private List<Building> cachedList;

    protected override JobDef JobDef
        => MyDefOf.Incinerate;

    protected override bool Accepts(Thing x) 
        => base.Accepts(x) 
        && (x.GetStatValue(StatDefOf.Flammability) >= 0.01f || filter.Allows(x))
        && x.def.EverHaulable
        && !JobDriver_Smash.ActiveFor(x);

    public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false) 
        => base.HasJobOnThing(pawn, t, forced)
        && pawn.CanReserve(t)
        && Destinations(t.Map).Any(x => pawn.CanReserve(x));

    protected override Thing Destination(Pawn pawn, Thing target) {
        int tick = Find.TickManager.TicksGame;
        if (!ReferenceEquals(pawn, cachedPawn)
                || !ReferenceEquals(target, cachedTarget)
                || cachedTick != tick) {
            cachedTick = tick;
            cachedPawn = pawn;
            cachedTarget = target;
            cachedDest = Destinations(target.Map)
                .Where(x => pawn.CanReserve(x))
                .OrderBy(x => (x.Position - target.Position).LengthHorizontalSquared)
                .FirstOrDefault();
        }
        return cachedDest;
    }

    private IEnumerable<Thing> Destinations(Map map) {
        int tick = Find.TickManager.TicksGame;
        if (!ReferenceEquals(map, cachedMap) || cachedTickList != tick) {
            cachedTickList = tick;
            cachedMap = map;
            cachedList = map.listerBuildings
                .AllBuildingsColonistOfDef(MyDefOf.ElectricCrematorium)
                .ToList();
        }
        return cachedList;
    }
}
