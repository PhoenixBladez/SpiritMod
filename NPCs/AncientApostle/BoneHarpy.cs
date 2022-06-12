using Microsoft.Xna.Framework;
using SpiritMod.Items.Consumable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Buffs;
using SpiritMod.Buffs.DoT;

namespace SpiritMod.NPCs.AncientApostle
{
	public class BoneHarpy : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Apostle");
			Main.npcFrameCount[npc.type] = 7;
		}

		public override void SetDefaults()
		{
			npc.width = 32;
			npc.height = 34;
			npc.damage = 16;
			npc.defense = 10;
			npc.lifeMax = 70;
			npc.noGravity = true;
			npc.value = 90f;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[BuffID.Confused] = true;
			npc.buffImmune[ModContent.BuffType<FesteringWounds>()] = true;
			npc.buffImmune[ModContent.BuffType<BloodCorrupt>()] = true;
			npc.buffImmune[ModContent.BuffType<BloodInfusion>()] = true;
			npc.noTileCollide = false;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath6;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.AncientApostleBanner>();
		}

		private ref float MoveSpeed => ref npc.ai[1];
		private ref float MoveSpeedY => ref npc.ai[2];
		private ref float Counter => ref npc.ai[3];

		public override void AI()
		{
			if (Counter == 0)
				npc.ai[0] = 150;

			Counter++;
			Player player = Main.player[npc.target];
			npc.rotation = npc.velocity.X * 0.1f;
			npc.spriteDirection = npc.direction;

			if (npc.Center.X >= player.Center.X && MoveSpeed >= -60) // flies to players x position
				MoveSpeed--;

			if (npc.Center.X <= player.Center.X && MoveSpeed <= 60)
				MoveSpeed++;

			npc.velocity.X = MoveSpeed * 0.06f;

			if (npc.Center.Y >= player.Center.Y - npc.ai[0] && MoveSpeedY >= -50) //Flies to players Y position
			{
				MoveSpeedY--;
				npc.ai[0] = 150f;
			}

			if (npc.Center.Y <= player.Center.Y - npc.ai[0] && MoveSpeedY <= 50)
				MoveSpeedY++;

			npc.velocity.Y = MoveSpeedY * 0.12f;
			if (Main.rand.Next(220) == 8 && Main.netMode != NetmodeID.MultiplayerClient)
			{
				npc.ai[0] = -25f;
				npc.netUpdate = true;
			}

			if (Counter >= 240) //Fires desert feathers like a shotgun
			{
				Counter = 0;
				Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 73);
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Vector2 direction = Main.player[npc.target].Center - npc.Center;
					direction.Normalize();
					direction.X *= 11f;
					direction.Y *= 11f;

					int amountOfProjectiles = 3;
					for (int i = 0; i < amountOfProjectiles; ++i)
					{
						float A = (float)Main.rand.Next(-150, 150) * 0.01f;
						float B = (float)Main.rand.Next(-150, 150) * 0.01f;
						int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<NPCs.Boss.DesertFeather>(), 11, 1, Main.myPlayer, 0, 0);
						Main.projectile[p].scale = .6f;
					}
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.sky && !Main.LocalPlayer.GetSpiritPlayer().ZoneAsteroid ? 0.16f : 0f;

		public override void NPCLoot()
		{
			if (Main.rand.Next(6) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<JewelCrown>());

			if (Main.rand.Next(2) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Feather);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Stone, 2.5f * hitDirection, -2.5f, 0, Color.White, Main.rand.NextFloat(.2f, .8f));
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Stone, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}

			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Apostle2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Apostle3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Apostle4"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Apostle1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Apostle5"), 1f);
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.21f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
	}
}