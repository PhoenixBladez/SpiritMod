using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod
{
	public static class GlowmaskUtils
	{
		public static void DrawNPCGlowMask(SpriteBatch spriteBatch, NPC npc, Texture2D texture, Color? color = null)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(
				texture,
				npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY),
				npc.frame,
				color ?? Color.White,
				npc.rotation,
				npc.frame.Size() / 2,
				npc.scale,
				effects,
				0
			);
		}
		public static void DrawExtras(SpriteBatch spriteBatch, NPC npc, Texture2D texture)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(
				texture,
				npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY),
				npc.frame,
				new Color(200, 200, 200),
				npc.velocity.X * .1f,
				npc.frame.Size() / 2,
				npc.scale,
				effects,
				0
			);
		}
		public static void DrawArmorGlowMask(EquipType type, Texture2D texture, PlayerDrawSet info)
		{
			switch (type) {
				case EquipType.Head: {
						DrawData drawData = new DrawData(texture, new Vector2((int)(info.Position.X - Main.screenPosition.X) + ((info.drawPlayer.width - info.drawPlayer.bodyFrame.Width) / 2), (int)(info.Position.Y - Main.screenPosition.Y) + info.drawPlayer.height - info.drawPlayer.bodyFrame.Height + 4) + info.drawPlayer.headPosition + info.headOrigin, info.drawPlayer.bodyFrame, info.headGlowMaskColor, info.drawPlayer.headRotation, info.headOrigin, 1f, info.playerEffect, 0) {
							shader = info.headArmorShader
						};
						info.DrawDataCache.Add(drawData);
					}
					return;

				case EquipType.Body: {
						Rectangle bodyFrame = info.drawPlayer.bodyFrame;
						int num123 = 0;

						bodyFrame.X += num123;
						bodyFrame.Width -= num123;

						if (info.drawPlayer.direction == -1) {
							num123 = 0;
						}

						if (!info.drawPlayer.invis) {
							DrawData drawData = new DrawData(texture, new Vector2((int)(info.Position.X - Main.screenPosition.X - (info.drawPlayer.bodyFrame.Width / 2) + (info.drawPlayer.width / 2) + num123), ((int)(info.Position.Y - Main.screenPosition.Y + info.drawPlayer.height - info.drawPlayer.bodyFrame.Height + 4))) + info.drawPlayer.bodyPosition + new Vector2(info.drawPlayer.bodyFrame.Width / 2, info.drawPlayer.bodyFrame.Height / 2), bodyFrame, info.bodyGlowMaskColor, info.drawPlayer.bodyRotation, info.bodyOrigin, 1f, info.playerEffect, 0) {
								shader = info.bodyArmorShader
							};
							info.DrawDataCache.Add(drawData);
						}
					}
					return;

				case EquipType.Legs: {
						if (info.drawPlayer.shoe != 15 || info.drawPlayer.wearsRobe) {
							if (!info.drawPlayer.invis) {
								DrawData drawData = new DrawData(texture, new Vector2((int)(info.Position.X - Main.screenPosition.X - (info.drawPlayer.legFrame.Width / 2) + (info.drawPlayer.width / 2)), (int)(info.Position.Y - Main.screenPosition.Y + info.drawPlayer.height - info.drawPlayer.legFrame.Height + 4)) + info.drawPlayer.legPosition + info.legOrigin, info.drawPlayer.legFrame, info.legGlowMaskColor, info.drawPlayer.legRotation, info.legOrigin, 1f, info.playerEffect, 0) {
									shader = info.legArmorShader
								};
								info.DrawDataCache.Add(drawData);
							}
						}
					}
					return;
			}
		}

		public static void DrawItemGlowMask(Texture2D texture, PlayerDrawSet info)
		{
			Item item = info.drawPlayer.HeldItem;
			if (info.shadow != 0f || info.drawPlayer.frozen || ((info.drawPlayer.itemAnimation <= 0 || item.useStyle == 0) && (item.holdStyle <= 0 || info.drawPlayer.pulley)) || info.drawPlayer.dead || item.noUseGraphic || (info.drawPlayer.wet && item.noWet)) {
				return;
			}

			Vector2 offset = Vector2.Zero;
			Vector2 origin = Vector2.Zero;
			float rotOffset = 0;

			if (item.useStyle == ItemUseStyleID.Shoot) {
				if (Item.staff[item.type]) {
					rotOffset = 0.785f * info.drawPlayer.direction;
					if (info.drawPlayer.gravDir == -1f) {
						rotOffset -= 1.57f * info.drawPlayer.direction;
					}

					origin = new Vector2(texture.Width * 0.5f * (1 - info.drawPlayer.direction), (info.drawPlayer.gravDir == -1f) ? 0 : texture.Height);

					int oldOriginX = -(int)origin.X;
					ItemLoader.HoldoutOrigin(info.drawPlayer, ref origin);
					offset = new Vector2(origin.X + oldOriginX, 0);
				}
				else {
					offset = new Vector2(10, texture.Height / 2);
					ItemLoader.HoldoutOffset(info.drawPlayer.gravDir, item.type, ref offset);
					origin = new Vector2(-offset.X, texture.Height / 2);
					if (info.drawPlayer.direction == -1) {
						origin.X = texture.Width + offset.X;
					}

					offset = new Vector2(texture.Width / 2, offset.Y);
				}
			}
			else {
				origin = new Vector2(texture.Width * 0.5f * (1 - info.drawPlayer.direction), (info.drawPlayer.gravDir == -1f) ? 0 : texture.Height);
			}

			info.DrawDataCache.Add(new DrawData(
				texture,
				info.ItemLocation - Main.screenPosition + offset,
				texture.Bounds,
				Color.White * ((255f - item.alpha) / 255f),
				info.drawPlayer.itemRotation + rotOffset,
				origin,
				item.scale,
				info.playerEffect,
				0
			));
		}

		public static void DrawItemGlowMaskWorld(SpriteBatch spriteBatch, Item item, Texture2D texture, float rotation, float scale)
		{
			Main.spriteBatch.Draw(
				texture,
				new Vector2(item.position.X - Main.screenPosition.X + item.width / 2, item.position.Y - Main.screenPosition.Y + item.height - (texture.Height / 2) + 2f),
				new Rectangle(0, 0, texture.Width, texture.Height),
				Color.White * ((255f - item.alpha) / 255f),
				rotation,
				new Vector2(texture.Width / 2, texture.Height / 2),
				scale,
				SpriteEffects.None,
				0f
			);
		}
	}
}

