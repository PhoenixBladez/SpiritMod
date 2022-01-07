using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using SpiritMod.Utilities;
using System;

namespace SpiritMod.NPCs.StarjinxEvent
{
    public interface IStarjinxEnemy
	{
		void DrawPathfinderOutline(SpriteBatch spriteBatch);
	}

	public static class PathfinderOutlineDraw
	{
		public static void DrawAfterImage(SpriteBatch spriteBatch, NPC npc, Rectangle frame, Vector2 offset, Vector2 origin)
		{
			SpriteEffects spriteEffects = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Vector2 center = npc.Center + new Vector2(0, npc.gfxOffY);
			Texture2D tex = Main.npcTexture[npc.type];

			DrawAfterImage(spriteBatch, tex, center, frame, offset, npc.Opacity, npc.rotation, npc.scale, origin, spriteEffects);
		}

		public static void DrawAfterImage(SpriteBatch spriteBatch, Texture2D tex, Vector2 position, Rectangle frame, Vector2 offset, float opacity, float rotation, float scale, Vector2 origin, SpriteEffects effects)
		{
			float baseOpacity = 0.75f;
			PulseDraw.DrawPulseEffect((float)Math.Asin(-0.6f), 12, 35, delegate (Vector2 posOffset, float pulseEffectOpacity)
			{
				spriteBatch.Draw(tex, position + offset + posOffset - Main.screenPosition, 
					frame, new Color(230, 55, 166) * baseOpacity * opacity * pulseEffectOpacity, rotation, origin, scale, effects, 0f);
			});
		}
	}
}