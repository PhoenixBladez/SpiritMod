using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class TitaniumKnife : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Titanium Knife");
		}


        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.width = 30;
            item.height = 50;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.thrown = true;
            item.channel = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("TitaniumKnifeProjectile");
            item.useAnimation = 25;
            item.consumable = true;
            item.maxStack = 999;
            item.useTime = 25;
            item.shootSpeed = 7.5f;
            item.damage = 48;
            item.knockBack = 2f;
			item.value = Terraria.Item.sellPrice(0, 0, 4, 0);
            item.rare = 4;
            item.autoReuse = true;
            item.maxStack = 999;
            item.consumable = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.TitaniumBar, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 20);
            recipe.AddRecipe();
        }
    }
}
