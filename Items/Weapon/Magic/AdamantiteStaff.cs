using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class AdamantiteStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Adamantite Staff");
			Tooltip.SetDefault("Shoots out a cluster of energy that splits into different directions");
		}


		public override void SetDefaults()
		{
			item.damage = 51;
			item.magic = true;
			item.mana = 11;
			item.width = 41;
			item.height = 41;
			item.useTime = 28;
			item.useAnimation = 28;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.noMelee = true; 
			item.knockBack = 1;
            item.value = Terraria.Item.sellPrice(0, 1, 40, 0);
            item.rare = 4;
			item.UseSound = SoundID.Item8;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("AdamantiteStaffProj");
			item.shootSpeed = 10f;
		}
		
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.AdamantiteBar, 12);
             recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
	}
}
