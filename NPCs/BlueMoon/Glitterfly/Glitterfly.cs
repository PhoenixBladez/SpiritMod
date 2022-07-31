using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using SpiritMod.Items.Sets.SeraphSet;
using SpiritMod.Items.Sets.MagicMisc.AstralClock;
using SpiritMod.Projectiles.Hostile;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.BlueMoon.Glitterfly
{
	public class Glitterfly : ModNPC
	{
		//int timer = 0;
		int moveSpeed = 0;
		int moveSpeedY = 0;
		float HomeY = 150f;
		//bool hat = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glitterfly");
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Pixie];
		}

		public override void SetDefaults()
		{
			NPC.width = 50;
			NPC.height = 40;
			NPC.damage = 45;
			NPC.lifeMax = 460;
			NPC.defense = 16;
			NPC.knockBackResist = 0.1f;

			NPC.noGravity = true;
			NPC.buffImmune[ModContent.BuffType<StarFlame>()] = true;
			NPC.buffImmune[BuffID.Confused] = true;

			AnimationType = NPCID.Pixie;
			NPC.HitSound = SoundID.NPCHit44;
			NPC.DeathSound = SoundID.NPCDeath46;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Banners.GlitterflyBanner>();
        }

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("Glitterflies do not feed often, as their palette consists of a rare fungus. In addition, they possess a venomous sting that causes intense disorientation and vertigo."),
			});
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 20; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.VilePowder, 1.5f * hitDirection, -1.5f, 0, default, 0.57f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Teleporter, 1.5f * hitDirection, -1.5f, 0, default, 0.52f);
			}
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Glitterfly1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Glitterfly2").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Glitterfly3").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Glitterfly4").Type, 1f);
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return MyWorld.BlueMoon && NPC.CountNPCS(ModContent.NPCType<Glitterfly>()) < 3 && spawnInfo.Player.ZoneOverworldHeight ? 1f : 0f;
		}

		public override bool PreAI()
		{
			bool expertMode = Main.expertMode;
			Lighting.AddLight((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f), 0.0f, 0.04f, 0.8f);

			Player player = Main.player[NPC.target];

            if (NPC.Center.X >= player.Center.X && moveSpeed >= Main.rand.Next(-60, -40)) // flies to players x position
            {
                NPC.netUpdate = true;
                moveSpeed--;
            }
            if (NPC.Center.X <= player.Center.X && moveSpeed <= Main.rand.Next(40, 60))
            {
                NPC.netUpdate = true;
                moveSpeed++;
            }
			NPC.velocity.X = moveSpeed * 0.13f;

			if (NPC.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= Main.rand.Next(-40, -30)) //Flies to players Y position
			{
				moveSpeedY--;
				HomeY = Main.rand.NextFloat(160f, 180f);
                NPC.netUpdate = true;
            }

            if (NPC.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= Main.rand.Next(30, 40))
            {
                NPC.netUpdate = true;
                moveSpeedY++;
            }
			NPC.velocity.Y = moveSpeedY * 0.1f;
			++NPC.ai[1];
			if (NPC.ai[1] >= 10 && Main.netMode != NetmodeID.MultiplayerClient) {
				int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 0f, 0f, ModContent.ProjectileType<GlitterDust>(), 0, 0, Main.myPlayer, 0, 0);
                NPC.ai[1] = 0;
			}
			Vector2 center = NPC.Center;
			float num8 = (float)player.miscCounter / 40f;
			float num7 = 1.0471975512f * 2;
			for (int k = 0; k < 3; k++) {
				{
					int num6 = Dust.NewDust(center, 0, 0, DustID.GoldCoin, 0f, 0f, 100, default, 1.3f);
					Main.dust[num6].noGravity = true;
					Main.dust[num6].velocity = Vector2.Zero;
					Main.dust[num6].noLight = true;
					Main.dust[num6].position = center + (num8 * 6.28318548f + num7 * (float)k).ToRotationVector2() * 12f;
				}
			}
			int distance = (int)Math.Sqrt((NPC.Center.X - player.Center.X) * (NPC.Center.X - player.Center.X) + (NPC.Center.Y - player.Center.Y) * (NPC.Center.Y - player.Center.Y));
			if (distance < 540) {
				++NPC.ai[0];
				if (NPC.ai[0] == 140 || NPC.ai[0] == 280 || NPC.ai[0] == 320) {
                    SoundEngine.PlaySound(SoundID.Item43, NPC.Center);
                    if (Main.netMode != NetmodeID.MultiplayerClient) {
						Vector2 dir = Main.player[NPC.target].Center - NPC.Center;
						dir.Normalize();
						dir.X *= 9f;
						dir.Y *= 9f;
						float A = (float)Main.rand.Next(-200, 200) * 0.01f;
						float B = (float)Main.rand.Next(-200, 200) * 0.01f;
						int damage = expertMode ? 19 : 27;
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, dir.X + A, dir.Y + B, ModContent.ProjectileType<StarSting>(), damage, 1, Main.myPlayer, 0, 0);
					}
				}
				if (NPC.ai[0] >= 450) {
                    NPC.netUpdate = true;
                    HomeY -= 100f;
				}
				if (NPC.ai[0] >= 456) {
                    NPC.netUpdate = true;
                    HomeY = 160f;
					NPC.ai[0] = 0;
				}
			}
			else {
                NPC.netUpdate = true;
                NPC.ai[0] = 0;
			}
			return true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY), NPC.frame,
							 drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>("NPCs/BlueMoon/Glitterfly/Glitterfly_Glow").Value, screenPos);
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(5) == 0)
				target.AddBuff(ModContent.BuffType<StarFlame>(), 200);
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MoonStone>(), 5));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StopWatch>(), 100));
		}
	}
}