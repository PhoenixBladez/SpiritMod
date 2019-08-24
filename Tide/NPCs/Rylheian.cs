using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tide.NPCs
{
	[AutoloadBossHead]
	public class Rylheian : ModNPC
	{
		int timer = 0;
		int moveSpeed = 0;
		int moveSpeedY = 0;
		float HomeY = 150f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("R'lyehian");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Pixie];
		}

		public override void SetDefaults()
		{
			npc.width = 80;
			npc.height = 100;
			npc.damage = 42;
			npc.lifeMax = 2800;
			npc.knockBackResist = 0;

			npc.boss = true;
			npc.noGravity = true;
			npc.noTileCollide = true;

			animationType = NPCID.Pixie;
			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath5;
		}

		private int Counter;
		public override bool PreAI()
		{
			{
				npc.spriteDirection = npc.direction;
				Player player = Main.player[npc.target];
				if (npc.Center.X >= player.Center.X && moveSpeed >= -53) // flies to players x position
				{
					moveSpeed--;
				}

				if (npc.Center.X <= player.Center.X && moveSpeed <= 53)
				{
					moveSpeed++;
				}

				npc.velocity.X = moveSpeed * 0.1f;

				if (npc.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -30) //Flies to players Y position
				{
					moveSpeedY--;
					HomeY = 150f;
				}

				if (npc.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 30)
				{
					moveSpeedY++;
				}

				npc.velocity.Y = moveSpeedY * 0.1f;
				if (Main.rand.Next(220) == 6)
				{
					HomeY = -35f;
				}
				Counter++;
				if (Counter > 400)
				{
					int newNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Tentacle"), npc.whoAmI);
					Counter = 0;
				}
			}
			timer++;
			if (timer == 200 || timer == 250 || timer == 300 || timer == 350 || timer == 400 || timer == 450)
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
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X, direction.Y, mod.ProjectileType("WitherBolt"), 28, 1, Main.myPlayer, 0, 0);
				}
			}
			if (timer >= 500)
			{
				timer = 0;
			}
			return true;
		}

		public override void NPCLoot()
		{
			{
				string[] lootTable = { "CthulhuStaff2", "CthulhuStaff1" };
				int loot = Main.rand.Next(lootTable.Length);
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType(lootTable[loot]));
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (TideWorld.TheTide && TideWorld.InBeach && Main.hardMode && !NPC.AnyNPCs(mod.NPCType("Rylheian")))
				return 0.6f;

			return 0;
		}

		public override void AI()
		{
			int dust = Dust.NewDust(npc.position, npc.width, npc.height, 173);
			npc.spriteDirection = npc.direction;
		}

		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.GreaterHealingPotion;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int i = 0; i < 10; i++)
				;
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Tentacle"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/TentacleHead"), 1f);
				if (TideWorld.TheTide)
				{
					TideWorld.TidePoints2 += 3;
				}
			}
		}
	}
}
