using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.FrostTroll
{
	[AutoloadBossHead]
	public class FrostSaucer : ModNPC
	{
		int timer = 0;
		int moveSpeed = 0;
		int moveSpeedY = 0;
		float HomeY = 150f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snow Monger");
		}

		public override void SetDefaults()
		{
			npc.width = 344;
			npc.height = 298;
			npc.damage = 46;
			npc.lifeMax = 4500;
			npc.knockBackResist = 0;

			npc.boss = true;
			npc.noGravity = true;
			npc.noTileCollide = true;

			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath5;
		}

		private int Counter;
		public override bool PreAI()
		{
			Counter++;
			if (Counter > 1000 && !NPC.AnyNPCs(143))
			{
				int newNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, 143, npc.whoAmI);
				Counter = 0;
			}
			npc.netUpdate = true;
			npc.TargetClosest(true);
			npc.TargetClosest(false);
			npc.velocity.Y = -100;
			if (npc.ai[0] == 0)
			{
				npc.spriteDirection = npc.direction;
				Player player = Main.player[npc.target];
				if (npc.Center.X >= player.Center.X && moveSpeed >= -53) // flies to players x position
					moveSpeed--;
				else if (npc.Center.X <= player.Center.X && moveSpeed <= 53)
					moveSpeed++;

				npc.velocity.X = moveSpeed * 0.1f;

				if (npc.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -30) //Flies to players Y position
				{
					moveSpeedY--;
					HomeY = 150f;
				}
				else if (npc.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 30)
					moveSpeedY++;

				npc.velocity.Y = moveSpeedY * 0.1f;
				if (Main.rand.Next(220) == 6)
					HomeY = -35f;
			}
			npc.velocity.Y = moveSpeedY * 0.1f;

			timer++;
			if (timer == 200 || timer == 250)
			{
				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				direction.Normalize();
				direction.X *= 5f;
				direction.Y *= 5f;

				int amountOfProjectiles = Main.rand.Next(5, 6);
				for (int i = 0; i < amountOfProjectiles; ++i)
				{
					float A = (float)Main.rand.Next(-150, 150) * 0.03f;
					float B = (float)Main.rand.Next(-150, 150) * 0.03f;
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, 128, 29, 1, Main.myPlayer, 0, 0);
				}
			}
			else if (timer == 300 || timer == 340 || timer == 380 || timer == 420 || timer == 460 || timer == 470 || timer == 480 || timer == 490)
			{
				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				direction.Normalize();
				direction.X *= 14f;
				direction.Y *= 14f;

				int amountOfProjectiles = Main.rand.Next(1, 1);
				for (int i = 0; i < amountOfProjectiles; ++i)
				{
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X, direction.Y, mod.ProjectileType("SnowBall"), 32, 1, Main.myPlayer, 0, 0);
				}

			}
			else if (timer >= 500)
			{
				timer = 0;
			}
			return true;
		}

		public override void NPCLoot()
		{
			string[] lootTable = { "Bauble", "BlizzardEdge", "Chillrend", "ShiverWind" };
			int loot = Main.rand.Next(lootTable.Length);
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType(lootTable[loot]));

			for (int i = 0; i < 15; ++i)
			{
				if (Main.rand.Next(8) == 0)
				{
					int newDust = Dust.NewDust(npc.position, npc.width, npc.height, 76, 0f, 0f, 100, default(Color), 2.5f);
					Main.dust[newDust].noGravity = true;
					Main.dust[newDust].velocity *= 5f;
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return Main.invasionType == 2 && !NPC.AnyNPCs(mod.NPCType("FrostSaucer")) ? 0.04f : 0f;
		}

		public override void AI()
		{
			int dust = Dust.NewDust(npc.position, npc.width, npc.height - 40, 76);
			Main.dust[dust].noGravity = true;
		}

		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.GreaterHealingPotion;
		}
	}
}
