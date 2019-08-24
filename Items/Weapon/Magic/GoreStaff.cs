using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class GoreStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gore Staff");
			Tooltip.SetDefault("Shoots a cluster of blood that splits into Ichor streams");
		}


		public override void SetDefaults()
		{
			item.damage = 37;
			item.magic = true;
			item.mana = 16;
			item.width = 40;
			item.height = 40;
			item.useTime = 29;
			item.useAnimation = 29;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.noMelee = true; 
			item.knockBack = 6;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 0, 80, 0);
            item.rare = 5;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("IchorBomb");
			item.shootSpeed = 6f;
		}
		
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "FleshClump", 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
	}
}
