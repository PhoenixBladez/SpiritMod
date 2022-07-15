using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarjinxSet.Sagittarius
{
	public class SagittariusLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.HeldItem);

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (drawInfo.drawPlayer.HeldItem.type == ModContent.ItemType<Sagittarius>() && false)
				DrawItem(Mod.Assets.Request<Texture2D>("Items/Sets/StarjinxSet/Sagittarius/Sagittarius_held").Value, Mod.Assets.Request<Texture2D>("Items/Sets/StarjinxSet/Sagittarius/Sagittarius_heldGlow").Value, drawInfo);
		}

		public void DrawItem(Texture2D texture, Texture2D glow, PlayerDrawSet info)
		{
			Item item = info.drawPlayer.HeldItem;
			if (info.shadow != 0f || info.drawPlayer.frozen || ((info.drawPlayer.itemAnimation <= 0 || item.useStyle == 0) && (item.holdStyle <= 0 || info.drawPlayer.pulley)) || info.drawPlayer.dead || (info.drawPlayer.wet && item.noWet))
				return;

			Rectangle drawFrame = texture.Bounds;
			int numFrames = 9;
			drawFrame.Height /= numFrames;
			drawFrame.Y = (drawFrame.Height) * (int)(numFrames * ((info.drawPlayer.HeldItem.useAnimation - info.drawPlayer.itemAnimation) / (float)info.drawPlayer.HeldItem.useAnimation));

			Vector2 offset = new Vector2(10, texture.Height / (2 * numFrames));

			ItemLoader.HoldoutOffset(info.drawPlayer.gravDir, item.type, ref offset);
			Vector2 origin = new Vector2(-offset.X, texture.Height / (2 * numFrames));

			offset = new Vector2(texture.Width / 2, offset.Y);
			if (info.drawPlayer.direction == -1)
			{
				origin.X = texture.Width - origin.X;
				offset.X = texture.Width / 4;
			}


			info.DrawDataCache.Add(new DrawData(
				texture,
				info.ItemLocation - Main.screenPosition + offset,
				drawFrame,
				Lighting.GetColor((int)info.ItemLocation.X / 16, (int)info.ItemLocation.Y / 16),
				info.drawPlayer.itemRotation,
				origin,
				item.scale,
				info.itemEffect,
				0
			));

			info.DrawDataCache.Add(new DrawData(
				 glow,
				 info.ItemLocation - Main.screenPosition + offset,
				 drawFrame,
				 Color.White,
				 info.drawPlayer.itemRotation,
				 origin,
				 item.scale,
				 info.itemEffect,
				 0
			 ));
		}
	}
}