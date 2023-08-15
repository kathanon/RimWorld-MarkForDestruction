using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace MarkForDestruction;
public static class MyDefOf {
    public static readonly DesignationDef Destroy = MyDefOf_Load.kathanon_MarkForDestruction_Destroy;

    public static readonly JobDef Smash = MyDefOf_Load.kathanon_MarkForDestruction_Smash;

    public static readonly JobDef Incinerate = MyDefOf_Load.kathanon_MarkForDestruction_Incinerate;

    public static readonly SpecialThingFilterDef NonBurnableWeapons = MyDefOf_Load.AllowNonBurnableWeapons;

    public static readonly SpecialThingFilterDef NonBurnableApparel = MyDefOf_Load.AllowNonBurnableApparel;

    public static readonly ThingDef ElectricCrematorium = MyDefOf_Load.ElectricCrematorium;

    public static readonly RecipeDef CremateCorpse = MyDefOf_Load.CremateCorpse;

    public static readonly RecipeDef BurnApparel = MyDefOf_Load.BurnApparel;
}

[DefOf]
internal static class MyDefOf_Load {
    public static DesignationDef kathanon_MarkForDestruction_Destroy;

    public static JobDef kathanon_MarkForDestruction_Smash;

    public static JobDef kathanon_MarkForDestruction_Incinerate;

    public static ThingDef ElectricCrematorium;

    public static SpecialThingFilterDef AllowNonBurnableApparel;

    public static SpecialThingFilterDef AllowNonBurnableWeapons;

    public static RecipeDef CremateCorpse;

    public static RecipeDef BurnApparel;

    static MyDefOf_Load() {
        DefOfHelper.EnsureInitializedInCtor(typeof(MyDefOf_Load));
    }
}

