using Microsoft.Xna.Framework;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Hostile;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class Wheezer : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wheezer");
			Main.npcFrameCount[npc.type] = 16;
		}

		public override void SetDefaults()
		{
			npc.width = 40;
			npc.height = 36;
			npc.damage = 18;
			npc.defense = 9;
			npc.lifeMax = 60;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath53;
			npc.value = 960f;
			npc.knockBackResist = .35f;
            banner = npc.type;
            bannerItem = ModContent.ItemType<Items.Banners.WheezerBanner>();
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if(spawnInfo.playerSafe || !NPC.downedBoss1) {
				return 0f;
			}
			return SpawnCondition.Cavern.Chance * 0.08f;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for(int k = 0; k < 11; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, 5, hitDirection, -1f, 0, default(Color), .61f);
			}
			if(npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Wheezer_Head"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Wheezer_legs"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Wheezer_legs"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Wheezer_legs"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Wheezer_legs"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Wheezer_tail"), 1f);
			}
		}

		public override void NPCLoot()
		{
			if(Main.LocalPlayer.GetSpiritPlayer().emptyWheezerScroll) {
				MyWorld.numWheezersKilled++;
			}
			int Techs = Main.rand.Next(1, 4);
			for(int J = 0; J <= Techs; J++) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Carapace>());
			}
			if(Main.rand.Next(15) == 1) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<WheezerScale>());
			}
			if(Main.rand.Next(10000) == 125) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.DepthMeter);
			}
			if(Main.rand.Next(10000) == 125) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Compass);
			}
			if(Main.rand.Next(1000) == 39) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Rally);
			}
			if(Main.rand.NextBool(100))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Bezoar);
		}

		int frame = 0;
		int timer = 0;
		int shootTimer = 0;
		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			Player target = Main.player[npc.target];
			int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
			if(distance < 200) {
				npc.velocity = Vector2.Zero;
				if(npc.velocity == Vector2.Zero) {
					npc.velocity.X = .008f * npc.direction;
					npc.velocity.Y = 12f;
				}
				shootTimer++;
				if(shootTimer >= 80) {
					Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 95);
					Vector2 direction = Main.player[npc.target].Center - npc.Center;
					direction.Normalize();
					direction.X *= 5f;
					direction.Y *= 5f;

					int amountOfProjectiles = 1;
					for(int i = 0; i < amountOfProjectiles; ++i) {
						float A = (float)Main.rand.Next(-50, 50) * 0.02f;
						float B = (float)Main.rand.Next(-50, 50) * 0.02f;
						int p = Projectile.NewProjectile(npc.Center.X + (npc.direction * 20), npc.Center.Y - 10, direction.X + A, direction.Y + B, ModContent.ProjectileType<WheezerCloud>(), npc.damage / 3 * 2, 1, Main.myPlayer, 0, 0);
						for(int k = 0; k < 11; k++) {
							Dust.NewDust(npc.position, npc.width, npc.height, 5, direction.X + A, direction.Y + B, 0, default(Color), .61f);
						}
						Main.projectile[p].hostile = true;
					}
					shootTimer = 0;
				}
				timer++;
				if(timer == 4) {
					frame++;
					timer = 0;
				}
				if(frame >= 15) {
					frame = 11;
				}
			} else {
				shootTimer = 0;
				npc.aiStyle = 3;
				aiType = NPCID.Skeleton;
				timer++;
				if(timer == 4) {
					frame++;
					timer = 0;
				}
				if(frame >= 9) {
					frame = 1;
				}
			}
			if(shootTimer > 120) {
				shootTimer = 120;
			}
			if(shootTimer < 0) {
				shootTimer = 0;
			}
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frame.Y = frameHeight * frame;
		}
	}
}
