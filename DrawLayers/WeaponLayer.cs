using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.GameContent;

namespace SpiritMod.DrawLayers
{
	internal class WeaponLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.ArmOverItem);

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			MyPlayer myPlayer = drawPlayer.GetModPlayer<MyPlayer>();
			if (drawPlayer.active && !drawPlayer.outOfRange && false)
			{
				Texture2D weaponTexture = TextureAssets.Item[drawPlayer.inventory[drawPlayer.selectedItem].type].Value;
				Vector2 vector8 = new Vector2(weaponTexture.Width / 2, (weaponTexture.Height / 4) / 2);
				Vector2 vector9 = new Vector2(8, 0);

				vector8.Y = vector9.Y;

				Vector2 vector = drawPlayer.itemLocation;
				vector.Y += weaponTexture.Height * 0.5F;

				int num84 = (int)vector9.X;
				Vector2 origin2 = new Vector2(-num84, (weaponTexture.Height / 4) / 2);
				if (drawPlayer.direction == -1)
					origin2 = new Vector2(weaponTexture.Width + num84, (weaponTexture.Height / 4) / 2);

				SpriteEffects effect = drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

				var drawPos = new Vector2((int)(vector.X - Main.screenPosition.X + vector8.X), (int)(vector.Y - Main.screenPosition.Y + vector8.Y));
				var source = new Rectangle(0, (weaponTexture.Height / 4) * myPlayer.hexBowAnimationFrame, weaponTexture.Width, weaponTexture.Height / 4);
				var drawData = new DrawData(weaponTexture, drawPos, source, drawPlayer.HeldItem.GetAlpha(Color.White), drawPlayer.itemRotation, origin2, drawPlayer.HeldItem.scale, effect, 0);
				drawInfo.DrawDataCache.Add(drawData);

				if (drawPlayer.inventory[drawPlayer.selectedItem].color != default)
				{
					drawData = new DrawData(weaponTexture, drawPos, source, drawPlayer.HeldItem.GetColor(Color.White), drawPlayer.itemRotation, origin2, drawPlayer.HeldItem.scale, effect, 0);
					drawInfo.DrawDataCache.Add(drawData);
				}
			}
		}
	}
}
