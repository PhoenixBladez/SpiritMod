using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.CollideableNPC;
using SpiritMod.Utilities;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.StarjinxEvent
{
	public class SjinxPlatform : SpiritNPC, ISolidTopNPC
	{
		private const int FADEIN_TIME = 60;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Platform");
			Main.npcFrameCount[npc.type] = 1;
		}

		public override void SetDefaults()
		{
			npc.width = 300;
			npc.height = 24;
			npc.damage = 0;
			npc.defense = 28;
			npc.lifeMax = 1200;
			npc.aiStyle = -1;
			npc.HitSound = SoundID.NPCDeath1;
			npc.DeathSound = SoundID.NPCDeath10;
			npc.value = 0;
			npc.knockBackResist = 0f;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.dontCountMe = true;
			npc.dontTakeDamage = true;
			npc.alpha = 255;
		}

		private ref float TimeActive => ref npc.ai[0];

		public override void AI()
		{
			if(TimeActive < FADEIN_TIME) //Rise upwards until fully faded in 
			{
				float BaseRiseSpeed = -2f;
				float progress = TimeActive / FADEIN_TIME;

				npc.velocity.Y = BaseRiseSpeed * (1 - progress);
				npc.alpha = (int)(255 * (1 - progress));
			}
			else //Sine wave movement afterwards
				npc.velocity.Y = (float)Math.Sin((TimeActive - FADEIN_TIME) * 0.03f) * 0.4f;

			TimeActive++;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			float fadeProgress = Math.Min(TimeActive / FADEIN_TIME, 1);
			fadeProgress = EaseFunction.EaseQuarticIn.Ease(fadeProgress); //Nonlinear easing, starts slow then goes fast
			Color additiveCyan = new Color(74, 243, 255, 0);

			Texture2D npcTex = Main.npcTexture[npc.type];

			int numToDraw = 8;
			if(fadeProgress < 1)
				for(int i = 0; i < numToDraw; i++)
				{
					float offsetDist = 6 * (1 - fadeProgress);
					float rotation = (i / (float)numToDraw) * MathHelper.TwoPi;

					Vector2 offset = Vector2.UnitX.RotatedBy(rotation) * offsetDist;
					float opacity = (1 - fadeProgress) * 0.5f;

					spriteBatch.Draw(npcTex, npc.Center + offset - Main.screenPosition, npc.frame, npc.GetAlpha(Color.Lerp(additiveCyan, drawColor, fadeProgress) * opacity),
						npc.rotation, npc.frame.Size() / 2, npc.scale, SpriteEffects.None, 0);
				}

			spriteBatch.Draw(npcTex, npc.Center - Main.screenPosition, npc.frame, npc.GetAlpha(Color.Lerp(additiveCyan, drawColor, fadeProgress)),
				npc.rotation, npc.frame.Size() / 2, npc.scale, SpriteEffects.None, 0);

			return false;
		}

		public override bool CheckActive() => !ModContent.GetInstance<StarjinxEventWorld>().StarjinxActive;
	}

	public class SjinxPlatformMedium : SjinxPlatform
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.width = 450;
			npc.height = 30;
		}
	}

	public class SjinxPlatformLarge : SjinxPlatform
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.width = 600;
			npc.height = 50;
		}
	}
}