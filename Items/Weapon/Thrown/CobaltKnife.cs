using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class CobaltKnife : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobalt Cutter");
		}


        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.width = 14;
            item.height = 50;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.thrown = true;
            item.channel = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("CobaltKnifeProjectile");
            item.useAnimation = 20;
            item.consumable = true;
            item.maxStack = 999;
            item.useTime = 20;
            item.shootSpeed = 10.0f;
            item.damage = 33;
            item.knockBack = 2.7f;
			item.value = Item.sellPrice(0, 0, 1, 0);
            item.rare = 4;
            item.autoReuse = true;
            item.maxStack = 999;
            item.consumable = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CobaltBar, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 20);
            recipe.AddRecipe();
        }
    }
}
