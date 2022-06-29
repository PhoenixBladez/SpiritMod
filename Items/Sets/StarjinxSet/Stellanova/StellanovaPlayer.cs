using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.GlobalClasses.Players;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarjinxSet.Stellanova
{
	public class StellanovaPlayer : ModPlayer
	{
		//Constant values
		private const string TexturePath = "Items/Sets/StarjinxSet/Stellanova/StellanovaCannon_held";
		private const int NumFrames = 5;

		//Helper properties
		private string GlowmaskPath => TexturePath + "Glow";

		private bool CanDraw => Player.shadow == 0f && !Player.frozen && Player.itemAnimation > 0 && !Player.dead;
		private int CurFrame => (int)(NumFrames * ((Player.HeldItem.useAnimation - Player.itemAnimation) / (float)Player.HeldItem.useAnimation));
		private Rectangle DrawFrame
		{
			get
			{
				Rectangle drawFrame = Mod.Assets.Request<Texture2D>(TexturePath).Value.Bounds;
				drawFrame.Height /= NumFrames;
				drawFrame.Y = drawFrame.Height * CurFrame;
				return drawFrame;
			}
		}

		private Vector2 HoldOffset()
		{
			Vector2 offset = Main.OffsetsPlayerOnhand[Player.bodyFrame.Y / 56] * 2f;
			offset -= new Vector2(30 * Player.direction, 30).RotatedBy(Player.itemRotation);
			if (Player.direction > 0)
				offset.X += Player.width;
			else
				offset.X += Player.width / 2;

			return offset;
		}
		private Vector2 TextureOrigin
		{
			get
			{
				Vector2 origin = new Vector2(0, DrawFrame.Height / 2);
				if (Player.direction < 0)
					origin.X = Mod.Assets.Request<Texture2D>(TexturePath).Value.Width;

				return origin;
			}
		}

		//Add in custom draw layers
		public override void ModifyDrawLayers(List<PlayerDrawLayer> layers)
		{
			if(Player.HeldItem.type == ModContent.ItemType<StellanovaCannon>() && false)
				layers.Insert(layers.FindIndex(x => x.Name == "HeldItem" && x.mod == "Terraria"), new PlayerDrawLayer(Mod.Name, "StellanovaHeld",
					delegate (PlayerDrawSet info) { DrawItem(Mod.Assets.Request<Texture2D>("Items/Sets/StarjinxSet/Stellanova/StellanovaCannon_held").Value, info); }));
		}

		public override void PostUpdate()
		{
			if (Player.HeldItem.type == ModContent.ItemType<StellanovaCannon>() && false)
				Player.GetModPlayer<ExtraDrawOnPlayer>().DrawDict.Add(delegate (SpriteBatch sB) { DrawAdditiveLayer(sB); }, ExtraDrawOnPlayer.DrawType.Additive);
		}

		//Draw the item and its glowmask on the player
		public void DrawItem(Texture2D texture, PlayerDrawSet info)
		{
			if (!CanDraw || info.shadow != 0f)
				return;

			Color lightColor = Lighting.GetColor((int)info.itemLocation.X / 16, (int)info.itemLocation.Y / 16);
			Vector2 offset = HoldOffset();

			Main.playerDrawData.Add(new DrawData(texture, info.itemLocation - Main.screenPosition + offset, DrawFrame, lightColor, 
				Player.itemRotation, TextureOrigin, Player.HeldItem.scale, info.spriteEffects, 0)); 
		}

		public void DrawAdditiveLayer(SpriteBatch sB)
		{
			Texture2D glowmask = Mod.Assets.Request<Texture2D>(GlowmaskPath).Value;
			if (!CanDraw)
				return;

			SpriteEffects effects = Player.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Vector2 offset = HoldOffset();

			PulseDraw.DrawPulseEffect((float)Math.Asin(-0.6), 8, 12, delegate (Vector2 posOffset, float opacityMod)
			{
				sB.Draw(glowmask, Player.itemLocation + posOffset - Main.screenPosition + offset, DrawFrame, Color.White * opacityMod * 0.4f, 
					Player.itemRotation, TextureOrigin, Player.HeldItem.scale, effects, 0);
			});

			sB.Draw(glowmask, Player.itemLocation - Main.screenPosition + offset, DrawFrame, Color.White * 0.7f, Player.itemRotation, TextureOrigin, Player.HeldItem.scale, effects, 0);
		}
	}
}