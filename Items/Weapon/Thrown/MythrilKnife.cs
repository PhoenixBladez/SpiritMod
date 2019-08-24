using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class MythrilKnife : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mythril Machete");
		}


        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.width = 22;
            item.height = 22;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.thrown = true;
            item.channel = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("MythrilKnifeProjectile");
            item.useAnimation = 29;
            item.consumable = true;
            item.maxStack = 999;
            item.useTime = 29;
            item.shootSpeed = 8.0f;
            item.damage = 38;
            item.knockBack = 3f;
			item.value = Terraria.Item.sellPrice(0, 10, 0, 0);
            item.rare = 4;
            item.autoReuse = true;
            item.maxStack = 999;
            item.consumable = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MythrilBar, 1);
            recipe.AddTile(134);
            recipe.SetResult(this, 20);
            recipe.AddRecipe();
        }
    }
}
