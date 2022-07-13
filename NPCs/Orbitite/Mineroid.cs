using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Armor.AstronautVanity;
using SpiritMod.Items.Weapon.Summon;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Orbitite
{
	public class Mineroid : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orbitite");
			Main.npcFrameCount[NPC.type] = 6;
		}

		public override void SetDefaults()
		{
			NPC.width = 40;
			NPC.height = 40;
			NPC.damage = 25;
			NPC.defense = 15;
			NPC.lifeMax = 70;
			NPC.HitSound = SoundID.NPCHit7;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 110f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.buffImmune[BuffID.OnFire] = true;
			NPC.buffImmune[BuffID.Confused] = true;
			NPC.knockBackResist = .45f;
			NPC.aiStyle = 44;
			AIType = NPCID.FlyingAntlion;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.OrbititeBanner>();
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Meteor,
				new FlavorTextBestiaryInfoElement("This insignificant asteroid orbits a larger body, but will one day leave the nest and drift into the vast universe."),
			});
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.Player.ZoneMeteor && spawnInfo.SpawnTileY < Main.rockLayer && NPC.downedBoss2 ? 0.15f : 0f;

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OrbiterStaff>(), 20));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GravityModulator>(), 400));
			npcLoot.Add(ItemDropRule.OneFromOptions(ModContent.ItemType<AstronautLegs>(), ModContent.ItemType<AstronautHelm>(), ModContent.ItemType<AstronautBody>()));
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Main.EntitySpriteDraw(TextureAssets.Npc[NPC.type].Value, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>("NPCs/Orbitite/Mineroid_Glow").Value, screenPos);

		public override void HitEffect(int hitDirection, double damage)
		{
			SoundEngine.PlaySound(SoundID.DD2_WitherBeastHurt, NPC.Center);
			for (int k = 0; k < 11; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.CorruptionThorns, hitDirection, -1f, 0, default, .61f);
			}
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Mineroid1").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Mineroid2").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Mineroid3").Type);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 61);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 62);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 63);
				NPC.position.X = NPC.position.X + (float)(NPC.width / 2);
				NPC.position.Y = NPC.position.Y + (float)(NPC.height / 2);
				NPC.width = 30;
				NPC.height = 30;
				NPC.position.X = NPC.position.X - (float)(NPC.width / 2);
				NPC.position.Y = NPC.position.Y - (float)(NPC.height / 2);
				for (int num621 = 0; num621 < 20; num621++) {
					int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.CorruptionThorns, 0f, 0f, 100, default, 1f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.NextBool(2)) {
						Main.dust[num622].scale = 0.5f;
					}
				}
			}
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		int timer = 0;

		public override void AI()
		{
			timer++;
			if (timer >= 90 && Main.netMode != NetmodeID.MultiplayerClient) {
				bool expertMode = Main.expertMode;
				int damage = expertMode ? 10 : 16;
				Vector2 vector2_2 = Vector2.UnitY.RotatedByRandom(1.57079637050629f) * new Vector2(5f, 3f);
				int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, vector2_2.X, vector2_2.Y, ModContent.ProjectileType<MeteorShardHostile1>(), damage, 0.0f, Main.myPlayer, 0.0f, (float)NPC.whoAmI);
				Main.projectile[p].hostile = true;
				timer = 0;
				NPC.netUpdate = true;
			}

			NPC.spriteDirection = NPC.direction;
		}
	}
}
