using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
namespace SpiritMod.Items.Weapon.Magic
{
	public class FloranStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Staff");
			Tooltip.SetDefault("Calls three guarding energies that surround the player before dissipating \n Vines occasionally ensnare the foes, reducing their movement speed");
		}


		public override void SetDefaults()
		{
			item.width = 48;
			item.height = 50;			
			item.value = Item.buyPrice(0, 2, 0, 0);
			item.rare = 2;
			item.damage = 17;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.useTime = 15;
			item.useAnimation = 45;
			item.mana = 6;
            item.knockBack = 3;
            item.crit = 8;
			item.magic = true;
			item.noMelee = true;
			item.shoot = mod.ProjectileType("FloranOrb");
			item.shootSpeed = 10f;
		}

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(null, "FloranBar", 15);
            modRecipe.AddTile(TileID.Anvils);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();
        }
    }
}
