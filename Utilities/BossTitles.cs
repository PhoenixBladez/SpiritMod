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

		public enum DrawCondition
		{
			SpiritBosses,
			SpiritVanilla,
			AllBosses,
			Off
		}


		public static void SyncNPCType(int Type)
		{
			if (Main.netMode != NetmodeID.SinglePlayer)
				SpiritMod.WriteToPacket(SpiritMod.instance.GetPacket(), (byte)MessageType.BossTitle, Type).Send();

			else
				SetNPCType(Type);
		}

		public static void SetNPCType(int Type)
		{
			switch (ModContent.GetInstance<SpiritClientConfig>().DrawCondition)
			{
				case DrawCondition.SpiritBosses:
					if (ModContent.GetModNPC(Type) == null) //return if from vanilla
						return;
					if (ModContent.GetModNPC(Type).mod != SpiritMod.instance) //then return if not from spirit mod
						return;
					break;

				case DrawCondition.SpiritVanilla:
					if (ModContent.GetModNPC(Type) != null) //check if not vanilla
					{
						if (ModContent.GetModNPC(Type).mod != SpiritMod.instance) //then return if not from spirit mod
							return;
					}

					break;

				case DrawCondition.Off: //always return
					return;
			}
			NPCType = Type;
			TimeToDisplay = DisplayTimeMax;
		}

		public static void DrawTitle(SpriteBatch spriteBatch)
		{
			if(!Main.gamePaused)
				TimeToDisplay = Math.Max(TimeToDisplay - 1, 0);

			float fadespeed = 3;
			float Opacity = MathHelper.Clamp((float)-Math.Sin((DisplayTimeMax - (TimeToDisplay / (float)DisplayTimeMax)) * MathHelper.Pi) * fadespeed, 0, 1);
			string DisplayName = Lang.GetNPCName(NPCType).ToString();
			bool twins = NPCType == NPCID.Retinazer || NPCType == NPCID.Spazmatism;
			if (twins)
				DisplayName = Language.GetTextValue("Enemies.TheTwins");

			if (NPCType == NPCID.MoonLordCore)
				DisplayName = Language.GetTextValue("Enemies.MoonLord");


			Vector2 offset = Main.fontDeathText.MeasureString(DisplayName);
			Vector2 NameDrawPos = new Vector2((Main.screenWidth / 2) - offset.X / 2, Main.screenHeight / 8);

			//draw bloom
			Texture2D bloom = SpiritMod.Instance.GetTexture("Effects/Masks/Extra_A1");
			Vector2 bloomscale = new Vector2((offset.X / bloom.Width) * 1.5f, offset.Y / bloom.Height) * 1.25f;
			spriteBatch.Draw(bloom, NameDrawPos + offset / 2, null, Color.Black * Opacity * 1.25f, 0, bloom.Size()/2, bloomscale, SpriteEffects.None, 0);

			//draw the boss's name
			Utils.DrawBorderStringFourWay(spriteBatch, Main.fontDeathText, DisplayName, NameDrawPos.X,
				NameDrawPos.Y, Color.White * Opacity, Color.Black * Opacity, Vector2.Zero);

			//draw the underline between the name and head icon
			spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.UIScaleMatrix);
			Vector2 UnderlineDrawPos = NameDrawPos + offset/2;
			UnderlineDrawPos.Y += offset.Y / 3;
			Texture2D Underline = SpiritMod.instance.GetTexture("Textures/TitleUnderline");
			Vector2 underlinescale = new Vector2((offset.X / Underline.Width) * 0.75f, 0.33f);

			spriteBatch.Draw(Underline, UnderlineDrawPos, null, Color.White * Opacity, 0, Underline.Size() / 2, underlinescale, SpriteEffects.None, 0);
			spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.UIScaleMatrix);

			//draw the head icon
			Texture2D headIcon = GetHeadIcon();
			Vector2 HeadDrawPos = UnderlineDrawPos;
			HeadDrawPos.Y += Underline.Height;
			Vector2 Origin = new Vector2(headIcon.Width / 2, 0);
			if (headIcon == null)
				return;

			if (twins) //special exception for twins
			{
				headIcon = Main.npcHeadBossTexture[15];
				Texture2D headIcon2 = Main.npcHeadBossTexture[20];

				spriteBatch.Draw(headIcon, HeadDrawPos - (Vector2.UnitX * headIcon.Width), null, Color.White * Opacity, 0, Origin, 1, SpriteEffects.None, 0);
				spriteBatch.Draw(headIcon2, HeadDrawPos + (Vector2.UnitX * headIcon.Width), null, Color.White * Opacity, 0, Origin, 1, SpriteEffects.None, 0);
				return;
			}

			spriteBatch.Draw(headIcon, HeadDrawPos, null, Color.White * Opacity, 0, Origin, 1, SpriteEffects.None, 0);
		}

		private static Texture2D GetHeadIcon()
		{
			if (NPCType == NPCID.MoonLordCore)
				return Main.npcHeadBossTexture[8];

			if (NPCType == NPCID.Golem)
				return Main.npcHeadBossTexture[5];

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
