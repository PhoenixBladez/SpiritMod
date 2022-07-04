using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Items.Sets.GamblerChestLoot.RegalCane
{
	[AutoloadEquip(EquipType.HandsOn)]
	public class RegalCane : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Cane");

			//ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
			Item.vanity = true;
		}
		
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			if (Main.rand.Next(20) == 0)
			{
				int index3 = Dust.NewDust(new Vector2(Item.position.X, Item.position.Y), Item.width, Item.height, DustID.GoldFlame, 0.0f, 0.0f, 150, new Color(), 0.3f);
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
