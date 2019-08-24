using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class CrystalKnife : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Knife");
		}


        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.width = 9;
            item.height = 15;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.thrown = true;
            item.channel = true;
            item.noMelee = true;
            item.consumable = true;
            item.maxStack = 999;
            item.shoot = mod.ProjectileType("CrystalKnifeProjectile");
            item.useAnimation = 14;
            item.useTime = 14;
            item.shootSpeed = 9.0f;
            item.damage = 45;
            item.knockBack = 3.5f;
			item.value = Item.sellPrice(0, 0, 1, 50);
            item.crit = 24;
            item.rare = 5;
            item.autoReuse = true;
            item.maxStack = 999;
            item.consumable = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CrystalShard, 1);
            recipe.AddIngredient(ItemID.PearlstoneBlock, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
        }
    }
}