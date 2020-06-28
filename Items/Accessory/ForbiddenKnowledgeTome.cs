
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class ForbiddenKnowledgeTome : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tome of Forbidden Knowledge");
			Tooltip.SetDefault("Killing enemies releases homing spectral skulls\nThe skulls deal the same damage type as the weapon that killed the enemy");

		}


		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 30;
			item.value = Item.buyPrice(0, 1, 0, 0);
			item.rare = 3;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().forbiddenTome = true;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				ModContent.GetTexture("SpiritMod/Items/Accessory/ForbiddenKnowledgeTome_Glow"),
				new Vector2
				(
					item.position.X - Main.screenPosition.X + item.width * 0.5f,
					item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
				),
				new Rectangle(0, 0, texture.Width, texture.Height),
				Color.White,
				rotation,
				texture.Size() * 0.5f,
				scale,
				SpriteEffects.None,
				0f
			);
		}
	}
}
