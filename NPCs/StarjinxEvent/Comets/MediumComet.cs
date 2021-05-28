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
    public class MediumComet : SmallComet
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Medium Starjinx Comet");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.lifeMax = 15;
            npc.width = 42;
            npc.height = 40;
            beamscale = 1f;
        }
    }
}