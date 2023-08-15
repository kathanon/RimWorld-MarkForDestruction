using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace MarkForDestruction;
public class WorkGiver_Smash : WorkGiver_Destroy {
    protected override JobDef JobDef 
        => MyDefOf.Smash;
}
