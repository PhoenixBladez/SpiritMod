using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
namespace SpiritMod.NPCs
{
	public class Observer : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Observer");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.AngryNimbus];
		}

		public override void SetDefaults()
		{
			npc.width = 36;
			npc.height = 44;
			npc.damage = 44;
			npc.defense = 20;
			npc.lifeMax = 340;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 990f;
			npc.knockBackResist = .10f;
			npc.aiStyle = 22;
			npc.noGravity = true;
			npc.noTileCollide = true;
			aiType = NPCID.Wraith;
			animationType = NPCID.AngryNimbus;
		}

		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			Player target = Main.player[npc.target];
			int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
			if (distance < 320)
			{
				npc.ai[3]++;
				if (npc.ai[3] >= 100)
				{
					int type = mod.ProjectileType("PoisonGlob");
					int p = Terraria.Projectile.NewProjectile(npc.position.X + 5, npc.position.Y + 8, -(npc.position.X - target.position.X) / distance * 4, -(npc.position.Y - target.position.Y) / distance * 4, type, (int)((npc.damage * .5)), 0);
					Main.projectile[p].friendly = false;
					Main.projectile[p].hostile = true;
					npc.ai[3] = 0;
				}
			}
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.05f, 0.3f, 0.04f);

		}

		public override void HitEffect(int hitDirection, double damage)
		{
			int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 107, 0f, 0f, 100, default(Color), 2f);

			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Observer_gore"));
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.playerSafe || !Main.hardMode)
			{
				return 0f;
			}
			return SpawnCondition.Cavern.Chance * 0.024478f;
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(4) == 0)
			{
				target.AddBuff(BuffID.Poisoned, 160);
			}
		}

		public override void NPCLoot()
		{
			if (Main.rand.Next(2) == 1)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Acid"));
		}
	}
}
