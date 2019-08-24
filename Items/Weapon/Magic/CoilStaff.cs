using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
namespace SpiritMod.Items.Weapon.Magic
{
	public class CoilStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coil Mine Staff");
			Tooltip.SetDefault("Shoots out a detonating coil mine \n Only two mines can exist at once \n Occasionally burns foes");
		}


		public override void SetDefaults()
		{
			item.width = 48;
			item.height = 50;			
			item.value = Item.buyPrice(0, 0, 30, 0);
			item.rare = 2;
			item.damage = 21;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.useTime = 24;
			item.useAnimation = 24;
			item.mana = 11;
            item.knockBack = 3;
			item.magic = true;
			item.noMelee = true;
			item.shoot = mod.ProjectileType("CoilMine");
			item.shootSpeed = 10f;
		}

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(null, "TechDrive", 15);
            modRecipe.AddTile(TileID.Anvils);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();
        }
    }
}
