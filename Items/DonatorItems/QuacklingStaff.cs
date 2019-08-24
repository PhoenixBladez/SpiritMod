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
            Tooltip.SetDefault("~ Donator Item ~\nSummons a friendly duck to launch aqua bolts at enemies.");

        }


		public override void SetDefaults()
		{
            item.width = 26;
            item.height = 28;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 3;
            item.mana = 8;
            item.damage = 19;
            item.knockBack = 1;
            item.useStyle = 1;
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
        }
    }
}