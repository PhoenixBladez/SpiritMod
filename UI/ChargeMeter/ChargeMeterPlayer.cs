using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using System;

namespace SpiritMod.UI.ChargeMeter
{
    public class ChargeMeterPlayer : ModPlayer
    {
        public struct ChargeMeter
        {
            public bool drawMeter;
            public float charge;
            public string meterTexture;
            public string barTexture;
        }

        public ChargeMeter chargeMeter;

        public override void ResetEffects()
        {
            chargeMeter.drawMeter = false;
            chargeMeter.charge = 0;
            chargeMeter.meterTexture = "SpiritMod/ChargeMeter/ChargeMeter";
            chargeMeter.barTexture = "SpiritMod/ChargeMeter/ChargeBar";
        }
    }

	public class ChargeMeterLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.ArmOverItem);

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (drawInfo.shadow != 0f)
				return;

			ChargeMeterPlayer player = drawInfo.drawPlayer.GetModPlayer<ChargeMeterPlayer>();

			if (player.chargeMeter.drawMeter)
			{
				Player drawPlayer = drawInfo.drawPlayer;
				ChargeMeterPlayer modPlayer = drawPlayer.GetModPlayer<ChargeMeterPlayer>();
				Vector2 Position = drawPlayer.position;
				DrawData drawData = new DrawData();
				SpriteEffects spriteEffects;
				if (drawPlayer.gravDir == 1.0)
				{
					if (drawPlayer.direction == 1)
						spriteEffects = SpriteEffects.None;
					else
						spriteEffects = SpriteEffects.FlipHorizontally;
				}
				else
				{
					if (drawPlayer.direction == 1)
						spriteEffects = SpriteEffects.FlipVertically;
					else
						spriteEffects = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
				}

				Texture2D ChargeMeter = ModContent.Request<Texture2D>(player.chargeMeter.meterTexture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				Texture2D ChargeBar = ModContent.Request<Texture2D>(player.chargeMeter.barTexture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				int num24 = (int)((drawPlayer.miscCounter / 300.0f * MathHelper.TwoPi).ToRotationVector2().Y * 4.0);
				Vector2 position = new Vector2((int)(Position.X - Main.screenPosition.X - (drawPlayer.bodyFrame.Width / 2) + (drawPlayer.width / 2)), (int)(Position.Y - Main.screenPosition.Y + drawPlayer.height - drawPlayer.bodyFrame.Height + 4.0)) + drawPlayer.bodyPosition + new Vector2(drawPlayer.bodyFrame.Width / 2, drawPlayer.bodyFrame.Height / 2) + new Vector2((-drawPlayer.direction), (num24 - 60));
				drawData = new DrawData(ChargeMeter, position, new Microsoft.Xna.Framework.Rectangle?(), Color.White, drawPlayer.bodyRotation, ChargeMeter.Size() / 2f, 1f, spriteEffects, 0);
				drawInfo.DrawDataCache.Add(drawData);

				Rectangle rect = new Rectangle(0, 0, (int)(ChargeBar.Width * player.chargeMeter.charge), ChargeBar.Height);
				drawData = new DrawData(ChargeBar, position + new Vector2(6, 0), rect, Color.White, drawPlayer.bodyRotation, ChargeMeter.Size() / 2f, 1f, SpriteEffects.None, 0);
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}
}
