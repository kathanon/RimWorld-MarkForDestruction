using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace MarkForDestruction;
[HarmonyPatch]
public static class Patch_RemoveDesignation {
    [HarmonyPrefix]
    [HarmonyPatch(typeof(DesignationManager), nameof(RemoveDesignation))]
    public static void RemoveDesignation(Designation des) {
        Thing t;
        if (des.def == MyDefOf.Destroy && (t = des.target.Thing) != null) {
            t.Map.listerHaulables.Notify_Spawned(t);
        }
    }
}
