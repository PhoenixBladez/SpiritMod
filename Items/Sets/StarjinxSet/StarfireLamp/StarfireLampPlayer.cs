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

namespace SpiritMod.Items.Sets.StarjinxSet.StarfireLamp
{
	/// <summary>
	/// Modplayer class handling the custom drawing information relating to Starfire Lantern, and for storing the current NPC targetted by the player with the weapon.
	/// </summary>
	public class StarfireLampPlayer : ModPlayer
	{
		//The location of the held texture, used for drawing and determining bounds of the drawing rectangle
		private static string TexturePath => "Items/Sets/StarjinxSet/StarfireLamp/StarfireLantern_held";
		private string GlowmaskPath => TexturePath + "Glow";

		//How long the twinkling effect on use lasts
		public int TwinkleTime { get; set; }
		public const int MaxTwinkleTime = 12;

		//The npc targetted to be focused by homing shots
		public NPC LampTargetNPC { get; set; }
		public int LampTargetTime { get; set; }
		public const int MaxTargetTime = 480;

		//The opacity of the animated glowmask, set to 1 on weapon use and fades out over time
		public float GlowmaskOpacity { get; set; }

		//Where the origin of the texture is drawn
		private Vector2 GetHoldPosition => Player.MountedCenter + new Vector2(Player.direction * Player.width / 2, 0);
		private Vector2 Origin => new Vector2(38, 18);

		//The amount of frames used for the animation
		private const int NumFrames = 6;

		//The current frame being drawn, currently based on game time as to constantly increase
		private int CurFrame => (int)((Main.GlobalTimeWrappedHourly * 11) % NumFrames);
		private bool HoldingLamp => Player.HeldItem.type == ModContent.ItemType<StarfireLamp>() && false;

		//The rectangle of the texture currently being drawn
		private Rectangle DrawRectangle
		{
			get
			{
				Texture2D texture = Mod.Assets.Request<Texture2D>(TexturePath).Value;
				Rectangle drawRect = texture.Bounds;
				drawRect.Height /= NumFrames;
				drawRect.Y = CurFrame * drawRect.Height;
				return drawRect;
			}
		}

		public override void Initialize()
		{
			TwinkleTime = 0;
			LampTargetNPC = null;
			LampTargetTime = 0;
			GlowmaskOpacity = 0;
		}

		public override void PreUpdate()
		{
			TwinkleTime = Math.Max(TwinkleTime - 1, 0);
			LampTargetTime = Math.Max(LampTargetTime - 1, 0);

			//Fade out over time, instantly if not holding lamp
			if (HoldingLamp)
				GlowmaskOpacity = Math.Max(GlowmaskOpacity - 0.035f, 0);

			else
				GlowmaskOpacity = 0;

			//Check if the target npc is still active and targettable, if not, set to null and reset target time
			if (LampTargetNPC != null)
				LampTargetNPC = (LampTargetNPC.active && LampTargetNPC.CanBeChasedBy(Player)) ? LampTargetNPC : null;

			else
				LampTargetTime = 0;

			if (LampTargetTime == 0)
				LampTargetNPC = null;
		}

		//Add drawing methods to the lists of player layers if the lamp is being held
		public override void PostUpdate()
		{
			if (HoldingLamp)
				Player.GetModPlayer<ExtraDrawOnPlayer>().DrawDict.Add(delegate (SpriteBatch sB) { DrawAdditiveLayer(sB); }, ExtraDrawOnPlayer.DrawType.Additive);
		}
		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			if(HoldingLamp)
				layers.Insert(layers.FindIndex(x => x.Name == "HeldItem" && x.mod == "Terraria"), new PlayerLayer(Mod.Name, "StarfireLampHeld",
					delegate (PlayerDrawInfo info) { DrawItem(info); }));
		}

		//Prevent the texture from being drawn if the player is dead or under other conditions where held items would typically not be drawn
		private bool CanDraw => Player.shadow == 0f && !Player.frozen && !Player.dead;

