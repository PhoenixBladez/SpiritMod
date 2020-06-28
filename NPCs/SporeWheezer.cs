using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Hostile;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class SporeWheezer : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spore Wheezer");
			Main.npcFrameCount[npc.type] = 7;
		}

		public override void SetDefaults()
		{
			npc.width = 40;
			npc.height = 60;
			npc.damage = 20;
			npc.defense = 7;
			npc.lifeMax = 90;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath53;
			npc.value = 560f;
			npc.knockBackResist = .55f;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			int x = spawnInfo.spawnTileX;
			int y = spawnInfo.spawnTileY;
			int tile = (int)Main.tile[x, y].type;
			return (tile == TileID.MushroomGrass) && NPC.downedBoss1 && spawnInfo.spawnTileY > Main.rockLayer ? 3f : 0f;

		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for(int k = 0; k < 11; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, 42, hitDirection, -1f, 0, default(Color), .61f);
			}
			if(npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SporeWheezer1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SporeWheezer2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SporeWheezer3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SporeWheezer4"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SporeWheezer4"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SporeWheezer4"), 1f);
			}
		}

		public override void NPCLoot()
		{
			int lootcount = Main.rand.Next(1, 3);
			for(int J = 0; J <= lootcount; J++) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Carapace>());
			}
			int lootcount1 = Main.rand.Next(3, 6);
			for(int J = 0; J <= lootcount1; J++) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GlowingMushroom);
			}
			if(Main.rand.Next(15) == 1)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<WheezerScale>());
			if(Main.rand.Next(10000) == 125) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.DepthMeter);
			}
			if(Main.rand.Next(10000) == 125) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Compass);
			}
			if(Main.rand.Next(1000) == 39) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Rally);
			}
		}

		int frame = 0;
		int timer = 0;
		int shootTimer = 0;
		public override void AI()
		{
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0f, 0.1f, 0.15f);
			npc.spriteDirection = npc.direction;
			Player target = Main.player[npc.target];
			int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
			if(distance < 200) {
				npc.velocity = Vector2.Zero;
				if(npc.velocity == Vector2.Zero) {
					npc.velocity.X = .008f * npc.direction;
					npc.velocity.Y = 12f;
				}
				float num395 = Main.mouseTextColor / 200f - 0.35f;
				num395 *= 0.2f;
				npc.scale = num395 + 0.95f;
				shootTimer++;
				if(shootTimer >= 90) {
					Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 95);
					Vector2 direction = Main.player[npc.target].Center - npc.Center;
					direction.Normalize();
					direction.X *= 5f;
					direction.Y *= 5f;

					int amountOfProjectiles = Main.rand.Next(2, 4);
					bool expertMode = Main.expertMode;
					int damage = expertMode ? 8 : 13;
					for(int i = 0; i < amountOfProjectiles; ++i) {
						float A = (float)Main.rand.Next(-50, 50) * 0.02f;
						float B = (float)Main.rand.Next(-60, -40) * 0.1f;
						int p = Projectile.NewProjectile(npc.Center.X + (npc.direction * 12), npc.Center.Y - 10, direction.X + A, direction.Y + B, ModContent.ProjectileType<WheezerSporeHostile>(), damage, 1, Main.myPlayer, 0, 0);
						for(int k = 0; k < 11; k++) {
							Dust.NewDust(npc.position, npc.width, npc.height, 42, direction.X + A, direction.Y + B, 0, default(Color), .61f);
						}
						Main.projectile[p].hostile = true;
					}
					shootTimer = 0;
				}
				frame = 0;
			} else {
				npc.scale = 1f;
				shootTimer = 0;
				npc.aiStyle = 3;
				aiType = NPCID.Skeleton;
				timer++;
				if(timer == 4) {
					frame++;
					timer = 0;
				}
				if(frame >= 6) {
					frame = 0;
				}
			}
			if(shootTimer > 120) {
				shootTimer = 120;
			}
			if(shootTimer < 0) {
				shootTimer = 0;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/SporeWheezer_Glow"));
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frame.Y = frameHeight * frame;
		}
	}
}
