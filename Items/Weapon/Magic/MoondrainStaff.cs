using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class MoondrainStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moondrain Staff");
			Tooltip.SetDefault("Shoots a clump of lifestealing blood");
		}


		public override void SetDefaults()
		{
			item.damage = 47;
			item.magic = true;
			item.mana = 8;
			item.width = 40;
			item.height = 40;
			item.useTime = 32;
			item.useAnimation = 32;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.noMelee = true; 
			item.knockBack = 0;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 0, 90, 0);
            item.rare = 5;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("Moondrainer");
			item.shootSpeed = 8f;
		}
		
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Veinstone", 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
	}
}
