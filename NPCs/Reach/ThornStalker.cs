using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Hostile;
using SpiritMod.Items.Sets.BriarDrops;
using SpiritMod.Items.Armor.Masks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;
using SpiritMod.Biomes;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.Reach
{
	public class ThornStalker : ModNPC
	{
		bool attack = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thorn Stalker");
			Main.npcFrameCount[NPC.type] = 13;
			NPCID.Sets.TrailCacheLength[NPC.type] = 3;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}

		public override void SetDefaults()
		{
			NPC.width = 48;
			NPC.height = 58;
			NPC.damage = 15;
			NPC.defense = 8;
			NPC.lifeMax = 70;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 90f;
			NPC.knockBackResist = .35f;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Banners.ThornStalkerBanner>();
			SpawnModBiomes = new int[1] { ModContent.GetInstance<BriarSurfaceBiome>().Type };
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				new FlavorTextBestiaryInfoElement("Don't be fooled by its humanoid form, this creature is anything but. This beast will stalk its prey to the furthest reaches of the briar for the perfect chance to strike."),
			});
		}

		int frame = 0;
		int timer = 0;
		int shootTimer = 0;
		public override void HitEffect(int hitDirection, double damage)
		{
			SoundEngine.PlaySound(SoundID.NPCHit7, NPC.Center);
			for (int k = 0; k < 11; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Plantera_Green, hitDirection, -1f, 0, Color.Green, .61f);
			}
            if (NPC.life <= 0) {
                SoundEngine.PlaySound(SoundID.Zombie7, NPC.Center);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ThornStalker1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ThornStalker2").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ThornStalker3").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ThornStalker4").Type, 1f);
			}
		}

		public override void AI()
		{
			Lighting.AddLight((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f), 0.024f, 0.088f, 0.026f);
			NPC.spriteDirection = NPC.direction;
			Player target = Main.player[NPC.target];
			shootTimer++;
			if (shootTimer % 200 == 150) {
				attack = true;
			}
			if (attack) {
				NPC.velocity.Y = 6;
				NPC.velocity.X = .008f * NPC.direction;
				//shootTimer++;
				if (frame == 11 && timer == 0) {
					SoundEngine.PlaySound(SoundID.Item64, NPC.Center);
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						for (int i = 0; i < 2; i++) {
							Vector2 knifePos = new Vector2(NPC.Center.X + Main.rand.Next(-50, 50), NPC.Center.Y - Main.rand.Next(60));
							Vector2 direction = Main.player[NPC.target].Center - knifePos;
							direction.Normalize();
							direction *= Main.rand.NextFloat(7, 10);
							bool expertMode = Main.expertMode;
							int damage = expertMode ? 9 : 13;
							Projectile.NewProjectile(NPC.GetSource_FromAI(), knifePos, direction, ModContent.ProjectileType<ThornKnife>(), damage, 0);
						}
					}
					timer++;
				}

				if (target.position.X > NPC.position.X) {
					NPC.direction = 1;
				}
				else {
					NPC.direction = -1;
				}
			}
			else {
				//shootTimer = 0;
				NPC.aiStyle = 3;
				AIType = NPCID.WalkingAntlion;
			}
		}

		public override void FindFrame(int frameHeight)
		{
			if (attack)
			{
				timer++;
				if (timer >= 11)
				{
					frame++;
					timer = 0;
				}
				if (frame > 12)
				{
					attack = false;
					frame = 12;
				}
				if (frame < 7)
					frame = 7;
			}
			else
			{
				timer++;
				if (timer >= 6)
				{
					frame++;
					timer = 0;
				}

				if (frame > 6)
					frame = 1;
			}

			if (!attack && !NPC.collideY && NPC.velocity.Y > 0)
				frame = 0;

			NPC.frame.Y = frameHeight * frame;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, (NPC.height * 0.5f));
			for (int k = 0; k < NPC.oldPos.Length; k++) {
				var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY);
				Color color = NPC.GetAlpha(drawColor) * (float)(((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) / 2);
				spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
			}
			return true;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>("NPCs/Reach/ThornStalker_Glow").Value, screenPos);
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			var night = npcLoot.NightCondition();
			night.OnSuccess(ItemDropRule.Common(ModContent.ItemType<EnchantedLeaf>()));

			npcLoot.AddCommon<VineChain>(33);
			npcLoot.AddCommon<LeafPaddyHat>(20);
			npcLoot.Add(night);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.Player;
			if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon)) && (!Main.eclipse) && (SpawnCondition.GoblinArmy.Chance == 0)) {
				return spawnInfo.Player.GetSpiritPlayer().ZoneReach ? 0.36f : 0f;
			}
			return 0f;
		}
	}
}
