using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class GreatAwakening : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Great Awakening");
			Tooltip.SetDefault("'An almagamation of the waking and sleeping'\nShoots out the flames of dawn surrounded by energies of dusk \nInflicts a multitude of debuffs \nEnemies hit are illuminanted by Holy Light");
		}


		public override void SetDefaults()
		{
			item.damage = 52;
			item.magic = true;
			item.mana = 11;
			item.width = 50;
			item.height = 50;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.noMelee = true; 
			item.knockBack = 3;
            item.value = Terraria.Item.sellPrice(0, 8, 0, 0);
            item.rare = 7;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("FieryAura");
			item.shootSpeed = 26f;
		}
		
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DuskStone", 12);
            recipe.AddIngredient(null, "InfernalAppendage", 12);
            recipe.AddIngredient(null, "IlluminatedCrystal", 12);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
	}
}
