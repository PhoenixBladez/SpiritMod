using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Consumable.Quest;
using SpiritMod.Items.Sets.BriarDrops;
using SpiritMod.Items.Sets.HuskstalkSet;
using SpiritMod.Items.Sets.GladeWraithDrops;
using SpiritMod.Projectiles;
using SpiritMod.Projectiles.Hostile;
using SpiritMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SpiritMod.NPCs.Reach
{
	[AutoloadBossHead]
	public class ForestWraith : ModNPC, IBCRegistrable
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glade Wraith");
			Main.npcFrameCount[npc.type] = 12;
		}

		public override void SetDefaults()
		{
			npc.width = 44;
			npc.height = 82;
			npc.damage = 28;
			npc.defense = 10;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.lifeMax = 300;
			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 541f;
			npc.knockBackResist = 0.05f;
			npc.noGravity = true;
            npc.chaseable = true;
            npc.noTileCollide = true;
			npc.aiStyle = 44;
            aiType = NPCID.FlyingFish;
            banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.GladeWraithBanner>();
		}
		bool throwing = false;
		public override void FindFrame(int frameHeight)
		{
			if (!throwing)
			{
				npc.frameCounter += 0.15f;
				npc.frameCounter %= 4;
				int frame = (int)npc.frameCounter;
				npc.frame.Y = frame * frameHeight;
			}
			else
			{
				npc.frameCounter += 0.15f;
				npc.frameCounter %= 8;
				int frame = (int)npc.frameCounter;
				npc.frame.Y = (frame + 4) * frameHeight;
			}
		}

		//bool rotationspawns1 = false;
		int timer = 0;
		bool thrown = false;
		public override bool PreAI()
		{
            npc.collideX = false;
            npc.collideY = false;
			Lighting.AddLight((int)((npc.position.X + (npc.width / 2)) / 16f), (int)((npc.position.Y + (npc.height / 2)) / 16f), 0.46f, 0.32f, .1f);
			//bool expertMode = Main.expertMode;
			timer++;
			if ((timer == 240 || timer == 280 || timer == 320)) {
				Main.PlaySound(SoundID.Grass, (int)npc.position.X, (int)npc.position.Y, 0);
				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				direction.Normalize();
				direction.X *= 10f;
				direction.Y *= 10f;
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					int amountOfProjectiles = 1;
					for (int i = 0; i < amountOfProjectiles; ++i) {
						float A = Main.rand.Next(-120, 120) * 0.01f;
						float B = Main.rand.Next(-120, 120) * 0.01f;
						int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<OvergrowthLeaf>(), 6, 1, Main.myPlayer, 0, 0);
						Main.projectile[p].hostile = true;
						Main.projectile[p].friendly = false;
					}
				}
				npc.netUpdate = true;
			}
			if ((timer >= 500 && timer <= 720)) {
				throwing = true;
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Fire, 0f, -2.5f, 0, default, 0.6f);
				npc.defense = 0;
				npc.velocity = Vector2.Zero;
				if ((int)npc.frameCounter == 4 && !thrown)
				{
					thrown = true;
					Vector2 direction = npc.GetArcVel(Main.player[npc.target].Center, 0.4f, 100, 500, maxXvel : 14);
					Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 8);
					if (Main.netMode != NetmodeID.MultiplayerClient)
						Projectile.NewProjectile(npc.Center, direction, ModContent.ProjectileType<LittleBouncingSpore>(), 8, 1, Main.myPlayer, 0, 0);
				}
				if ((int)npc.frameCounter != 4)
				{
					thrown = false;
				}
				npc.netUpdate = true;
			}
			else
			{
				throwing = false;
			}
			if (timer >= 730) {
				npc.defense = 10;
				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				direction.Normalize();
				Main.PlaySound(SoundID.Zombie, npc.Center, 7);
				direction.X *= Main.rand.Next(6, 9);
				direction.Y *= Main.rand.Next(6, 9);
				npc.velocity.X = direction.X;
				npc.velocity.Y = direction.Y;
				npc.velocity *= 0.97f;
				timer = 0;
				npc.netUpdate = true;
			}
			if (timer >= 750) {
				timer = 0;
				npc.netUpdate = true;
			}
			npc.spriteDirection = npc.direction;
			return true;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			int d = 3;
			int d1 = 7;
			for (int k = 0; k < 30; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, default, 0.3f);
				Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GladeWraith/GladeWraith1"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GladeWraith/GladeWraith2"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GladeWraith/GladeWraith3"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GladeWraith/GladeWraith4"));
				npc.position.X = npc.position.X + (npc.width / 2);
				npc.position.Y = npc.position.Y + (npc.height / 2);
				npc.width = 30;
				npc.height = 30;
				npc.position.X = npc.position.X - (npc.width / 2);
				npc.position.Y = npc.position.Y - (npc.height / 2);
				for (int num621 = 0; num621 < 20; num621++) {
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Plantera_Green, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0) {
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
					}
				}
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
			if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust)
				&& ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime)
				&& (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime)
				&& (SpawnCondition.GoblinArmy.Chance == 0)) {
				if (!NPC.AnyNPCs(ModContent.NPCType<ForestWraith>()))
					return spawnInfo.player.GetSpiritPlayer().ZoneReach && NPC.downedBoss1 && !Main.dayTime ? .05f : 0f;
			}
			return 0f;
		}
        public override bool PreNPCLoot()
        {
            Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/DownedMiniboss"));
            MyWorld.downedGladeWraith = true;
            return true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
			=> GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Reach/ForestWraith_Glow"));
		public override void NPCLoot()
		{
			int[] lootTable = { ModContent.ItemType<OakHeart>(), ModContent.ItemType<HuskstalkStaff>() };
			int loot = Main.rand.Next(lootTable.Length);
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, lootTable[loot]);

			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<EnchantedLeaf>(), Main.rand.Next(5, 8));
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
