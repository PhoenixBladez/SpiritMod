using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies
{
	public class Pulsar : ModNPC, IStarjinxEnemy
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pulsar");
			Main.npcFrameCount[NPC.type] = 9;
		}

		public override bool IsLoadingEnabled(Mod mod) => false;

		public override void SetDefaults()
		{
			NPC.lifeMax = 500;
			NPC.defense = 25;
			NPC.value = 600f;
			NPC.alpha = 0;
			NPC.friendly = false;
			NPC.knockBackResist = 0f;
			NPC.HitSound = SoundID.DD2_CrystalCartImpact;
			NPC.DeathSound = SoundID.DD2_ExplosiveTrapExplode;
			NPC.width = 66;
			NPC.height = 88;
			NPC.damage = 45;
			NPC.lavaImmune = true;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
		}

		public override bool CheckDead() => false;

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			SoundEngine.PlaySound(SoundID.NPCHit4, NPC.Center);
			for (int k = 0; k < 10; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, .934f);
			}
			if (NPC.life <= 0)
			{
				SoundEngine.PlaySound(SoundID.Item14, NPC.Center);
				for (int i = 0; i < 3; ++i)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 99);
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SpiritMod/Gores/Pulsar/Pulsar" + (i + 1)).Type, 1f);
				}
			}
		}

		public override void AI()
		{
			NPC.velocity *= 0.99f;
			NPC.ai[0]++;
			NPC.ai[1]++;
			if (NPC.ai[0] >= 150 && !Filters.Scene["PulsarShockwave"].IsActive())
			{
				SoundEngine.PlaySound(SoundID.DD2_WyvernDiveDown, NPC.Center);
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center - new Vector2(NPC.rotation * 40, 0), Vector2.Zero, ModContent.ProjectileType<PulsarShockwave>(), 0, 0, NPC.target);
				NPC.ai[0] = 1;
				NPC.ai[2] = 0;
				NPC.netUpdate = true;
			}
			if (NPC.ai[0] % 5 == 0)
			{
				int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Flare);
				Main.dust[d].fadeIn += .5f;
			}
			if (NPC.ai[1] % 240 == 0)
			{
				SoundEngine.PlaySound(SoundID.DD2_WitherBeastAuraPulse, NPC.Center);
			}
			if (NPC.ai[0] >= 120 && NPC.ai[0] < 150)
			{
				NPC.ai[2] = 1;
			}
			NPC.rotation = (float)Math.Sin(NPC.ai[1] / 30f) / 2;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (NPC.ai[2] != 0)
			{
				SpriteEffects spriteEffects = SpriteEffects.None;
				if (NPC.spriteDirection == 1)
					spriteEffects = SpriteEffects.FlipHorizontally;
				Texture2D texture = TextureAssets.Npc[NPC.type].Value;
				float num99 = (float)(Math.Cos(Main.GlobalTimeWrappedHourly % 2.4f / 2.4f * MathHelper.TwoPi) / 4.0f + 0.5f);
				Color AfterimageColor = new Color(sbyte.MaxValue, sbyte.MaxValue, sbyte.MaxValue, 0).MultiplyRGBA(new Color(255, 111, 33, 150)) * 5f;
				Vector2 GlowPosition = new Vector2(NPC.Center.X - 24, NPC.Center.Y - 42) - Main.screenPosition - new Vector2((float)texture.Width / 3, (texture.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f + NPC.frame.Size() / 2 * NPC.scale + new Vector2(0.0f, NPC.gfxOffY);
				for (int index2 = 0; index2 < 4; ++index2)
				{
					Color GlowColor = NPC.GetAlpha(AfterimageColor) * (1f - num99);
					Vector2 GlowPosition2 = new Vector2(NPC.Center.X - 24, NPC.Center.Y - 42) + ((index2 / 4f * MathHelper.TwoPi) + NPC.rotation).ToRotationVector2() * (float)(8.0 * num99 + 2.0) - Main.screenPosition - new Vector2((float)texture.Width / 3, texture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f + NPC.frame.Size() / 2 * NPC.scale + new Vector2(0.0f, NPC.gfxOffY);
					Main.spriteBatch.Draw(texture, GlowPosition2, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), GlowColor, NPC.rotation, new Vector2(33, 0), NPC.scale, spriteEffects, 0.0f);
				}
				Main.spriteBatch.Draw(texture, GlowPosition, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), AfterimageColor, NPC.rotation, new Vector2(33, 0), NPC.scale, spriteEffects, 0.0f);
			}
			Main.spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.position + new Vector2(33, 0) - Main.screenPosition, NPC.frame, drawColor, NPC.rotation, new Vector2(33, 0), NPC.scale, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/StarjinxEvent/Enemies/Pulsar_Glow").Value, NPC.position + new Vector2(33, 0) - Main.screenPosition, NPC.frame, Color.White, NPC.rotation, new Vector2(33, 0), NPC.scale, SpriteEffects.None, 0f);

			return false;

		}
		public void DrawPathfinderOutline(SpriteBatch spriteBatch) => PathfinderOutlineDraw.DrawAfterImage(spriteBatch, NPC, NPC.frame, new Vector2(0, -44), new Vector2(33, 0));
	}
}