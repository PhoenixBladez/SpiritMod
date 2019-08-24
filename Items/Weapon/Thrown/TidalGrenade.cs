using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class TidalGrenade : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tidal Grenade");
			Tooltip.SetDefault("Explodes into bolts of tidal energy");
		}


        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.width = 22;
            item.height = 22;
			item.autoReuse = true;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.thrown = true;
            item.channel = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("TidalGrenade");
            item.useAnimation = 30;
            item.consumable = true;
            item.maxStack = 999;
            item.useTime = 30;
            item.shootSpeed = 10.0f;
            item.damage = 21;
            item.knockBack = 7f;
			item.value = Terraria.Item.sellPrice(0, 0, 0, 60);
            item.rare = 3;
            item.autoReuse = false;
            item.maxStack = 999;
            item.consumable = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Coral, 3);
            recipe.AddIngredient(null, "PearlFragment", 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 33);
            recipe.AddRecipe();
        }

    }
}
