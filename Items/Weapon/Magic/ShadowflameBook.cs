using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class ShadowflameBook : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skeletal Shadow");
			Tooltip.SetDefault("Shoots out homing, shadowflame skulls");
		}


		public override void SetDefaults()
		{
			item.damage = 43;
			item.magic = true;
			item.mana = 8;
			item.width = 30;
			item.height = 28;
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.noMelee = true; 
			item.knockBack = 5;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 4;
			item.UseSound = SoundID.Item8;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("ShadowSkull");
			item.shootSpeed = 8f;
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BookofSkulls, 1);
            recipe.AddIngredient(ItemID.ClothierVoodooDoll, 1);
            recipe.AddIngredient(ItemID.SoulofNight, 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}