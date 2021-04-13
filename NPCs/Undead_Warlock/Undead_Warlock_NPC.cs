using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Undead_Warlock
{
	public class Undead_Warlock_NPC : GlobalNPC
	{
		public bool isMagicalShrapneled = false;
		public bool isNecrofied = false;
		public int necroTimer = 0;
		public int shrapnelTimer = 0;
		
		public override bool InstancePerEntity
		{
			get
			{
				return true;
			}
		}

		public override void UpdateLifeRegen(NPC npc, ref int damage)
		{
			if (isNecrofied)
			{
				npc.lifeRegen += 20;
			}
			if (isMagicalShrapneled)
			{
				npc.lifeRegen -= 10;
			}
		}
		
		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
		{
			if (isNecrofied)
			{
				int num7 = 16;
				float num8 = (float) (Math.Cos((double) Main.GlobalTime % 2.40000009536743 / 2.40000009536743 * 6.28318548202515) / 2.0 + 0.5);
				float num10 = 0.0f;
				float addY = 0f;
				float addHeight = 0f;
				SpriteEffects spriteEffects = SpriteEffects.None;
				if (npc.spriteDirection == 1)
					spriteEffects = SpriteEffects.FlipHorizontally;
				Texture2D texture = Main.npcTexture[npc.type];
				Vector2 vector2_3 = new Vector2((float) (Main.npcTexture[npc.type].Width / 2), (float) (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
				Vector2 position1 = npc.Center - Main.screenPosition - new Vector2((float) texture.Width, (float) (texture.Height / Main.npcFrameCount[npc.type])) * npc.scale / 2f + vector2_3 * npc.scale + new Vector2(0.0f, addY + addHeight + npc.gfxOffY);
				Microsoft.Xna.Framework.Color color2 = new Microsoft.Xna.Framework.Color((int) sbyte.MaxValue - npc.alpha, (int) sbyte.MaxValue - npc.alpha, (int) sbyte.MaxValue - npc.alpha, 0).MultiplyRGBA(Microsoft.Xna.Framework.Color.PaleGreen) * 0.7f;
				for (int index2 = 0; index2 < num7; ++index2)
				{
					Microsoft.Xna.Framework.Color newColor2 = color2;
					Microsoft.Xna.Framework.Color color3 = npc.GetAlpha(newColor2) * (1f - num8) * 0.7f;
					Vector2 position2 = new Vector2 (npc.Center.X,npc.Center.Y-2) + ((float) ((double) index2 / (double) num7 * 6.28318548202515) + npc.rotation + num10).ToRotationVector2() * (float) (2.0 * (double) num8 + 2.0) - Main.screenPosition - new Vector2((float) texture.Width, (float) (texture.Height / Main.npcFrameCount[npc.type])) * npc.scale / 2f + vector2_3 * npc.scale + new Vector2(0.0f, addY + addHeight + npc.gfxOffY);
					Main.spriteBatch.Draw(Main.npcTexture[npc.type], position2, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color3, npc.rotation, vector2_3, npc.scale, spriteEffects, 0.0f);
				}
				Main.spriteBatch.Draw(Main.npcTexture[npc.type], position1, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color2, npc.rotation, vector2_3, npc.scale, spriteEffects, 0.0f);
			}
			if (isMagicalShrapneled)
			{
				int num7 = 16;
				float num8 = (float) (Math.Cos((double) Main.GlobalTime % 2.40000009536743 / 2.40000009536743 * 6.28318548202515) / 2.0 + 0.5);
				float num10 = 0.0f;
				float addY = 0f;
				float addHeight = 0f;
				SpriteEffects spriteEffects = SpriteEffects.None;
				if (npc.spriteDirection == 1)
					spriteEffects = SpriteEffects.FlipHorizontally;
				Texture2D texture = Main.npcTexture[npc.type];
				Vector2 vector2_3 = new Vector2((float) (Main.npcTexture[npc.type].Width / 2), (float) (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
				Vector2 position1 = npc.Center - Main.screenPosition - new Vector2((float) texture.Width, (float) (texture.Height / Main.npcFrameCount[npc.type])) * npc.scale / 2f + vector2_3 * npc.scale + new Vector2(0.0f, addY + addHeight + npc.gfxOffY);
				Microsoft.Xna.Framework.Color color2 = new Microsoft.Xna.Framework.Color((int) sbyte.MaxValue - npc.alpha, (int) sbyte.MaxValue - npc.alpha, (int) sbyte.MaxValue - npc.alpha, 0).MultiplyRGBA(Microsoft.Xna.Framework.Color.MediumPurple) * 0.7f;
				for (int index2 = 0; index2 < num7; ++index2)
				{
					Microsoft.Xna.Framework.Color newColor2 = color2;
					Microsoft.Xna.Framework.Color color3 = npc.GetAlpha(newColor2) * (1f - num8) * 0.75f;
					Vector2 position2 = new Vector2 (npc.Center.X,npc.Center.Y-2) + ((float) ((double) index2 / (double) num7 * 6.28318548202515) + npc.rotation + num10).ToRotationVector2() * 2f - Main.screenPosition - new Vector2((float) texture.Width, (float) (texture.Height / Main.npcFrameCount[npc.type])) * npc.scale / 2f + vector2_3 * npc.scale + new Vector2(0.0f, addY + addHeight + npc.gfxOffY);
					Main.spriteBatch.Draw(Main.npcTexture[npc.type], position2, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color2, npc.rotation, vector2_3, npc.scale, spriteEffects, 0.0f);
				}
				Main.spriteBatch.Draw(Main.npcTexture[npc.type], position1, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color2, npc.rotation, vector2_3, npc.scale, spriteEffects, 0.0f);
			}
		}
	}
}