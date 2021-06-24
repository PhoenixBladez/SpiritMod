using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Items.Sets.GamblerChestLoot.GildedMustache
{
	[AutoloadEquip(EquipType.Face)]
	public class GildedMustache : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Gilded Mustache");
		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 30;
			item.value = Item.sellPrice(0, 0, 0, 0);
			item.rare = ItemRarityID.Blue;
			item.accessory = true;
			item.vanity = true;
		}
		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
			=> drawHair = true;
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			if (Main.rand.Next(20) == 0)
			{
				int index3 = Dust.NewDust(new Vector2(item.position.X, item.position.Y), item.width, item.height, 228, 0.0f, 0.0f, 150, new Color(), 0.3f);
				Main.dust[index3].fadeIn = 0.75f;
				Dust dust = Main.dust[index3];
				Vector2 vector2_2 = dust.velocity * 0.1f;
				dust.velocity = vector2_2;
				Main.dust[index3].noLight = true;
				Main.dust[index3].noGravity = true;
			}
		}
	}
}
