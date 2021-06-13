using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SpiritMod.VerletChains;

namespace SpiritMod.NPCs.ChainedSinner
{
    internal class ChainedSinnerBase : ModNPC
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Chained Sinner");

		public override void SetDefaults()
        {
            npc.width = 24;
            npc.height = 24;
            npc.knockBackResist = 0;
            npc.aiStyle = -1;
            npc.lifeMax = 50;
            npc.damage = 10;
            npc.defense = 4;
            npc.noTileCollide = true;
            npc.noGravity = true;
			npc.dontTakeDamage = true;
        }

        bool spawnedHead = false;
        public override void AI()
        {
            npc.velocity = Vector2.Zero;
            if (!spawnedHead)
            {
                spawnedHead = true;
                int child = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<ChainedSinner>());
                if (Main.npc[child].modNPC is ChainedSinner head)
                {
                    head.parentid = npc.whoAmI;
                    head.InitializeChain(npc.Center);
                }
            }
        }
    }
}