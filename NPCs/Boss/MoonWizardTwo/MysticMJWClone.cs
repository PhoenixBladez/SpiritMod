using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using SpiritMod.Buffs;
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
			Main.npcFrameCount[npc.type] = 21;
			NPCID.Sets.TrailCacheLength[npc.type] = 10;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.friendly = false;
			npc.lifeMax = 500;
			npc.defense = 20;
			npc.value = 0;
			npc.aiStyle = -1;
            npc.knockBackResist = 0f;
			npc.width = 17;
			npc.height = 35;
			npc.damage = 110;
            npc.scale = 2f;
			npc.lavaImmune = true;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[ModContent.BuffType<FesteringWounds>()] = true;
            npc.buffImmune[BuffID.Venom] = true;
            npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit13;
			npc.DeathSound = SoundID.NPCDeath2;
			npc.ai[1] = 3;
		}

		public void DrawAfterImage(SpriteBatch spriteBatch, Vector2 offset, float trailLengthModifier, Color color, float opacity, float startScale, float endScale) => DrawAfterImage(spriteBatch, offset, trailLengthModifier, color, color, opacity, startScale, endScale);
		public void DrawAfterImage(SpriteBatch spriteBatch, Vector2 offset, float trailLengthModifier, Color startColor, Color endColor, float opacity, float startScale, float endScale)
		{
			SpriteEffects spriteEffects = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			for (int i = 1; i < 10; i++)
			{
				Color color = Color.Lerp(startColor, endColor, i / 10f) * opacity;
				spriteBatch.Draw(mod.GetTexture("NPCs/Boss/MoonWizardTwo/MoonWizardTwo_Afterimage"), new Vector2(npc.Center.X, npc.Center.Y) + offset - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * (float)i * trailLengthModifier, npc.frame, color, npc.rotation, npc.frame.Size() * 0.5f, MathHelper.Lerp(startScale, endScale, i / 10f), spriteEffects, 0f);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) { return false; }
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) { drawSpecialGlow(spriteBatch, drawColor); }
		public void drawSpecialGlow(SpriteBatch spriteBatch, Color drawColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (npc.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;
			Texture2D texture = Main.npcTexture[npc.type];
			float num99 = (float)(Math.Cos((double)Main.GlobalTime % 2.40000009536743 / 2.40000009536743 * 6.28318548202515) / 4.0 + 0.5);
			Microsoft.Xna.Framework.Color AfterimageColor = new Microsoft.Xna.Framework.Color((int)sbyte.MaxValue, (int)sbyte.MaxValue, (int)sbyte.MaxValue, 0).MultiplyRGBA(new Color(255, 0, 236, 150)) * 5f;
			Vector2 GlowPosition = new Vector2(npc.Center.X, npc.Center.Y - 18) - Main.screenPosition - new Vector2((float)texture.Width / 3, (float)(texture.Height / Main.npcFrameCount[npc.type])) * npc.scale / 2f + npc.frame.Size() / 2 * npc.scale + new Vector2(0.0f, npc.gfxOffY);
			for (int index2 = 0; index2 < 4; ++index2)
			{
				Microsoft.Xna.Framework.Color GlowColor = npc.GetAlpha(AfterimageColor) * (1f - num99);
				Vector2 GlowPosition2 = new Vector2(npc.Center.X, npc.Center.Y - 18) + ((float)((double)index2 / (double)4 * 6.28318548202515) + npc.rotation).ToRotationVector2() * (float)(8.0 * (double)num99 + 2.0) - Main.screenPosition - new Vector2((float)texture.Width / 3, (float)(texture.Height / Main.npcFrameCount[npc.type])) * npc.scale / 2f + npc.frame.Size() / 2 * npc.scale + new Vector2(0.0f, npc.gfxOffY);
				Main.spriteBatch.Draw(texture, GlowPosition2, new Microsoft.Xna.Framework.Rectangle?(npc.frame), GlowColor, npc.rotation, npc.frame.Size() / 2, npc.scale, spriteEffects, 0.0f);
			}
			Main.spriteBatch.Draw(texture, GlowPosition, new Microsoft.Xna.Framework.Rectangle?(npc.frame), AfterimageColor, npc.rotation, npc.frame.Size() / 2, npc.scale, spriteEffects, 0.0f);
		}
		public void DrawSpecialGlow(SpriteBatch spriteBatch, Color drawColor)
		{
			float num108 = 4;
			float num107 = (float)Math.Cos((double)(Main.GlobalTime % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;
			float num106 = 0f;

			SpriteEffects spriteEffects3 = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Vector2 vector33 = new Vector2(npc.Center.X, npc.Center.Y - 18) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity;
			Color color29 = new Color(127 - npc.alpha, 127 - npc.alpha, 127 - npc.alpha, 0).MultiplyRGBA(Color.Pink);
			for (int num103 = 0; num103 < 4; num103++)
			{
				Color color28 = color29;
				color28 = npc.GetAlpha(color28);
				color28 *= 1f - num107;
				Vector2 vector29 = new Vector2(npc.Center.X, npc.Center.Y - 18) + ((float)num103 / (float)num108 * 6.28318548f + npc.rotation + num106).ToRotationVector2() * (4f * num107 + 2f) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * (float)num103;
				Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/MoonWizardTwo/MoonWizardTwo_Glow"), vector29, npc.frame, color28, npc.rotation, npc.frame.Size() / 2f, npc.scale, spriteEffects3, 0f);
			}
			Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/MoonWizardTwo/MoonWizardTwo_Glow"), vector33, npc.frame, color29, npc.rotation, npc.frame.Size() / 2f, npc.scale, spriteEffects3, 0f);

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
			npc.TargetClosest(true);
			Player player = Main.player[npc.target];
			UpdateFrame(0.15f, 4, 9);
			npc.ai[0]++;
			if (npc.ai[0] == DASHAMOUNT)
			{
				npc.damage = 70;
				dashDirection = (player.Center + (player.velocity * 20)) - npc.Center;
				dashDistance = dashDirection.Length();
				dashDirection.Normalize();
				dashDirection *= speed;
				npc.velocity = dashDirection;

				npc.rotation = npc.velocity.ToRotation() + 1.57f;
				Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 81);
			}
			if (npc.ai[0] < DASHAMOUNT)
			{
				dashDirection = (player.Center + (player.velocity * 20)) - npc.Center;
				npc.rotation = dashDirection.ToRotation() + 1.57f;
				npc.velocity = Vector2.Zero;
			}
			if (npc.ai[0] > Math.Abs(dashDistance / speed) + DASHAMOUNT + 40)
			{
				npc.active = false;
			}
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frame.Width = 60;
			npc.frame.X = ((int)trueFrame % 3) * npc.frame.Width;
			npc.frame.Y = (((int)trueFrame - ((int)trueFrame % 3)) / 3) * npc.frame.Height;
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
