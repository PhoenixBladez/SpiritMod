using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Sets.BriarDrops;
using SpiritMod.Items.Sets.HuskstalkSet;
using SpiritMod.Items.Sets.GladeWraithDrops;
using SpiritMod.Projectiles;
using SpiritMod.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;

namespace SpiritMod.NPCs.Reach
{
	[AutoloadBossHead]
	public class ForestWraith : ModNPC, IBCRegistrable
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glade Wraith");
			Main.npcFrameCount[NPC.type] = 12;
		}

		public override void SetDefaults()
		{
			NPC.width = 44;
			NPC.height = 82;
			NPC.damage = 28;
			NPC.defense = 10;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.lifeMax = 300;
			NPC.HitSound = SoundID.NPCHit7;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 541f;
			NPC.knockBackResist = 0.05f;
			NPC.noGravity = true;
			NPC.chaseable = true;
			NPC.noTileCollide = true;
			NPC.aiStyle = 44;
			AIType = NPCID.FlyingFish;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.GladeWraithBanner>();
		}

		bool throwing = false;

		public override void FindFrame(int frameHeight)
		{
			if (!throwing)
			{
				NPC.frameCounter += 0.15f;
				NPC.frameCounter %= 4;
				int frame = (int)NPC.frameCounter;
				NPC.frame.Y = frame * frameHeight;
			}
			else
			{
				NPC.frameCounter += 0.15f;
				NPC.frameCounter %= 8;
				int frame = (int)NPC.frameCounter;
				NPC.frame.Y = (frame + 4) * frameHeight;
			}
		}

		int timer = 0;
		bool thrown = false;

		public override bool PreAI()
		{
			NPC.collideX = false;
			NPC.collideY = false;
			Lighting.AddLight((int)((NPC.position.X + (NPC.width / 2)) / 16f), (int)((NPC.position.Y + (NPC.height / 2)) / 16f), 0.46f, 0.32f, .1f);
			timer++;
			if (timer == 240 || timer == 280 || timer == 320)
			{
				SoundEngine.PlaySound(SoundID.Grass, NPC.Center);
				Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
				direction.Normalize();
				direction.X *= 10f;
				direction.Y *= 10f;
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					int amountOfProjectiles = 1;
					for (int i = 0; i < amountOfProjectiles; ++i)
					{
						float A = Main.rand.Next(-120, 120) * 0.01f;
						float B = Main.rand.Next(-120, 120) * 0.01f;
						int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<OvergrowthLeaf>(), 6, 1, Main.myPlayer, 0, 0);
						Main.projectile[p].hostile = true;
						Main.projectile[p].friendly = false;
					}
				}
				NPC.netUpdate = true;
			}

			if (timer >= 500 && timer <= 720)
			{
				throwing = true;
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, 0f, -2.5f, 0, default, 0.6f);
				NPC.defense = 0;
				NPC.velocity = Vector2.Zero;

				if ((int)NPC.frameCounter == 4 && !thrown)
				{
					thrown = true;
					Vector2 direction = NPC.GetArcVel(Main.player[NPC.target].Center, 0.4f, 100, 500, maxXvel: 14);
					SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
					if (Main.netMode != NetmodeID.MultiplayerClient)
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction, ModContent.ProjectileType<LittleBouncingSpore>(), 8, 1, Main.myPlayer, 0, 0);
				}

				if ((int)NPC.frameCounter != 4)
					thrown = false;

				NPC.netUpdate = true;
			}
			else
				throwing = false;

			if (timer >= 730)
			{
				NPC.defense = 10;
				Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
				direction.Normalize();
				SoundEngine.PlaySound(SoundID.Zombie7, NPC.Center);
				direction.X *= Main.rand.Next(6, 9);
				direction.Y *= Main.rand.Next(6, 9);
				NPC.velocity.X = direction.X;
				NPC.velocity.Y = direction.Y;
				NPC.velocity *= 0.97f;
				timer = 0;
				NPC.netUpdate = true;
			}

			if (timer >= 750)
			{
				timer = 0;
				NPC.netUpdate = true;
			}

			NPC.spriteDirection = NPC.direction;
			return true;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GrassBlades, 2.5f * hitDirection, -2.5f, 0, default, 0.3f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, 7, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}

			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("GladeWraith1").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("GladeWraith2").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("GladeWraith3").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("GladeWraith4").Type);

				NPC.position.X = NPC.position.X + (NPC.width / 2);
				NPC.position.Y = NPC.position.Y + (NPC.height / 2);
				NPC.width = 30;
				NPC.height = 30;
				NPC.position.X = NPC.position.X - (NPC.width / 2);
				NPC.position.Y = NPC.position.Y - (NPC.height / 2);

				for (int num621 = 0; num621 < 20; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Plantera_Green, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
					}
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.Player;
			if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust)
				&& ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.SpawnTileY > Main.worldSurface || Main.dayTime)
				&& (!Main.eclipse || spawnInfo.SpawnTileY > Main.worldSurface || !Main.dayTime)
				&& (SpawnCondition.GoblinArmy.Chance == 0))
			{
				if (!NPC.AnyNPCs(ModContent.NPCType<ForestWraith>()))
					return spawnInfo.Player.GetSpiritPlayer().ZoneReach && NPC.downedBoss1 && !Main.dayTime ? .05f : 0f;
			}
			return 0f;
		}

		public override bool PreKill()
		{
			SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/DownedMiniboss"), NPC.Center);
			MyWorld.downedGladeWraith = true;
			return true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>("NPCs/Reach/ForestWraith_Glow").Value, screenPos);

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.OneFromOptions(1, ModContent.ItemType<OakHeart>(), ModContent.ItemType<HuskstalkStaff>()));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EnchantedLeaf>(), 1, 5, 7));
		}

		public void RegisterToChecklist(out BossChecklistDataHandler.EntryType entryType, out float progression,
			out string name, out Func<bool> downedCondition, ref BossChecklistDataHandler.BCIDData identificationData,
			ref string spawnInfo, ref string despawnMessage, ref string texture, ref string headTextureOverride,
			ref Func<bool> isAvailable)
		{
			entryType = BossChecklistDataHandler.EntryType.Miniboss;
			progression = 0.8f;
			name = "Glade Wraith";
			downedCondition = () => MyWorld.downedGladeWraith;
			identificationData = new BossChecklistDataHandler.BCIDData(
				new List<int> { ModContent.NPCType<ForestWraith>() },
				new List<int> {
					ModContent.ItemType<GladeWreath>()
				},
				null,
				new List<int> {
					ModContent.ItemType<HuskstalkStaff>(),
					ModContent.ItemType<AncientBark>()
				});
			spawnInfo =
				"Destroy a Bone Altar in the Underground Briar. The Glade Wraith also spawns naturally at nighttime after defeating the Eye of Cthulhu. Alternatively, find a Glade Wreath in Briar Chests and use it in the Briar at any time.";
			texture = "SpiritMod/Textures/BossChecklist/GladeWraithTexture";
			headTextureOverride = "SpiritMod/NPCs/Reach/ForestWraith_Head_Boss";
		}
	}
}