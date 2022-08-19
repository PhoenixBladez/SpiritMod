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
			if (drawPlayer.active && !drawPlayer.outOfRange && false)
			{
				Texture2D weaponTexture = TextureAssets.Item[drawPlayer.inventory[drawPlayer.selectedItem].type].Value;
				Vector2 offset = new Vector2(weaponTexture.Width / 2, 0);
				Vector2 itemLocation = drawPlayer.itemLocation + new Vector2(0, weaponTexture.Height * 0.5f);

				Vector2 origin2 = new Vector2(-8, (weaponTexture.Height / 4) / 2);
				if (drawPlayer.direction == -1)
					origin2 = new Vector2(weaponTexture.Width + 8, (weaponTexture.Height / 4) / 2);

				SpriteEffects effect = drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

				var drawPos = new Vector2((int)(itemLocation.X + offset.X), (int)(itemLocation.Y + offset.Y)) - Main.screenPosition;
				var source = new Rectangle(0, weaponTexture.Height / 4, weaponTexture.Width, weaponTexture.Height / 4);
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
