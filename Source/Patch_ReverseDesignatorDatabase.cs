using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace MarkForDestruction;
[HarmonyPatch]
public static class Patch_ReverseDesignatorDatabase {
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ReverseDesignatorDatabase), "InitDesignators")]
    public static void InitDesignators(List<Designator> ___desList) 
        => ___desList.Add(new Designator_Destroy());
}
