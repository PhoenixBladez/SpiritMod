using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Flail;
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
			Main.npcFrameCount[npc.type] = 12;
		}

		public override void SetDefaults()
		{
			npc.width = 40;
			npc.height = 60;
			npc.damage = 16;
			npc.defense = 7;
			npc.lifeMax = 63;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath53;
			npc.value = 560f;
			npc.knockBackResist = .55f;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.SporeWheezerBanner>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			int x = spawnInfo.spawnTileX;
			int y = spawnInfo.spawnTileY;
			int tile = (int)Main.tile[x, y].type;
			return (tile == TileID.MushroomGrass) && spawnInfo.spawnTileY > Main.rockLayer ? 3f : 0f;

		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 11; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, 42, hitDirection, -1f, 0, default(Color), .61f);
			}
			if (npc.life <= 0) {
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
            /*int Techs = Main.rand.Next(1, 4);
			for (int J = 0; J <= Techs; J++) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Carapace>());
			}*/
            int lootcount1 = Main.rand.Next(3, 6);
			for (int J = 0; J <= lootcount1; J++) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GlowingMushroom);
			}
			if (Main.rand.Next(15) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<WheezerScale>());
			if (Main.rand.Next(80) == 0) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.DepthMeter);
            }
            if (Main.rand.NextBool(60))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<ClatterMace>());
            }
            if (Main.rand.Next(80) == 0) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Compass);
			}
			if (Main.rand.Next(200) == 0) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Rally);
            }
            string[] lootTable = { "ClatterboneBreastplate", "ClatterboneFaceplate", "ClatterboenLeggings" };
            if (Main.rand.Next(55) == 0)
            {
                int loot = Main.rand.Next(lootTable.Length);
                {
                    npc.DropItem(mod.ItemType(lootTable[loot]));
                }
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
			if (distance < 200) {
				npc.velocity = Vector2.Zero;
				if (npc.velocity == Vector2.Zero) {
					npc.aiStyle = 0;
					if (target.position.X > npc.position.X) {
						npc.spriteDirection = 1;
					}
					else {
						npc.spriteDirection = -1;
					}
					npc.velocity.Y = 12f;
				}
				float num395 = Main.mouseTextColor / 200f - 0.35f;
				num395 *= 0.2f;
				npc.scale = num395 + 0.95f;
				shootTimer++;
				if (shootTimer >= 90) {
					Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 95);
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
					Vector2 direction = Main.player[npc.target].Center - npc.Center;
					direction.Normalize();
					direction.X *= 5f;
					direction.Y *= 5f;

					int amountOfProjectiles = Main.rand.Next(2, 4);
					bool expertMode = Main.expertMode;
					int damage = expertMode ? 8 : 13;
					for (int i = 0; i < amountOfProjectiles; ++i) {
						float A = (float)Main.rand.Next(-50, 50) * 0.02f;
						float B = (float)Main.rand.Next(-60, -40) * 0.1f;
						int p = Projectile.NewProjectile(npc.Center.X + (npc.direction * 12), npc.Center.Y - 10, direction.X + A, direction.Y + B, ModContent.ProjectileType<WheezerSporeHostile>(), damage, 1, Main.myPlayer, 0, 0);
						for (int k = 0; k < 11; k++) {
							Dust.NewDust(npc.position, npc.width, npc.height, 42, direction.X + A, direction.Y + B, 0, default(Color), .61f);
						}
						Main.projectile[p].hostile = true;
					}
					}
					shootTimer = 0;
				}
				timer++;
				if (timer == 4) {
					frame++;
					timer = 0;
				}
				if (frame >= 11) {
					frame = 7;
				}
			}
			else {
				npc.scale = 1f;
				shootTimer = 0;
				npc.aiStyle = 3;
				aiType = NPCID.SnowFlinx;
				timer++;
				if (timer == 4) {
					frame++;
					timer = 0;
				}
				if (frame >= 6) {
					frame = 0;
				}
			}
			if (shootTimer > 120) {
				shootTimer = 120;
			}
			if (shootTimer < 0) {
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
