using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.CollideableNPC;
using SpiritMod.Utilities;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.StarjinxEvent
{
	public class SjinxPlatform : SpiritNPC, ISolidTopNPC
	{
		private const int FADEIN_TIME = 60;

		public override bool IsLoadingEnabled(Mod mod) => false;

		public bool Grappleable() => true;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Platform");
			Main.npcFrameCount[NPC.type] = 1;
		}

		public override void SetDefaults()
		{
			NPC.width = 300;
			NPC.height = 24;
			NPC.damage = 0;
			NPC.defense = 28;
			NPC.lifeMax = 1200;
			NPC.aiStyle = -1;
			NPC.HitSound = SoundID.NPCDeath1;
			NPC.DeathSound = SoundID.NPCDeath10;
			NPC.value = 0;
			NPC.knockBackResist = 0f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.dontCountMe = true;
			NPC.dontTakeDamage = true;
			NPC.alpha = 255;
		}

		private ref float TimeActive => ref NPC.ai[0];
		private ref float Offset => ref NPC.ai[1];

		public override void AI()
		{
			if (Offset == 0)
				Offset = Main.rand.NextFloat(0.1f, 1000f);

			if (TimeActive < FADEIN_TIME) //Rise upwards until fully faded in 
			{
				float BaseRiseSpeed = -2f;
				float progress = TimeActive / FADEIN_TIME;

				NPC.velocity.Y = BaseRiseSpeed * (1 - progress);
				NPC.alpha = (int)(255 * (1 - progress));
			}
			else //Sine wave movement afterwards
				NPC.velocity.Y = (float)Math.Sin((TimeActive + Offset - FADEIN_TIME) * 0.03f) * 0.4f;

			TimeActive++;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			float fadeProgress = Math.Min(TimeActive / FADEIN_TIME, 1);
			fadeProgress = EaseFunction.EaseQuarticIn.Ease(fadeProgress); //Nonlinear easing, starts slow then goes fast
			drawColor = StarjinxGlobalNPC.GetColorBrightness(drawColor, 1f);
			Color additiveCyan = new Color(74, 243, 255, 0);

			Texture2D npcTex = TextureAssets.Npc[NPC.type].Value;

			int numToDraw = 8;
			if (fadeProgress < 1)
			{
				for (int i = 0; i < numToDraw; i++)
				{
					float offsetDist = 6 * (1 - fadeProgress);
					float rotation = (i / (float)numToDraw) * MathHelper.TwoPi;

					Vector2 offset = Vector2.UnitX.RotatedBy(rotation) * offsetDist;
					float opacity = (1 - fadeProgress) * 0.5f;

					spriteBatch.Draw(npcTex, NPC.Center + offset - Main.screenPosition, NPC.frame, NPC.GetAlpha(Color.Lerp(additiveCyan, drawColor, fadeProgress) * opacity),
						NPC.rotation, NPC.frame.Size() / 2, NPC.scale, SpriteEffects.None, 0);
				}
			}

			spriteBatch.Draw(npcTex, NPC.Center - Main.screenPosition, NPC.frame, NPC.GetAlpha(Color.Lerp(additiveCyan, drawColor, fadeProgress)),
				NPC.rotation, NPC.frame.Size() / 2, NPC.scale, SpriteEffects.None, 0);

			return false;
		}

		public override bool CheckActive() => !ModContent.GetInstance<StarjinxEventWorld>().StarjinxActive;
	}

	public class SjinxPlatformMedium : SjinxPlatform
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.width = 450;
			NPC.height = 30;
		}
	}

	public class SjinxPlatformLarge : SjinxPlatform
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.width = 600;
			NPC.height = 50;
		}
	}
}