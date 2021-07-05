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
		public const int MaxTwinkleTime = 12;

		public NPC LampTargetNPC { get; set; }
		public int LampTargetTime { get; set; }
		public const int MaxTargetTime = 480;

		public override void Initialize()
		{
			TwinkleTime = 0;
			LampTargetNPC = null;
			LampTargetTime = 0;
		}

		public override void ResetEffects()
		{
			TwinkleTime = Math.Max(TwinkleTime - 1, 0);
			LampTargetTime = Math.Max(LampTargetTime - 1, 0);
			if (LampTargetNPC != null)
			{
				LampTargetNPC = (LampTargetNPC.active && LampTargetNPC.CanBeChasedBy(player)) ? LampTargetNPC : null;
			}
			else
				LampTargetTime = 0;

			if (LampTargetTime == 0)
				LampTargetNPC = null;
		}

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

			float Timer = (float)Math.Sin(Main.GameUpdateCount / 24f);
			Vector2 drawPosition = info.drawPlayer.MountedCenter - new Vector2(0, 60 + (Timer * 4));


			Main.playerDrawData.Add(new DrawData(
				texture,
				drawPosition - Main.screenPosition,
				null,
				Lighting.GetColor(drawPosition.ToTileCoordinates().X, drawPosition.ToTileCoordinates().Y),
				0,
				texture.Size()/2,
				item.scale,
				info.spriteEffects,
				0));

			for (int i = 0; i < 8; i++)
			{
				Vector2 offset = Vector2.UnitX.RotatedBy(MathHelper.TwoPi * i / 8f) * Math.Max(Timer, 0) * 8;
				Main.playerDrawData.Add(new DrawData(
				 glow,
				 drawPosition + offset - Main.screenPosition,
				 null,
				 Color.White * Math.Max(1 - Timer, 0),
				 0,
				 texture.Size() / 2,
				 item.scale,
				 info.spriteEffects,
				 0));
			}

			Main.playerDrawData.Add(new DrawData(
				 glow,
				 drawPosition - Main.screenPosition,
				 null,
				 Color.White,
				 0,
				 texture.Size() / 2,
				 item.scale,
				 info.spriteEffects,
				 0));

			if(TwinkleTime > 0)
			{
				float rotation = player.direction * (TwinkleTime / (float)MaxTwinkleTime) * MathHelper.Pi;
				float opacity = 0.8f * (MaxTwinkleTime - Math.Abs((MaxTwinkleTime / 2) - TwinkleTime)) / (MaxTwinkleTime / 2);
				Vector2 scale = new Vector2(1, 0.6f) * ((MaxTwinkleTime / 2) - Math.Abs((MaxTwinkleTime/2) - TwinkleTime)) / (MaxTwinkleTime / 2);
				Texture2D startex = Main.extraTexture[89];
				Vector2 offset = new Vector2(0, 6);

				DrawData data = new DrawData(
					 startex,
					 drawPosition + offset - Main.screenPosition,
					 null,
					 new Color(255, 147, 113) * opacity,
					 rotation,
					 startex.Size() / 2,
					 item.scale * scale,
					 info.spriteEffects,
					 0);

				Main.playerDrawData.Add(data);

				data.rotation += MathHelper.PiOver2;
				Main.playerDrawData.Add(data);
			}
		}
	}
}