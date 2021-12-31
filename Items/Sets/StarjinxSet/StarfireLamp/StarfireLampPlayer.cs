using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Utilities;
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
		public float AttackingTime { get; set; }

		public override void Initialize()
		{
			TwinkleTime = 0;
			LampTargetNPC = null;
			LampTargetTime = 0;
			AttackingTime = 0;
		}

		private bool HoldingLamp => player.HeldItem.type == ModContent.ItemType<StarfireLamp>();
		public override void PreUpdate()
		{
			TwinkleTime = Math.Max(TwinkleTime - 1, 0);
			LampTargetTime = Math.Max(LampTargetTime - 1, 0);

			if (HoldingLamp)
				AttackingTime = Math.Max(AttackingTime - 0.035f, 0);

			else
				AttackingTime = 0;

			//Check if the target npc is still active and targettable, if not, set to null and reset target time
			if (LampTargetNPC != null)
				LampTargetNPC = (LampTargetNPC.active && LampTargetNPC.CanBeChasedBy(player)) ? LampTargetNPC : null;

			else
				LampTargetTime = 0;

			if (LampTargetTime == 0)
				LampTargetNPC = null;
		}

		public override void PostUpdate()
		{
			if (HoldingLamp)
				player.GetModPlayer<ExtraDrawOnPlayer>().DrawDict.Add(delegate (SpriteBatch sB) { DrawBeam(sB); }, ExtraDrawOnPlayer.DrawType.Additive);
		}

		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			if(player.HeldItem.type == ModContent.ItemType<StarfireLamp>())
			{
				layers.Insert(layers.FindIndex(x => x.Name == "HeldItem" && x.mod == "Terraria"), new PlayerLayer(mod.Name, "StarfireLampHeld",
					delegate (PlayerDrawInfo info) { DrawItem(mod.GetTexture("Items/Sets/StarjinxSet/StarfireLamp/StarfireLantern_held"),
						mod.GetTexture("Items/Sets/StarjinxSet/StarfireLamp/StarfireLantern_heldGlow"), info); }));
			}
		}

		private Vector2 GetHoldPosition => player.MountedCenter + new Vector2(player.direction * player.width / 2, 0);
		private Vector2 Origin => new Vector2(38, 18);

		public void DrawItem(Texture2D texture, Texture2D glow, PlayerDrawInfo info)
		{
			Item item = info.drawPlayer.HeldItem;
			if (info.shadow != 0f || info.drawPlayer.frozen || info.drawPlayer.dead)
				return;

			int numFrames = 6;
			int currentFrame = (int)((Main.GlobalTime * 11) % numFrames);
			Rectangle drawRect = texture.Bounds;
			drawRect.Height /= numFrames;
			drawRect.Y = currentFrame * drawRect.Height;

			Vector2 drawPosition = GetHoldPosition - Main.screenPosition;

			Main.playerDrawData.Add(new DrawData(
				texture,
				drawPosition,
				drawRect,
				Lighting.GetColor((drawPosition + Main.screenPosition).ToTileCoordinates().X, (drawPosition + Main.screenPosition).ToTileCoordinates().Y),
				0,
				Origin,
				item.scale,
				info.spriteEffects,
				0));

			if(TwinkleTime > 0)
			{
				float rotation = player.direction * (TwinkleTime / (float)MaxTwinkleTime) * MathHelper.Pi;
				float opacity = 0.8f * (MaxTwinkleTime - Math.Abs((MaxTwinkleTime / 2) - TwinkleTime)) / (MaxTwinkleTime / 2);
				Vector2 scale = new Vector2(1, 0.6f) * ((MaxTwinkleTime / 2) - Math.Abs((MaxTwinkleTime/2) - TwinkleTime)) / (MaxTwinkleTime / 2);
				Texture2D startex = Main.extraTexture[89];
				Vector2 twinkleOffset = new Vector2(0, 26);

				DrawData data = new DrawData(
					 startex,
					 drawPosition + twinkleOffset,
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

		public void DrawBeam(SpriteBatch sB)
		{
			if (player.frozen || player.dead)
				return;

			Texture2D glow = mod.GetTexture("Items/Sets/StarjinxSet/StarfireLamp/StarfireLantern_heldGlow");
			Vector2 drawPosition = GetHoldPosition - Main.screenPosition;
			float Timer = 0.15f;
			Item item = player.HeldItem;

			int numFrames = 6;
			int currentFrame = (int)((Main.GlobalTime * 11) % numFrames);
			Rectangle drawRect = glow.Bounds;
			drawRect.Height /= numFrames;
			drawRect.Y = currentFrame * drawRect.Height;

			for (int i = 0; i < 8; i++)
			{
				Vector2 pulseoffset = Vector2.UnitX.RotatedBy(MathHelper.TwoPi * i / 8f) * Math.Max(Timer, 0) * 8;
				sB.Draw(
				 glow,
				 drawPosition + pulseoffset,
				 drawRect,
				 Color.White * Math.Max(1 - Timer, 0) * AttackingTime * 0.5f,
				 0,
				 Origin,
				 item.scale,
				 player.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
				 0);
			}

			sB.Draw(
				 glow,
				 drawPosition,
				 drawRect,
				 Color.White * AttackingTime,
				 0,
				 Origin,
				 item.scale,
				 player.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
				 0);

			if (LampTargetNPC == null || LampTargetTime == 0)
				return;

			//Texture2D star = mod.GetTexture("Effects/Masks/Star");
			Texture2D beam = mod.GetTexture("Effects/Mining_Helmet");


			Color color = Color.Lerp(SpiritMod.StarjinxColor(Main.GameUpdateCount / 12f) , Color.White, 0.5f);

			float halfTargetTime = MaxTargetTime / 2f;
			float starOpacity = Math.Min(2 * ((halfTargetTime - Math.Abs(halfTargetTime - LampTargetTime)) / halfTargetTime), 0.7f);
			float beamOpacity = Math.Max((15f - Math.Abs(15f - (MaxTargetTime - LampTargetTime))) / 15f, starOpacity);
			Vector2 beamOffset = new Vector2(0, 26);
			Vector2 dist = LampTargetNPC.Center - (drawPosition + beamOffset + Main.screenPosition);

			//sB.Draw(star, LampTargetNPC.Center - Main.screenPosition, null, color * starOpacity, Main.GameUpdateCount / 24f, star.Size() / 2, 1f, SpriteEffects.None, 0);
			//sB.Draw(star, LampTargetNPC.Center - Main.screenPosition, null, color * starOpacity * 0.8f, -Main.GameUpdateCount / 24f, star.Size() / 2, 0.8f, SpriteEffects.None, 0);

			for (int i = -1; i <= 1; i++)
			{
				float rot = dist.ToRotation();
				Vector2 offset = (i == 0) ? Vector2.Zero : Vector2.UnitX.RotatedBy(rot + MathHelper.PiOver4 * i) * 4;
				float opacity = (i == 0) ? 1f : 0.5f;
				sB.Draw(beam, drawPosition + offset + beamOffset, null, color * beamOpacity * opacity, rot + MathHelper.PiOver2, new Vector2(beam.Width / 2f, beam.Height * 0.58f), new Vector2(1, (dist.Length() * 1.2f) / (beam.Height / 2f)), SpriteEffects.None, 0);
			}
		}
	}
}