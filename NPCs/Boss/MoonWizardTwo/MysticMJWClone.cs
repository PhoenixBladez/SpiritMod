using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using SpiritMod.Buffs;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs.Boss.MoonWizardTwo.Projectiles;
using SpiritMod.Prim;
using SpiritMod.Utilities;
using SpiritMod.Buffs.DoT;

namespace SpiritMod.NPCs.Boss.MoonWizardTwo
{
	public class MysticMJWClone : ModNPC
	{
		private float trueFrame = 0;

		const int DASHAMOUNT = 20;

		private Vector2 dashDirection = Vector2.Zero;
		private float dashDistance = 0f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mystic Clone");
			Main.npcFrameCount[NPC.type] = 21;
			NPCID.Sets.TrailCacheLength[NPC.type] = 10;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}

		public override void SetDefaults()
		{
			NPC.friendly = false;
			NPC.lifeMax = 500;
			NPC.defense = 20;
			NPC.value = 0;
			NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
			NPC.width = 17;
			NPC.height = 35;
			NPC.damage = 110;
            NPC.scale = 2f;
			NPC.lavaImmune = true;
            NPC.buffImmune[BuffID.Poisoned] = true;
            NPC.buffImmune[ModContent.BuffType<FesteringWounds>()] = true;
            NPC.buffImmune[BuffID.Venom] = true;
            NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.HitSound = SoundID.NPCHit13;
			NPC.DeathSound = SoundID.NPCDeath2;
			NPC.ai[1] = 3;
		}

