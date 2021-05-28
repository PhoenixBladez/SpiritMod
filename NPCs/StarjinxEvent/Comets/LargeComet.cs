using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using SpiritMod.Prim;

namespace SpiritMod.NPCs.StarjinxEvent.Comets
{
    public class LargeComet : SmallComet
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Large Starjinx Comet");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.lifeMax = 20;
            npc.width = 62;
            npc.height = 58;
            beamscale = 1.25f;
        }
    }
}