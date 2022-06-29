using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ID;
using SpiritMod.Buffs;
using SpiritMod.Buffs.DoT;

namespace SpiritMod.NPCs.Boss.Atlas
{
	public class CobbledEye3 : ModNPC
	{
		int timer = 0;
		bool start = true;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobbled Eye");
		}

		public override void SetDefaults()
		{
			NPC.width = 42;
			NPC.height = 42;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.damage = 80;
			NPC.lifeMax = 1000;

			NPC.buffImmune[BuffID.Confused] = true;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Venom] = true;
			NPC.buffImmune[ModContent.BuffType<FesteringWounds>()] = true;
			NPC.buffImmune[ModContent.BuffType<BloodCorrupt>()] = true;
			NPC.buffImmune[ModContent.BuffType<BloodInfusion>()] = true;
		}

		public override bool PreAI()
		{
			bool expertMode = Main.expertMode;
			if (start) {
				for (int num621 = 0; num621 < 15; num621++)
					Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Stone, 0f, 0f, 100, default, 2f);
				start = false;
			}

			NPC.TargetClosest(true);
			Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
			direction.Normalize();
			direction *= 9f;
			NPC.rotation = direction.ToRotation();
			timer++;
			if (timer > 60) {
				if (Main.rand.Next(8) == 0) {
					for (int num621 = 0; num621 < 5; num621++) {
						int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Electric, 0f, 0f, 100, default, 2f);
					}
					int damage = expertMode ? 24 : 35;
					int proj2 = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X, direction.Y, ModContent.ProjectileType<MiracleBeam>(), damage, 1f, NPC.target);
				}
				timer = 0;
			}

			int parent = (int)NPC.ai[0];
			if (parent < 0 || parent >= Main.maxNPCs || !Main.npc[parent].active || Main.npc[parent].type != ModContent.NPCType<Atlas>()) {
				NPC.active = false;
				return false;
			}

			NPC.Center = Main.npc[parent].Center + NPC.ai[2] * NPC.ai[1].ToRotationVector2();

			NPC.ai[1] += .03f;
			return false;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Stone, hitDirection, -1f, 0, default, 1f);
			}
			if (NPC.life <= 0) {
				NPC.position.X = NPC.position.X + (NPC.width / 2);
				NPC.position.Y = NPC.position.Y + (NPC.height / 2);
				NPC.width = 50;
				NPC.height = 50;
				NPC.position.X = NPC.position.X - (NPC.width / 2);
				NPC.position.Y = NPC.position.Y - (NPC.height / 2);
				for (int num621 = 0; num621 < 20; num621++) {
					int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Stone, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0) {
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
					}
				}
				for (int num623 = 0; num623 < 40; num623++) {
					int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Stone, 0f, 0f, 100, default, 3f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 5f;
					num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Stone, 0f, 0f, 100, default, 2f);
					Main.dust[num624].velocity *= 2f;
				}
			}
		}

		public override bool CheckActive() => false;
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>("NPCs/Boss/Atlas/CobbledEye_Glow").Value);

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.55f * bossLifeScale);
			NPC.damage = (int)(NPC.damage * 0.75f);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (NPC.velocity != Vector2.Zero) {
				Texture2D texture = TextureAssets.Npc[NPC.type].Value;
				Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
				for (int i = 1; i < NPC.oldPos.Length; ++i) {
					Vector2 vector2_2 = NPC.oldPos[i];
					Microsoft.Xna.Framework.Color color2 = Color.White * NPC.Opacity;
					color2.R = (byte)(0.5 * color2.R * (10 - i) / 20.0);
					color2.G = (byte)(0.5 * color2.G * (10 - i) / 20.0);
					color2.B = (byte)(0.5 * color2.B * (10 - i) / 20.0);
					color2.A = (byte)(0.5 * color2.A * (10 - i) / 20.0);
					Main.spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, new Vector2(NPC.oldPos[i].X - Main.screenPosition.X + (NPC.width / 2),
						NPC.oldPos[i].Y - Main.screenPosition.Y + NPC.height / 2), new Rectangle?(NPC.frame), color2, NPC.oldRot[i], origin, NPC.scale, SpriteEffects.None, 0.0f);
				}

			}
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}
	}
}