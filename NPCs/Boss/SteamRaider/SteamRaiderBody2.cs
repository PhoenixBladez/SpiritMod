using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.SteamRaider
{
	public class SteamRaiderBody2 : SteamRaiderBody
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Starplate Voyager");

		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.width = 30;
			NPC.height = 14;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (Exposed) {
				Microsoft.Xna.Framework.Color color1 = Lighting.GetColor((int)((double)NPC.position.X + (double)NPC.width * 0.5) / 16, (int)(((double)NPC.position.Y + (double)NPC.height * 0.5) / 16.0));
				Vector2 drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, NPC.height * 0.5f);
				int r1 = (int)color1.R;
				drawOrigin.Y += 34f;
				drawOrigin.Y += 8f;
				--drawOrigin.X;
				Vector2 position1 = NPC.Bottom - Main.screenPosition;
				Texture2D texture2D2 = TextureAssets.GlowMask[239].Value;
				float num11 = (float)((double)Main.GlobalTimeWrappedHourly % 1.0 / 1.0);
				float num12 = num11;
				if ((double)num12 > 0.5)
					num12 = 1f - num11;
				if ((double)num12 < 0.0)
					num12 = 0.0f;
				float num13 = (float)(((double)num11 + 0.5) % 1.0);
				float num14 = num13;
				if ((double)num14 > 0.5)
					num14 = 1f - num13;
				if ((double)num14 < 0.0)
					num14 = 0.0f;
				Microsoft.Xna.Framework.Rectangle r2 = texture2D2.Frame(1, 1, 0, 0);
				drawOrigin = r2.Size() / 2f;
				Vector2 position3 = position1 + new Vector2(0.0f, -20f);
				Microsoft.Xna.Framework.Color color3 = new Microsoft.Xna.Framework.Color(84, 207, 255) * 1.6f;
				Main.spriteBatch.Draw(texture2D2, position3, r2, color3, NPC.rotation, drawOrigin, NPC.scale * 0.5f, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
				float num15 = 1f + num11 * 0.75f;
				Main.spriteBatch.Draw(texture2D2, position3, r2, color3 * num12, NPC.rotation, drawOrigin, NPC.scale * 0.5f * num15, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
				float num16 = 1f + num13 * 0.75f;
				Main.spriteBatch.Draw(texture2D2, position3, r2, color3 * num14, NPC.rotation, drawOrigin, NPC.scale * 0.5f * num16, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
				Texture2D texture2D3 = TextureAssets.Extra[89].Value;
				Microsoft.Xna.Framework.Rectangle r3 = texture2D3.Frame(1, 1, 0, 0);
				drawOrigin = r3.Size() / 2f;
				Vector2 scale = new Vector2(0.75f, 1f + num16) * 1.5f;
				float num17 = 1f + num13 * 0.75f;
				GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>("NPCs/Boss/SteamRaider/SteamRaiderBody2_Glow").Value);

			}
		}
	}
}