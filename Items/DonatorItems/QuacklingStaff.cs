using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.DonatorItems
{
	public class QuacklingStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quackling Staff");
            Tooltip.SetDefault("Summons a friendly duck to launch aqua bolts at enemies");

        }


		public override void SetDefaults()
		{
            item.width = 26;
            item.height = 28;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 3;
            item.mana = 9;
            item.damage = 21;
            item.knockBack = 1;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 30;
            item.useAnimation = 30;
            item.summon = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("QuacklingMinion");
            item.buffType = mod.BuffType("QuacklingBuff");
            item.buffTime = 3600;
            item.UseSound = SoundID.Item44;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(2123, 1);
            recipe.AddIngredient(320, 10);
			recipe.AddIngredient(165, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();

            ModRecipe recipe1 = new ModRecipe(mod);
            recipe1.AddIngredient(2122, 1);
            recipe1.AddIngredient(320, 10);
            recipe1.AddIngredient(165, 1);
            recipe1.AddTile(TileID.Anvils);
            recipe1.SetResult(this, 1);
            recipe1.AddRecipe();
        }
    }
}