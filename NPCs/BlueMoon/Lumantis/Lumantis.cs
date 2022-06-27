using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable.Potion;
using SpiritMod.Items.Sets.SeraphSet;
using SpiritMod.Items.Sets.MagicMisc.AstralClock;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Buffs;
using Terraria.GameContent.ItemDropRules;

namespace SpiritMod.NPCs.BlueMoon.Lumantis
{
	public class Lumantis : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lumantis");
			Main.npcFrameCount[NPC.type] = 5;
		}

		public override void SetDefaults()
		{
			NPC.width = 40;
			NPC.height = 40;
			NPC.damage = 62;
			NPC.defense = 20;
			NPC.lifeMax = 560;
			NPC.buffImmune[ModContent.BuffType<StarFlame>()] = true;
			NPC.HitSound = SoundID.DD2_LightningBugHurt;
			NPC.DeathSound = SoundID.NPCDeath34;
			NPC.value = 760f;
			NPC.knockBackResist = .2f;
			NPC.aiStyle = 3;
			AIType = NPCID.WalkingAntlion;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Banners.LumantisBanner>();
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return MyWorld.BlueMoon && NPC.CountNPCS(ModContent.NPCType<Lumantis>()) < 4 && spawnInfo.Player.ZoneOverworldHeight ? .6f : 0f;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 11; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Flare_Blue, hitDirection, -1f, 1, default, .81f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.VenomStaff, hitDirection, -1f, 1, default, .51f);
			}
			if (NPC.life <= 0) {
				for (int k = 0; k < 11; k++) {
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Flare_Blue, hitDirection, -1f, 1, default, .81f);
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.VenomStaff, hitDirection, -1f, 1, default, .71f);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Lumantis/Lumantis1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Lumantis/Lumantis2").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Lumantis/Lumantis3").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Lumantis/Lumantis4").Type, 1f);
			}
		}
		int timer;
		int frame;
		public override void AI()
		{
			Player player = Main.player[NPC.target];
			timer++;
			Lighting.AddLight((int)((NPC.position.X + (float)(NPC.width / 2)) / 16f), (int)((NPC.position.Y + (float)(NPC.height / 2)) / 16f), .196f * 3, .092f * 3, 0.214f * 3);
			++NPC.ai[1];
			if (NPC.ai[1] >= 600) {
				reflectPhase = true;
				NPC.aiStyle = 0;
				if (player.position.X > NPC.position.X) {
					NPC.spriteDirection = 1;
				}
				else {
					NPC.spriteDirection = -1;
				}
			}
			else {
				NPC.aiStyle = 3;
				AIType = NPCID.WalkingAntlion;
				NPC.spriteDirection = NPC.direction;
				reflectPhase = false;
				NPC.defense = 20;
				if (timer >= 4) {
					frame++;
					timer = 0;
				}
				if (frame >= 3) {
					frame = 0;
				}
			}
			if (NPC.ai[1] >= 840) {
				NPC.ai[1] = 0;
			}
		}
		public override void FindFrame(int frameHeight)
		{
			if (!reflectPhase) {
				NPC.frame.Y = frameHeight * frame;
			}
			else {
				NPC.frame.Y = frameHeight * 4;
			}
		}

		bool reflectPhase;
		public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
		{

			if (reflectPhase) {
				player.Hurt(PlayerDeathReason.LegacyEmpty(), item.damage, 0, true, false, false, -1);
				SoundEngine.PlaySound(SoundID.DD2_LightningBugZap, NPC.position);
			}
		}

		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			if (reflectPhase && !projectile.minion && !Main.player[projectile.owner].channel) {
				projectile.hostile = true;
				projectile.friendly = false;
				SoundEngine.PlaySound(SoundID.DD2_LightningBugZap, NPC.position);
				projectile.penetrate = 2;
				projectile.velocity.X = projectile.velocity.X * -1f;
			}
		}

		private void DoDustEffect(Vector2 position, float distance, float minSpeed = 2f, float maxSpeed = 3f, object follow = null)
		{
			float angle = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi);
			Vector2 vec = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
			Vector2 vel = vec * Main.rand.NextFloat(minSpeed, maxSpeed);

			int dust = Dust.NewDust(position - vec * distance, 0, 0, DustID.VenomStaff);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].scale *= .6f;
			Main.dust[dust].velocity = vel;
			Main.dust[dust].customData = follow;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>("NPCs/BlueMoon/Lumantis/Lumantis_Glow").Value);
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MoonStone>(), 5));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StopWatch>(), 100));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MoonJelly>(), 10));
		}
	}
}