		public void DrawAfterImage(SpriteBatch spriteBatch, Vector2 offset, float trailLengthModifier, Color color, float opacity, float startScale, float endScale) => DrawAfterImage(spriteBatch, offset, trailLengthModifier, color, color, opacity, startScale, endScale);
		public void DrawAfterImage(SpriteBatch spriteBatch, Vector2 offset, float trailLengthModifier, Color startColor, Color endColor, float opacity, float startScale, float endScale)
		{
			SpriteEffects spriteEffects = (NPC.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			for (int i = 1; i < 10; i++)
			{
				Color color = Color.Lerp(startColor, endColor, i / 10f) * opacity;
				spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Boss/MoonWizardTwo/MoonWizardTwo_Afterimage").Value, new Vector2(NPC.Center.X, NPC.Center.Y) + offset - Main.screenPosition + new Vector2(0, NPC.gfxOffY) - NPC.velocity * (float)i * trailLengthModifier, NPC.frame, color, NPC.rotation, NPC.frame.Size() * 0.5f, MathHelper.Lerp(startScale, endScale, i / 10f), spriteEffects, 0f);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) { return false; }
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) { drawSpecialGlow(spriteBatch, drawColor); }
		public void drawSpecialGlow(SpriteBatch spriteBatch, Color drawColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (NPC.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;
			Texture2D texture = TextureAssets.Npc[NPC.type].Value;
			float num99 = (float)(Math.Cos((double)Main.GlobalTimeWrappedHourly % 2.40000009536743 / 2.40000009536743 * 6.28318548202515) / 4.0 + 0.5);
			Microsoft.Xna.Framework.Color AfterimageColor = new Microsoft.Xna.Framework.Color((int)sbyte.MaxValue, (int)sbyte.MaxValue, (int)sbyte.MaxValue, 0).MultiplyRGBA(new Color(255, 0, 236, 150)) * 5f;
			Vector2 GlowPosition = new Vector2(NPC.Center.X, NPC.Center.Y - 18) - Main.screenPosition - new Vector2((float)texture.Width / 3, (float)(texture.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f + NPC.frame.Size() / 2 * NPC.scale + new Vector2(0.0f, NPC.gfxOffY);
			for (int index2 = 0; index2 < 4; ++index2)
			{
				Microsoft.Xna.Framework.Color GlowColor = NPC.GetAlpha(AfterimageColor) * (1f - num99);
				Vector2 GlowPosition2 = new Vector2(NPC.Center.X, NPC.Center.Y - 18) + ((float)((double)index2 / (double)4 * 6.28318548202515) + NPC.rotation).ToRotationVector2() * (float)(8.0 * (double)num99 + 2.0) - Main.screenPosition - new Vector2((float)texture.Width / 3, (float)(texture.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f + NPC.frame.Size() / 2 * NPC.scale + new Vector2(0.0f, NPC.gfxOffY);
				Main.spriteBatch.Draw(texture, GlowPosition2, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), GlowColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, spriteEffects, 0.0f);
			}
			Main.spriteBatch.Draw(texture, GlowPosition, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), AfterimageColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, spriteEffects, 0.0f);
		}
		public void DrawSpecialGlow(SpriteBatch spriteBatch, Color drawColor)
		{
			float num108 = 4;
			float num107 = (float)Math.Cos((double)(Main.GlobalTimeWrappedHourly % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;
			float num106 = 0f;

			SpriteEffects spriteEffects3 = (NPC.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Vector2 vector33 = new Vector2(NPC.Center.X, NPC.Center.Y - 18) - Main.screenPosition + new Vector2(0, NPC.gfxOffY) - NPC.velocity;
			Color color29 = new Color(127 - NPC.alpha, 127 - NPC.alpha, 127 - NPC.alpha, 0).MultiplyRGBA(Color.Pink);
			for (int num103 = 0; num103 < 4; num103++)
			{
				Color color28 = color29;
				color28 = NPC.GetAlpha(color28);
				color28 *= 1f - num107;
				Vector2 vector29 = new Vector2(NPC.Center.X, NPC.Center.Y - 18) + ((float)num103 / (float)num108 * 6.28318548f + NPC.rotation + num106).ToRotationVector2() * (4f * num107 + 2f) - Main.screenPosition + new Vector2(0, NPC.gfxOffY) - NPC.velocity * (float)num103;
				Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Boss/MoonWizardTwo/MoonWizardTwo_Glow").Value, vector29, NPC.frame, color28, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, spriteEffects3, 0f);
			}
			Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Boss/MoonWizardTwo/MoonWizardTwo_Glow").Value, vector33, NPC.frame, color29, NPC.rotation, NPC.frame.Size() / 2f, NPC.scale, spriteEffects3, 0f);

		}
		//0-3: idle
		//4-9 propelling
		//10-13 skirt up
		//14-21: turning
		//22-28: kick
		//29-38: Teleport
		//39-51:Teleport part 2
		//54-61: Weird float

		public override void AI()
		{
			float speed = 30;
			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];
			UpdateFrame(0.15f, 4, 9);
			NPC.ai[0]++;
			if (NPC.ai[0] == DASHAMOUNT)
			{
				NPC.damage = 70;
				dashDirection = (player.Center + (player.velocity * 20)) - NPC.Center;
				dashDistance = dashDirection.Length();
				dashDirection.Normalize();
				dashDirection *= speed;
				NPC.velocity = dashDirection;

				NPC.rotation = NPC.velocity.ToRotation() + 1.57f;
				SoundEngine.PlaySound(SoundID.Item81, NPC.Center);
			}
			if (NPC.ai[0] < DASHAMOUNT)
			{
				dashDirection = (player.Center + (player.velocity * 20)) - NPC.Center;
				NPC.rotation = dashDirection.ToRotation() + 1.57f;
				NPC.velocity = Vector2.Zero;
			}
			if (NPC.ai[0] > Math.Abs(dashDistance / speed) + DASHAMOUNT + 40)
			{
				NPC.active = false;
			}
		}
		public override void FindFrame(int frameHeight)
		{
			NPC.frame.Width = 60;
			NPC.frame.X = ((int)trueFrame % 3) * NPC.frame.Width;
			NPC.frame.Y = (((int)trueFrame - ((int)trueFrame % 3)) / 3) * NPC.frame.Height;
		}

		public void UpdateFrame(float speed, int minFrame, int maxFrame)
		{
			trueFrame += speed;
			if (trueFrame < minFrame) 
			{
				trueFrame = minFrame;
			}
			if (trueFrame > maxFrame) 
			{
				trueFrame = minFrame;
			}
		}
	}
}
