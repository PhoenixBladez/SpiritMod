using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
    public class AdamantiteKnife : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Adamantite Piercer");
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
            item.shoot = mod.ProjectileType("AdamantiteKnifeProjectile");
            item.useAnimation = 25;
            item.consumable = true;
            item.maxStack = 999;
            item.useTime = 25;
            item.shootSpeed = 8.5f;
            item.damage = 45;
            item.knockBack = 1.5f;
			item.value = Item.sellPrice(0, 0, 1, 50);
            item.rare = 4;
            item.autoReuse = true;
            item.maxStack = 999;
            item.consumable = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.AdamantiteBar, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 20);
            recipe.AddRecipe();
        }
    }
}
