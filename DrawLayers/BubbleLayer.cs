using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.GameContent;

namespace SpiritMod.DrawLayers
{
	internal class BubbleLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.ArmOverItem);

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (drawInfo.shadow != 0f)
				return;

			Player drawPlayer = drawInfo.drawPlayer;
			if (drawPlayer.active && !drawPlayer.outOfRange)
			{
				Texture2D texture = ModContent.Request<Texture2D>("Effects/PlayerVisuals/BubbleShield_Visual", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);

				Vector2 drawPos = drawPlayer.position + new Vector2(drawPlayer.width * 0.5f, drawPlayer.height * 0.5f);
				drawPos.X = (int)drawPos.X;
				drawPos.Y = (int)drawPos.Y;

				DrawData drawData = new DrawData(texture, drawPos - Main.screenPosition, new Rectangle?(), Color.White * 0.75f, 0, origin, 1, SpriteEffects.None, 0);
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}
}
