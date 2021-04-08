using Microsoft.Xna.Framework;
using SpiritMod.Items.Weapon.Gun;
using System;
using Terraria;
using Terraria.ID;
using SpiritMod.Items.Material;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BottomFeeder
{
	public class BottomFeeder : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bottom Feeder");
			Main.npcFrameCount[npc.type] = 11;
		}
		public override void SetDefaults()
		{
			npc.width = 60;
			npc.height = 68;
			npc.damage = 24;
			npc.defense = 9;
			npc.lifeMax = 175;
			npc.HitSound = SoundID.NPCHit18;
			npc.DeathSound = SoundID.NPCDeath5;
			npc.value = 800f;
			npc.knockBackResist = 0.34f;
			npc.aiStyle = 3;
			npc.noGravity = false;
			aiType = NPCID.WalkingAntlion;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.BottomFeederBanner>();
		}
		int frame = 1;
		int timer = 0;
		int shoottimer = 0;
		public override void AI()
		{
			npc.spriteDirection = -npc.direction;

			Player target = Main.player[npc.target];
			{
				timer++;
				Player player = Main.player[npc.target];
				int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));

				if (timer == 4) {
					frame++;
					timer = 0;
				}
				if (frame >= 8 && distance >= 180) {
					frame = 1;
				}
				else if (frame == 11 && distance < 180) {
					frame = 8;
				}
				if (distance < 178) {
					shoottimer++;
					if (!npc.wet) {
                        npc.velocity.X = .01f * npc.spriteDirection;
                        npc.spriteDirection = -npc.direction;
                        npc.velocity.Y = 10f;
                    }
					if (shoottimer >= 40 && shoottimer < 96) {
						if (Main.rand.Next(3) == 0 && Main.netMode != NetmodeID.MultiplayerClient) {
							int bloodproj;
							bloodproj = Main.rand.Next(new int[] { mod.ProjectileType("Feeder1"), mod.ProjectileType("Feeder2"), mod.ProjectileType("Feeder3") });
							bool expertMode = Main.expertMode;
							int damage = expertMode ? 10 : 15;
							int p = Terraria.Projectile.NewProjectile(npc.Center.X + (7 * npc.direction), npc.Center.Y - 10, -(npc.position.X - target.position.X) / distance * 8, -(npc.position.Y - target.position.Y + Main.rand.Next(-50, 50)) / distance * 8, bloodproj, damage, 0);
						}
					}
					if (shoottimer >= 96) {
						shoottimer = 0;
					}
				}

			}
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frame.Y = frameHeight * frame;
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(20) == 1) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<BottomFeederGun>());
			}
            if (Main.rand.Next(20) == 1)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Consumable.Food.FishFingers>());
            }
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<BloodFire>(), 2 + Main.rand.Next(2, 4));
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0 || npc.life >= 0) {
				int d = 5;
				for (int k = 0; k < 25; k++) {
					Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.47f);
					Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, .97f);
				}
			}
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity * 1.11f, mod.GetGoreSlot("Gores/FeederGore"), 1f);
				Gore.NewGore(npc.position, npc.velocity * 1.11f, mod.GetGoreSlot("Gores/FeederGore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity * 1.11f, mod.GetGoreSlot("Gores/FeederGore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity * 1.11f, mod.GetGoreSlot("Gores/FeederGore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity * 1.11f, mod.GetGoreSlot("Gores/FeederGore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity * 1.11f, mod.GetGoreSlot("Gores/FeederGore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity * 1.11f, mod.GetGoreSlot("Gores/FeederGore3"), 1f);
			}
		}
	}
}
