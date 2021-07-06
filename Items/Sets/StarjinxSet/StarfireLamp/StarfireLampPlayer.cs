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
	public class StarfireLampPlayer : ModPlayer, IDrawAdditive
	{
		public int TwinkleTime { get; set; }
		public const int MaxTwinkleTime = 12;

		public NPC LampTargetNPC { get; set; }
		public int LampTargetTime { get; set; }
		public const int MaxTargetTime = 480;

		private int additiveCall = -1;

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
			if(additiveCall != -1)
			{
				AdditiveCallManager.RemoveCall(additiveCall);
				additiveCall = -1;
			}

			if(player.HeldItem.type == ModContent.ItemType<StarfireLamp>())
			{
				additiveCall = AdditiveCallManager.ManualAppend(this);
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

		public void AdditiveCall(SpriteBatch sB)
		{
			if (LampTargetNPC == null || LampTargetTime == 0)
				return;

			Texture2D star = mod.GetTexture("Effects/Masks/Star");
			Texture2D beam = mod.GetTexture("Effects/Mining_Helmet");

			float Timer = (float)Math.Sin(Main.GameUpdateCount / 24f);
			Vector2 drawPosition = player.MountedCenter - new Vector2(0, 60 + (Timer * 4)) + new Vector2(0, 6);

			Color color = Color.Lerp(SpiritMod.StarjinxColor(Main.GameUpdateCount / 12f) , Color.White, 0.5f);

			float halfTargetTime = MaxTargetTime / 2f;
			float starOpacity = Math.Min(2 * ((halfTargetTime - Math.Abs(halfTargetTime - LampTargetTime)) / halfTargetTime), 0.7f);
			float beamOpacity = Math.Max((15f - Math.Abs(15f - (MaxTargetTime - LampTargetTime))) / 15f, starOpacity);

			Vector2 dist = LampTargetNPC.Center - drawPosition;

			sB.Draw(star, LampTargetNPC.Center - Main.screenPosition, null, color * starOpacity, Main.GameUpdateCount / 24f, star.Size() / 2, 1f, SpriteEffects.None, 0);
			sB.Draw(star, LampTargetNPC.Center - Main.screenPosition, null, color * starOpacity * 0.8f, -Main.GameUpdateCount / 24f, star.Size() / 2, 0.8f, SpriteEffects.None, 0);

			for (int i = -1; i <= 1; i++)
			{
				float rot = dist.ToRotation();
				Vector2 offset = (i == 0) ? Vector2.Zero : Vector2.UnitX.RotatedBy(rot + MathHelper.PiOver4 * i) * 4;
				float opacity = (i == 0) ? 1f : 0.5f;
				sB.Draw(beam, drawPosition + offset - Main.screenPosition, null, color * beamOpacity * opacity, rot + MathHelper.PiOver2, new Vector2(beam.Width / 2f, beam.Height * 0.58f), new Vector2(1, dist.Length() / (beam.Height / 2f)), SpriteEffects.None, 0);
			}
		}
	}
}