using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.MarbleSet;
using SpiritMod.Items.Weapon.Yoyo;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Food;
using SpiritMod.Utilities;

namespace SpiritMod.NPCs.Beholder
{
	[AutoloadBossHead]
	public class Beholder : ModNPC, IBCRegistrable
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Beholder");
			Main.npcFrameCount[npc.type] = 9;
			NPCID.Sets.TrailCacheLength[npc.type] = 3;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.width = 72;
			npc.height = 68;
			npc.damage = 33;
			npc.defense = 15;
			npc.lifeMax = 490;
			npc.HitSound = SoundID.DD2_CrystalCartImpact;
			npc.DeathSound = SoundID.NPCDeath2;
			npc.value = 760f;
			npc.knockBackResist = 0.35f;
			npc.noTileCollide = true;
			npc.noGravity = true;
			npc.aiStyle = 14;
			npc.noGravity = true;
			aiType = NPCID.Slimer;
			banner = npc.type;
			npc.rarity = 3;
			bannerItem = ModContent.ItemType<Items.Banners.BeholderBanner>();

			npc.noTileCollide = true;
		}
		public int dashTimer;
		int frame = 0;
		int timer = 0;
		int shootTimer = 0;
		public override bool PreAI()
		{
			Player player = Main.player[npc.target];
			float num5 = npc.position.X + (float)(npc.width / 2) - player.position.X - (float)(player.width / 2);
			float num6 = npc.position.Y + (float)npc.height - 59f - player.position.Y - (float)(player.height / 2);
			float num7 = (float)Math.Atan2((double)num6, (double)num5) + 1.57f;
			if (num7 < 0f) {
				num7 += 6.283f;
			}
			else if ((double)num7 > 6.283) {
				num7 -= 6.283f;
			}
			float num8 = 0.1f;
			if (npc.rotation < num7) {
				if ((double)(num7 - npc.rotation) > 3.1415) {
					npc.rotation -= num8;
				}
				else {
					npc.rotation += num8;
				}
			}
			else if (npc.rotation > num7) {
				if ((double)(npc.rotation - num7) > 3.1415) {
					npc.rotation += num8;
				}
				else {
					npc.rotation -= num8;
				}
			}
			if (npc.rotation > num7 - num8 && npc.rotation < num7 + num8) {
				npc.rotation = num7;
			}
			if (npc.rotation < 0f) {
				npc.rotation += 6.283f;
			}
			else if ((double)npc.rotation > 6.283) {
				npc.rotation -= 6.283f;
			}
			if (npc.rotation > num7 - num8 && npc.rotation < num7 + num8) {
				npc.rotation = num7;
			}
			npc.spriteDirection = npc.direction;
			return true;
		}
		bool manaSteal = false;
		int manaStealTimer;
		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			Player player = Main.player[npc.target];

			npc.TargetClosest(true);
			dashTimer++;
			if (dashTimer == 210 || dashTimer == 420 || dashTimer == 630) {
				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				direction.Normalize();
				Main.PlaySound(SoundID.DD2_WyvernDiveDown, npc.Center);
				direction.X = direction.X * Main.rand.Next(6, 9);
				direction.Y = direction.Y * Main.rand.Next(6, 9);
				npc.velocity.X = direction.X;
				npc.velocity.Y = direction.Y;
				npc.velocity *= 0.98f;
			}
			timer++;
			if (timer >= 6) {
				frame++;
				timer = 0;
			}
			if (frame >= 7) {
				frame = 0;
			}
			if (dashTimer == 770 && Main.netMode != NetmodeID.MultiplayerClient) {
				Main.PlaySound(SoundID.DD2_WitherBeastAuraPulse, npc.Center);
				npc.position.X = player.position.X + Main.rand.NextFloat(-200f, 200f);
				npc.position.Y = player.position.Y + Main.rand.NextFloat(-100f, -200f);
				
				npc.netUpdate = true;
			}
			if (dashTimer == 771)
			{
				for (int i = 0; i < 30; i++) {
					Vector2 vector23 = Vector2.UnitY.RotatedByRandom(6.28318548202515) * new Vector2(100f, 20f) * npc.scale * 1.85f / 2f;
					int index1 = Dust.NewDust(npc.Center + vector23, 0, 0, DustID.GoldCoin, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index1].position = npc.Center + vector23;
					Main.dust[index1].velocity = Vector2.Zero;
					Vector2 vector25 = Vector2.UnitY.RotatedByRandom(6.28318548202515) * new Vector2(20f, 100f) * npc.scale * 1.85f / 2f;
					int index3 = Dust.NewDust(npc.Center + vector25, 0, 0, DustID.GoldCoin, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index3].position = npc.Center + vector25;
					Main.dust[index3].velocity = Vector2.Zero;
				}
			}
			if (dashTimer == 800) {
				Main.PlaySound(SoundID.DD2_WyvernScream, npc.Center);
			}
			if (dashTimer >= 800 && dashTimer <= 1000) {
				frame = 8;
				npc.velocity.X *= .008f * npc.direction;
				npc.velocity.Y *= 0f;
				shootTimer++;
				if (shootTimer >= 40 && Main.netMode != NetmodeID.MultiplayerClient) {
					Vector2 direction = Main.player[npc.target].Center - npc.Center;
					direction.Normalize();
					direction.X *= 5f;
					direction.Y *= 5f;
					shootTimer = 0;
					int amountOfProjectiles = 1;
					bool expertMode = Main.expertMode;
					int damage = expertMode ? 11 : 16;
					for (int i = 0; i < amountOfProjectiles; ++i) {
						float A = (float)Main.rand.Next(-50, 50) * 0.02f;
						float B = (float)Main.rand.Next(-50, 50) * 0.02f;
						int p = Projectile.NewProjectile(npc.Center.X + (npc.direction * 12), npc.Center.Y + 20, direction.X + A, direction.Y + B, ProjectileID.Fireball, damage, 1, Main.myPlayer, 0, 0);
						for (int k = 0; k < 11; k++) {
							Dust.NewDust(npc.position, npc.width, npc.height, DustID.Fire, direction.X + A, direction.Y + B, 0, default, .61f);
						}
						Main.projectile[p].hostile = true;
					}
				}
			}
			else {
				shootTimer = 0;
			}
			if (dashTimer >= 1020) {
				dashTimer = 0;
			}
			if (manaSteal) {
				manaStealTimer++;
				int distance = (int)Math.Sqrt((npc.Center.X - player.Center.X) * (npc.Center.X - player.Center.X) + (npc.Center.Y - player.Center.Y) * (npc.Center.Y - player.Center.Y));
				if (distance < 300 && player.statMana > 0) {
					player.statMana--;
					for (int i = 0; i < 2; i++) {
						int dust = Dust.NewDust(npc.Center, npc.width, npc.height, DustID.PurificationPowder);
						Main.dust[dust].velocity *= -1f;
						Main.dust[dust].scale *= 1.4f;
						Main.dust[dust].noGravity = true;
						Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
						vector2_1.Normalize();
						Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
						Main.dust[dust].velocity = vector2_2;
						vector2_2.Normalize();
						Vector2 vector2_3 = vector2_2 * 64f;
						Main.dust[dust].position = npc.Center - vector2_3;
					}
				}
			}
			else {
				manaStealTimer = 0;
			}
			if (manaStealTimer >= 120) {
				manaSteal = false;
				manaStealTimer = 0;
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            int x = spawnInfo.spawnTileX;
            int y = spawnInfo.spawnTileY;
            int tile = (int)Main.tile[x, y].type;
            return spawnInfo.player.GetSpiritPlayer().ZoneMarble && NPC.downedBoss2 && !NPC.AnyNPCs(ModContent.NPCType<Beholder>()) && spawnInfo.spawnTileY > Main.rockLayer ? 0.002765f : 0f;

        }
		public override void FindFrame(int frameHeight)
		{
			npc.frame.Y = frameHeight * frame;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height * 0.5f));
			for (int k = 0; k < npc.oldPos.Length; k++) {
				var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
				Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
				spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
			}
			return true;
		}
        public override bool PreNPCLoot()
        {
            Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/DownedMiniboss"));
            MyWorld.downedBeholder = true;
            return true;
        }
        public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0) {
				Main.PlaySound(SoundID.DD2_WyvernScream, npc.Center);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Beholder1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Beholder2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Beholder3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Beholder4"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Beholder5"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Beholder6"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Beholder7"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Beholder8"), 1f);
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(4) == 1) {
				target.AddBuff(BuffID.Bleeding, 180);
			}
			manaSteal = true;
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<MarbleChunk>(), Main.rand.Next(4, 7) + 1);
			if (Main.rand.Next(2) == 0) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<BeholderYoyo>());
            }
            if (Main.rand.NextBool(5))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<TofuSatay>());
            }
        }

		public void RegisterToChecklist(out BossChecklistDataHandler.EntryType entryType, out float progression,
			out string name, out Func<bool> downedCondition, ref BossChecklistDataHandler.BCIDData identificationData,
			ref string spawnInfo, ref string despawnMessage, ref string texture, ref string headTextureOverride,
			ref Func<bool> isAvailable)
		{
			entryType = BossChecklistDataHandler.EntryType.Miniboss;
			progression = 3.2f;
			name = "Beholder";
			downedCondition = () => MyWorld.downedBeholder;
			identificationData = new BossChecklistDataHandler.BCIDData(
				new List<int> {
					ModContent.NPCType<Beholder>()
				},
				null,
				null,
				new List<int> {
					ModContent.ItemType<BeholderYoyo>(),
					ModContent.ItemType<MarbleChunk>()
				});
			spawnInfo =
				"The Beholder spawns rarely in Marble Caverns after the Eater of Worlds or Brain of Cthulhu has been defeated.";
			texture = "SpiritMod/Textures/BossChecklist/BeholderTexture";
			headTextureOverride = "SpiritMod/NPCs/Beholder_Head_Boss";
		}
	}
}
