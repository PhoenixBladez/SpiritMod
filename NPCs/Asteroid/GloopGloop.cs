using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Asteroid
{
	public class GloopGloop : ModNPC
	{
		int counter = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gloop Gloop");
			Main.npcFrameCount[npc.type] = 3;
		}

		public override void SetDefaults()
		{
			npc.width = 32;
			npc.height = 48;
			npc.damage = 18;
			npc.defense = 10;
			npc.lifeMax = 170;
			npc.noGravity = true;
			npc.value = 800f;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath22;
		}
        public override void AI()
        {
			npc.rotation = npc.velocity.ToRotation() + 1.57f;
            counter++;
			npc.velocity *= 0.98f;
			if (counter > 65)
			{
				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				direction.Normalize();
				direction *= 12;
				npc.velocity = direction;
				counter = 0;
			}
        }

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
	}
}
