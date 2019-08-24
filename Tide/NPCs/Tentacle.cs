using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Tide.NPCs
{
	public class Tentacle : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cthulhu Tenetacle");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BoundGoblin];
		}

		public override void SetDefaults()
		{
			npc.width = 20;
			npc.height = 30;
			npc.damage = 30;
			npc.defense = 8;
			npc.lifeMax = 110;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 60f;
			npc.knockBackResist = 0f;
			npc.aiStyle = 0;
			aiType = NPCID.BoundGoblin;
			animationType = NPCID.BoundGoblin;
		}

		public override void AI()
		{
			Player target = Main.player[npc.target];
			int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
			if (distance < 600)
			{
				npc.ai[0]++;
				if (npc.ai[0] >= 120)
				{
					int type = mod.ProjectileType("WitherBolt");
					int p = Terraria.Projectile.NewProjectile(npc.position.X, npc.position.Y, -(npc.position.X - target.position.X) / distance * 4, -(npc.position.Y - target.position.Y) / distance * 4, type, (int)((npc.damage * .5)), 0);
					Main.projectile[p].friendly = false;
					Main.projectile[p].hostile = true;
					npc.ai[0] = 0;
				}
			}
			{
				int dust = Dust.NewDust(npc.position, npc.width, npc.height - 20, 173);
				npc.spriteDirection = npc.direction;
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int i = 0; i < 10; i++)
				;
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Tentacle"), 1f);
			}
		}
	}
}
