using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable.Quest;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Thrown;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Projectiles;
using SpiritMod.Projectiles.Hostile;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SpiritMod.NPCs.Reach
{
	[AutoloadBossHead]
	public class ForestWraith : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glade Wraith");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 44;
			npc.height = 60;
			npc.damage = 28;
			npc.defense = 10;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.lifeMax = 400;
			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 541f;
			npc.knockBackResist = 0.05f;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.aiStyle = 44;
			aiType = NPCID.FlyingAntlion;
            banner = npc.type;
            bannerItem = ModContent.ItemType<Items.Banners.GladeWraithBanner>();
        }

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		//bool rotationspawns1 = false;
		int timer = 0;
		public override bool PreAI()
		{
			Lighting.AddLight((int)((npc.position.X + (npc.width / 2)) / 16f), (int)((npc.position.Y + (npc.height / 2)) / 16f), 0.46f, 0.32f, .1f);
			//bool expertMode = Main.expertMode;
			timer++;
			if(timer == 240 || timer == 280 || timer == 320) {
				Main.PlaySound(SoundID.Grass, (int)npc.position.X, (int)npc.position.Y, 0);
				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				direction.Normalize();
				direction.X *= 10f;
				direction.Y *= 10f;

				int amountOfProjectiles = Main.rand.Next(1, 1);
				for(int i = 0; i < amountOfProjectiles; ++i) {
					float A = Main.rand.Next(-120, 120) * 0.01f;
					float B = Main.rand.Next(-120, 120) * 0.01f;
					int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<OvergrowthLeaf>(), 6, 1, Main.myPlayer, 0, 0);
					Main.projectile[p].hostile = true;
					Main.projectile[p].friendly = false;
				}
				npc.netUpdate = true;
			}
			if(timer >= 420 && timer <= 720) {
				Dust.NewDust(npc.position, npc.width, npc.height, 6, 0f, -2.5f, 0, default, 0.6f);
				npc.defense = 0;
				npc.velocity = Vector2.Zero;
				if(Main.rand.Next(52) == 0) {
					Vector2 direction = Main.player[npc.target].Center - npc.Center;
					direction.Normalize();
					direction.X *= 3f;
					direction.Y *= 11f;
					Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 8);
					int amountOfProjectiles = Main.rand.Next(1, 1);
					for(int i = 0; i < amountOfProjectiles; ++i) {
						float A = Main.rand.Next(-120, 120) * 0.05f;
						float B = Main.rand.Next(-120, -10) * 0.05f;
						int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, -direction.Y + B, ModContent.ProjectileType<LittleBouncingSpore>(), 8, 1, Main.myPlayer, 0, 0);
						Main.projectile[p].hostile = true;
						Main.projectile[p].friendly = false;
					}
				}
				npc.netUpdate = true;
			}
			if(timer >= 730) {
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
			if(timer >= 750) {
				timer = 0;
				npc.netUpdate = true;
			}
			npc.spriteDirection = npc.direction;
			return true;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
			=> npc.lifeMax = (int)(npc.lifeMax * 0.55f * bossLifeScale);

		public override void HitEffect(int hitDirection, double damage)
		{
			int d = 3;
			int d1 = 7;
			for(int k = 0; k < 30; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, default, 0.3f);
				Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}
			if(npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GladeWraith/GladeWraith1"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GladeWraith/GladeWraith2"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GladeWraith/GladeWraith3"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GladeWraith/GladeWraith4"));
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 30;
				npc.height = 30;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for(int num621 = 0; num621 < 20; num621++) {
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 167, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 3f;
					if(Main.rand.Next(2) == 0) {
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
			if(!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) 
				&& ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) 
				&& (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime) 
				&& (SpawnCondition.GoblinArmy.Chance == 0)) {
				if(!NPC.AnyNPCs(ModContent.NPCType<ForestWraith>()))
					return spawnInfo.player.GetSpiritPlayer().ZoneReach && NPC.downedBoss1 && !Main.dayTime ? .05f : 0f;
			}
			return 0f;
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

			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<AncientBark>(), 3);

			if(!Main.LocalPlayer.HasItem(ModContent.ItemType<SacredVine>())) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<SacredVine>());
			}
		}

	}
}
