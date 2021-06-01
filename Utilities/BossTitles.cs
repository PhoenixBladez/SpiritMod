using Terraria;
using System;
using SpiritMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SpiritMod.Utilities
{
	public static class BossTitles
	{
		public static int NPCType = 0;
		public static int TimeToDisplay = 0;

		private const int DisplayTimeMax = (int)(60 * 3.5); //3.5 seconds

		public static void Reset()
		{
			NPCType = 0;
			TimeToDisplay = 0;
		}

		public static void SetNPCType(int Type)
		{
			NPCType = Type;
			TimeToDisplay = DisplayTimeMax;
		}

		public static void DrawTitle(SpriteBatch spriteBatch)
		{
			float fadespeed = 3;
			float Opacity = MathHelper.Clamp((float)-Math.Sin((DisplayTimeMax - (TimeToDisplay / (float)DisplayTimeMax)) * MathHelper.Pi) * fadespeed, 0, 1);
			string DisplayName = Lang.GetNPCName(NPCType).ToString();
			bool twins = NPCType == NPCID.Retinazer || NPCType == NPCID.Spazmatism;
			if (twins)
				DisplayName = Language.GetTextValue("Enemies.TheTwins");

			if (NPCType == NPCID.MoonLordCore)
				DisplayName = Language.GetTextValue("Enemies.MoonLord");


			Vector2 offset = Main.fontDeathText.MeasureString(DisplayName);
			Utils.DrawBorderStringFourWay(spriteBatch, Main.fontDeathText, DisplayName, (Main.screenWidth / 2) - offset.X/2, 
				Main.screenHeight / 8, Color.White * Opacity, Color.Black * Opacity, Vector2.Zero);

			Texture2D headIcon = GetHeadIcon();
			Vector2 drawPos = new Vector2(Main.screenWidth / 2, Main.screenHeight / 8 + offset.Y);
			if (headIcon == null)
				return;

			if (twins)
			{
				headIcon = Main.npcHeadBossTexture[15];
				Texture2D headIcon2 = Main.npcHeadBossTexture[20];

				spriteBatch.Draw(headIcon, drawPos - (Vector2.UnitX * headIcon.Width), null, Color.White * Opacity, 0, headIcon.Size() / 2, 1, SpriteEffects.None, 0);
				spriteBatch.Draw(headIcon2, drawPos + (Vector2.UnitX * headIcon.Width), null, Color.White * Opacity, 0, headIcon.Size() / 2, 1, SpriteEffects.None, 0);
				return;
			}

			spriteBatch.Draw(headIcon, drawPos, null, Color.White * Opacity, 0, headIcon.Size() / 2, 1, SpriteEffects.None, 0);
		}

		private static Texture2D GetHeadIcon()
		{
			NPC dummynpc = new NPC();
			dummynpc.SetDefaults(NPCType);
			if(dummynpc.modNPC == null)
			{
				if (dummynpc.GetBossHeadTextureIndex() >= 0)
					return Main.npcHeadBossTexture[dummynpc.GetBossHeadTextureIndex()];
			}

			return ModContent.TextureExists(dummynpc.modNPC.BossHeadTexture) ? ModContent.GetTexture(dummynpc.modNPC.BossHeadTexture) : null;
		}
	}
}
