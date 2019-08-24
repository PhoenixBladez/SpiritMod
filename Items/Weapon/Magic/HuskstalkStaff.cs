using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class HuskstalkStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Huskstalk Staff");
			Tooltip.SetDefault("Shoots consecutive leaves");
		}


		public override void SetDefaults()
		{
			item.damage = 15;
			item.magic = true;
			item.mana = 6;
			item.width = 40;
			item.height = 40;
            item.useTime = 8;
            item.useAnimation = 24;
            item.useStyle = 5;
			Item.staff[item.type] = true;
			item.noMelee = true; 
			item.knockBack = 6;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 0, 15, 0);
            item.rare = 1;
			item.UseSound = SoundID.Item20;
			item.autoReuse = false;
            item.shoot = 206;
			item.shootSpeed = 5.5f;
            item.reuseDelay = 30;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "AncientBark", 4);
            recipe.AddIngredient(null, "EnchantedLeaf", 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
