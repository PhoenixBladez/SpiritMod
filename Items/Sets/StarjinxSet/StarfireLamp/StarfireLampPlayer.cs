using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarjinxSet.StarfireLamp
{
	public class StarfireLampPlayer : ModPlayer
	{
		public int TwinkleTime { get; set; }

		public override void Initialize() => TwinkleTime = 0;
		public override void ResetEffects() => TwinkleTime = Math.Max(TwinkleTime - 1, 0);

		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			if(player.HeldItem.type == ModContent.ItemType<StarfireLamp>())
			{
				layers.Insert(layers.FindIndex(x => x.Name == "HeldItem" && x.mod == "Terraria"), new PlayerLayer(mod.Name, "StarfireLampHeld",
					delegate (PlayerDrawInfo info) { DrawItem(mod.GetTexture("Items/Sets/StarjinxSet/StarfireLamp/StarfireLamp"),
						mod.GetTexture("Items/Sets/StarjinxSet/StarfireLamp/StarfireLampGlow"), info); }));
			}
		}

		public void DrawItem(Texture2D texture, Texture2D glow, PlayerDrawInfo info)
		{
			Item item = info.drawPlayer.HeldItem;
			if (info.shadow != 0f || info.drawPlayer.frozen || info.drawPlayer.dead)
				return;

			Vector2 drawPosition = info.drawPlayer.MountedCenter - new Vector2(0, 60 + ((float)Math.Sin(Main.GameUpdateCount / 18f) * 4)) - Main.screenPosition;


			Main.playerDrawData.Add(new DrawData(
				texture,
				drawPosition,
				null,
				Color.White,
				0,
				texture.Size()/2,
				item.scale,
				info.spriteEffects,
				0
			)); 
			
			Main.playerDrawData.Add(new DrawData(
				 glow,
				 drawPosition,
				 null,
				 Color.White,
				 0,
				 texture.Size() / 2,
				 item.scale,
				 info.spriteEffects,
				 0
			 ));
		}
	}
}