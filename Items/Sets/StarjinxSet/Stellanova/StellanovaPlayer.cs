using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarjinxSet.Stellanova
{
	public class StellanovaPlayer : ModPlayer
	{
		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			if(player.HeldItem.type == ModContent.ItemType<StellanovaCannon>())
			{
				layers.Insert(layers.FindIndex(x => x.Name == "HeldItem" && x.mod == "Terraria"), new PlayerLayer(mod.Name, "StellanovaHeld",
					delegate (PlayerDrawInfo info) { DrawItem(mod.GetTexture("Items/Sets/StarjinxSet/Stellanova/StellanovaCannon_held"), 
						mod.GetTexture("Items/Sets/StarjinxSet/Stellanova/StellanovaCannon_heldGlow"), info); }));
			}
		}

		public void DrawItem(Texture2D texture, Texture2D glow, PlayerDrawInfo info)
		{
			Item item = info.drawPlayer.HeldItem;
			if (info.shadow != 0f || info.drawPlayer.frozen || ((info.drawPlayer.itemAnimation <= 0 || item.useStyle == 0) && (item.holdStyle <= 0 || info.drawPlayer.pulley)) || info.drawPlayer.dead || (info.drawPlayer.wet && item.noWet))
				return;

			Rectangle drawFrame = texture.Bounds;
			int numFrames = 5;
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


			Main.playerDrawData.Add(new DrawData(
				texture,
				info.itemLocation - Main.screenPosition + offset,
				drawFrame,
				Lighting.GetColor((int)info.itemLocation.X/16, (int)info.itemLocation.Y/16),
				info.drawPlayer.itemRotation,
				origin,
				item.scale,
				info.spriteEffects,
				0
			)); 
			
			Main.playerDrawData.Add(new DrawData(
				 glow,
				 info.itemLocation - Main.screenPosition + offset,
				 drawFrame,
				 Color.White,
				 info.drawPlayer.itemRotation,
				 origin,
				 item.scale,
				 info.spriteEffects,
				 0
			 ));
		}
	}
}