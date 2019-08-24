using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;


namespace SpiritMod.NPCs
{
	public class Automaton : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lihzahrd Automaton");
			Main.npcFrameCount[npc.type] = 9;
		}

		public override void SetDefaults()
		{
			npc.width = 68;
			npc.height = 74;
			npc.damage = 50;
			npc.defense = 31;
			npc.lifeMax = 540;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 1260f;
			npc.knockBackResist = 0.15789f;
			npc.aiStyle = 41;
			aiType = NPCID.Herpling;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			Player target = Main.player[npc.target];
			int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
			npc.position.X = npc.position.X + (float)(npc.width / 2);
			npc.position.Y = npc.position.Y + (float)(npc.height / 2);
			npc.width = 30;
			npc.height = 30;
			npc.position.X = npc.position.X - (float)(npc.width / 2);
			npc.position.Y = npc.position.Y - (float)(npc.height / 2);
			int p = Terraria.Projectile.NewProjectile(npc.position.X + 5, npc.position.Y + 8, -(npc.position.X - target.position.X) / distance * 4, -(npc.position.Y - target.position.Y) / distance * 4, ProjectileID.GreekFire1, (int)((npc.damage * .75)), 0);

			if (npc.life <= 0)
			{
				for (int num621 = 0; num621 < 20; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
			}
		}

		public override void AI()
		{
			Player target = Main.player[npc.target];
			npc.spriteDirection = npc.direction;
			int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
			if (distance < 320)
			{
				npc.ai[3]++;
				if (npc.ai[3] >= 400)
				{
					int type = ProjectileID.Fireball;
					int p = Terraria.Projectile.NewProjectile(npc.position.X + 5, npc.position.Y + 8, -(npc.position.X - target.position.X) / distance * 4, -(npc.position.Y - target.position.Y) / distance * 4, type, (int)((npc.damage * .5)), 0);
					Main.projectile[p].friendly = false;
					Main.projectile[p].hostile = true;
					npc.ai[3] = 0;
				}
			}
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SunShard"), 1);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (SpawnHelper.SupressSpawns(spawnInfo, SpawnFlags.Lihzahrd, SpawnZones.None, SpawnFlags.SafeWall))
				return 0;

			return SpawnCondition.JungleTemple.Chance * 0.456f;
		}
	}
}
