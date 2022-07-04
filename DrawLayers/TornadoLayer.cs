using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.GameContent;
using System;

namespace SpiritMod.DrawLayers
{
	internal class TornadoLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.FrontAccFront);

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (drawInfo.shadow != 0f)
				return;

			Mod mod = ModLoader.GetMod("SpiritMod");
			Player player = drawInfo.drawPlayer;
			if (player.active && !player.outOfRange)
			{

				float halfheight = player.height / 2; //tornado drawcode i made a while ago, based on what vanilla does(draws a ton of this texture with different rotations)
				float density = 20f;
				for (float i = 0; i < (int)density; i++)
				{
					Color color = new Color(212, 192, 100);
					color.A /= 2;
					float lerpamount = (Math.Abs(density / 2 - i) > ((density / 2) * 0.6f)) ? Math.Abs(density / 2 - i) / (density / 2) : 0f; //if too low or too high up, start making it transparent
					color = Color.Lerp(color, Color.Transparent, lerpamount);
					Texture2D texture = ModContent.Request<Texture2D>("SpiritMod/Textures/TornadoExtra", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
					Vector2 offset = Vector2.SmoothStep(player.Center + Vector2.UnitY * halfheight, player.Center - Vector2.UnitY * halfheight, i / density);
					float scale = MathHelper.Lerp(0.6f, 1f, i / density);
					DrawData drawdata = new DrawData(texture, offset - Main.screenPosition,
						new Rectangle(0, 0, texture.Width, texture.Height),
						Lighting.GetColor((int)player.Center.X / 16, (int)player.Center.Y / 16).MultiplyRGBA(color) * 0.5f * ((float)MyPlayer.ChitinDashTicks / 20),
						i / 6f - Main.GlobalTimeWrappedHourly * 7f,
						texture.Size() / 2,
						scale,
						SpriteEffects.None,
						0);

					drawInfo.DrawDataCache.Add(drawdata);
				}
			}
		}
	}
}