		public void DrawItem(PlayerDrawInfo info)
		{
			if (!CanDraw)
				return;

			Texture2D texture = Mod.Assets.Request<Texture2D>(TexturePath).Value;
			Vector2 drawPosition = GetHoldPosition - Main.screenPosition;
			Color lightColor = Lighting.GetColor(GetHoldPosition.ToTileCoordinates().X, GetHoldPosition.ToTileCoordinates().Y);

			Main.playerDrawData.Add(new DrawData(texture, drawPosition, DrawRectangle, lightColor, 0, Origin, Player.HeldItem.scale, info.spriteEffects, 0));
			DrawTwinkle(info);
		}

		private void DrawTwinkle(PlayerDrawInfo info)
		{
			if (TwinkleTime > 0)
			{
				float rotation = Player.direction * (TwinkleTime / (float)MaxTwinkleTime) * MathHelper.Pi;
				float opacity = 0.8f * (MaxTwinkleTime - Math.Abs((MaxTwinkleTime / 2) - TwinkleTime)) / (MaxTwinkleTime / 2);
				Vector2 scale = new Vector2(1, 0.6f) * ((MaxTwinkleTime / 2) - Math.Abs((MaxTwinkleTime / 2) - TwinkleTime)) / (MaxTwinkleTime / 2);
				Texture2D startex = Main.extraTexture[89];
				Vector2 twinkleOffset = new Vector2(0, 26);

				DrawData data = new DrawData(
					 startex,
					 GetHoldPosition - Main.screenPosition + twinkleOffset,
					 null,
					 new Color(255, 147, 113) * opacity,
					 rotation,
					 startex.Size() / 2,
					 Player.HeldItem.scale * scale,
					 info.spriteEffects,
					 0);

				Main.playerDrawData.Add(data);

				data.rotation += MathHelper.PiOver2;
				Main.playerDrawData.Add(data);
			}
		}

		public void DrawAdditiveLayer(SpriteBatch sB)
		{
			if (Player.frozen || Player.dead)
				return;

			Texture2D glow = Mod.Assets.Request<Texture2D>(GlowmaskPath).Value;
			Vector2 drawPosition = GetHoldPosition - Main.screenPosition;
			Item item = Player.HeldItem;
			SpriteEffects flip = Player.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			PulseDraw.DrawPulseEffect((float)Math.Asin(-0.6), 8, 12, delegate (Vector2 posOffset, float opacityMod)
			{
				sB.Draw(glow, drawPosition + posOffset, DrawRectangle, Color.White * opacityMod * GlowmaskOpacity * 0.5f, 0, Origin, item.scale, flip, 0);
			});

			sB.Draw(glow, drawPosition, DrawRectangle, Color.White * GlowmaskOpacity, 0, Origin, item.scale, flip, 0);

			DrawBeams(sB);
		}

		private void DrawBeams(SpriteBatch sB)
		{
			if (LampTargetNPC == null || LampTargetTime == 0)
				return;

			Texture2D beam = Mod.Assets.Request<Texture2D>("Effects/Mining_Helmet").Value;


			Color color = Color.Lerp(SpiritMod.StarjinxColor(Main.GameUpdateCount / 12f), Color.White, 0.5f);

			float halfTargetTime = MaxTargetTime / 2f;
			float starOpacity = Math.Min(2 * ((halfTargetTime - Math.Abs(halfTargetTime - LampTargetTime)) / halfTargetTime), 0.7f);
			float baseOpacity = Math.Max((15f - Math.Abs(15f - (MaxTargetTime - LampTargetTime))) / 15f, starOpacity);
			Vector2 position = GetHoldPosition + new Vector2(0, 26);
			Vector2 dist = LampTargetNPC.Center - position;

			for (int i = -1; i <= 1; i++)
			{
				float rot = dist.ToRotation();
				Vector2 offset = (i == 0) ? Vector2.Zero : Vector2.UnitX.RotatedBy(rot + MathHelper.PiOver4 * i) * 4;
				float opacityMod = (i == 0) ? 1f : 0.5f;
				sB.Draw(beam, position - Main.screenPosition + offset, null, color * baseOpacity * opacityMod, rot + MathHelper.PiOver2, new Vector2(beam.Width / 2f, beam.Height * 0.58f), new Vector2(1, (dist.Length() * 1.2f) / (beam.Height / 2f)), SpriteEffects.None, 0);
			}
		}
	}
}