using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace MarkForDestruction;
public class Designator_Destroy : Designator {
    public Designator_Destroy() {
        defaultLabel = Strings.Label;
        defaultDesc = Strings.Description;
        icon = Textures.Designator;
        soundDragSustain = SoundDefOf.Designate_DragStandard;
        soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
        useMouseIcon = true;
        soundSucceeded = SoundDefOf.Designate_Deconstruct;
    }

    public override int DraggableDimensions => 2;

    protected override DesignationDef Designation
        => MyDefOf.Destroy;

    public override AcceptanceReport CanDesignateCell(IntVec3 loc)
        => AffectableAt(loc).Any();

    public override AcceptanceReport CanDesignateThing(Thing t)
        => CanAffect(t);

    public override void DesignateThing(Thing t) {
        if (DebugSettings.godMode) {
            t.Destroy(DestroyMode.KillFinalize);
        } else {
            Map.designationManager.AddDesignation(new Designation(t, Designation));
            Map.listerHaulables.Notify_DeSpawned(t);
        }
    }

    public override void DesignateSingleCell(IntVec3 c) {
        foreach (var t in AffectableAt(c)) {
            DesignateThing(t);
        }
    }
    public override void SelectedUpdate()
        => GenUI.RenderMouseoverBracket();

    private IEnumerable<Thing> AffectableAt(IntVec3 loc)
        => Map.thingGrid.ThingsAt(loc).Where(CanAffect);

    private bool CanAffect(Thing t)
        => CanAffect(t.def)
        && Map.designationManager.DesignationOn(t) == null
        && !(t is Building b && b.GetStatValue(StatDefOf.WorkToBuild) == 0f);

    private bool CanAffect(ThingDef t)
        => t.useHitPoints
        && t.destroyable
        && !t.mineable
        && (t.category == ThingCategory.Item || t.category == ThingCategory.Building);
}
