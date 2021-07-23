using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.StarjinxEvent
{
    public interface StarjinxEnemy
	{
		void DrawPathfinderOutline(SpriteBatch spriteBatch);
	}
	public static class PathfinderOutlineDraw
	{
		public static void DrawAfterImage(SpriteBatch spriteBatch, NPC npc, Rectangle frame, Vector2 offset, Color color, float opacity, float startScale, float endScale, Vector2 origin) => DrawAfterImage(spriteBatch, npc, frame, offset, color, color, opacity, startScale, endScale, origin);
		public static void DrawAfterImage(SpriteBatch spriteBatch, NPC npc, Rectangle frame, Vector2 offset, Color startColor, Color endColor, float opacity, float startScale, float endScale, Vector2 origin)
		{
			SpriteEffects spriteEffects = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Texture2D tex = Main.npcTexture[npc.type];
			for (int i = 1; i < 10; i++)
			{
				Color color = new Color((int)sbyte.MaxValue, (int)sbyte.MaxValue, (int)sbyte.MaxValue, 0).MultiplyRGBA(Color.Lerp(startColor, endColor, i / 10f)) * 5;
				spriteBatch.Draw(tex, new Vector2(npc.Center.X, npc.Center.Y) + offset - Main.screenPosition + new Vector2(0, npc.gfxOffY), frame, color * opacity, npc.rotation, origin, MathHelper.Lerp(startScale, endScale, i / 10f), spriteEffects, 0f);
			}
		}
	}
}