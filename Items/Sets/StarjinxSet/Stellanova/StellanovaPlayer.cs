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

		private bool CanDraw => player.shadow == 0f && !player.frozen && player.itemAnimation > 0 && !player.dead;
		private int CurFrame => (int)(NumFrames * ((player.HeldItem.useAnimation - player.itemAnimation) / (float)player.HeldItem.useAnimation));
		private Rectangle DrawFrame
		{
			get
			{
				Rectangle drawFrame = mod.GetTexture(TexturePath).Bounds;
				drawFrame.Height /= NumFrames;
				drawFrame.Y = drawFrame.Height * CurFrame;
				return drawFrame;
			}
		}

		private Vector2 HoldOffset()
		{
			Vector2 offset = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
			offset -= new Vector2(30 * player.direction, 30).RotatedBy(player.itemRotation);
			if (player.direction > 0)
				offset.X += player.width;
			else
				offset.X += player.width / 2;

			return offset;
		}
		private Vector2 TextureOrigin
		{
			get
			{
				Vector2 origin = new Vector2(0, DrawFrame.Height / 2);
				if (player.direction < 0)
					origin.X = mod.GetTexture(TexturePath).Width;

				return origin;
			}
		}

		//Add in custom draw layers
		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			if(player.HeldItem.type == ModContent.ItemType<StellanovaCannon>() && false)
				layers.Insert(layers.FindIndex(x => x.Name == "HeldItem" && x.mod == "Terraria"), new PlayerLayer(mod.Name, "StellanovaHeld",
					delegate (PlayerDrawInfo info) { DrawItem(mod.GetTexture("Items/Sets/StarjinxSet/Stellanova/StellanovaCannon_held"), info); }));
		}

		public override void PostUpdate()
		{
			if (player.HeldItem.type == ModContent.ItemType<StellanovaCannon>() && false)
				player.GetModPlayer<ExtraDrawOnPlayer>().DrawDict.Add(delegate (SpriteBatch sB) { DrawAdditiveLayer(sB); }, ExtraDrawOnPlayer.DrawType.Additive);
		}

		//Draw the item and its glowmask on the player
		public void DrawItem(Texture2D texture, PlayerDrawInfo info)
		{
			if (!CanDraw || info.shadow != 0f)
				return;

			Color lightColor = Lighting.GetColor((int)info.itemLocation.X / 16, (int)info.itemLocation.Y / 16);
			Vector2 offset = HoldOffset();

			Main.playerDrawData.Add(new DrawData(texture, info.itemLocation - Main.screenPosition + offset, DrawFrame, lightColor, 
				player.itemRotation, TextureOrigin, player.HeldItem.scale, info.spriteEffects, 0)); 
		}

		public void DrawAdditiveLayer(SpriteBatch sB)
		{
			Texture2D glowmask = mod.GetTexture(GlowmaskPath);
			if (!CanDraw)
				return;

			SpriteEffects effects = player.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Vector2 offset = HoldOffset();

			PulseDraw.DrawPulseEffect((float)Math.Asin(-0.6), 8, 12, delegate (Vector2 posOffset, float opacityMod)
			{
				sB.Draw(glowmask, player.itemLocation + posOffset - Main.screenPosition + offset, DrawFrame, Color.White * opacityMod * 0.4f, 
					player.itemRotation, TextureOrigin, player.HeldItem.scale, effects, 0);
			});

			sB.Draw(glowmask, player.itemLocation - Main.screenPosition + offset, DrawFrame, Color.White * 0.7f, player.itemRotation, TextureOrigin, player.HeldItem.scale, effects, 0);
		}
	}
}