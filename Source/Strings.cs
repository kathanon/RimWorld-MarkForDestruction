using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace MarkForDestruction {
    public static class Strings {
        public const string ID = "kathanon.MarkForDestruction";
        public const string Name = "Mark For Destruction";

        public static readonly string Description = (ID + ".Description").Translate();
        public static readonly string Label       = (ID + ".Label"      ).Translate();
    }
}
