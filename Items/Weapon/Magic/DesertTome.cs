using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class DesertTome : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Sandstorm");
			Tooltip.SetDefault("Summons a wall of sand to decimate foes.");
		}



        public override void SetDefaults()
        {
            item.damage = 50;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.magic = true;
            item.width = 28;
            item.height = 30;
            item.useTime = 34;
            item.mana = 16;
            item.useAnimation = 34;
            item.useStyle = 5;
            item.knockBack = 11;
            item.value = 80000;
            item.rare = 6;
            item.UseSound = SoundID.Item34;
            item.autoReuse = true;
            item.shootSpeed = 4;
            item.UseSound = SoundID.Item20;
            item.shoot = mod.ProjectileType("SandWall");
        }
                public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.TitaniumBar, 10);
            recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.AdamantiteBar, 10);
            recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
