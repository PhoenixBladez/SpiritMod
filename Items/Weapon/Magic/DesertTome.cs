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
			DisplayName.SetDefault("Khamsin");
			Tooltip.SetDefault("Releases harsh desert winds upon enemies");
		}



        public override void SetDefaults()
        {
            item.damage = 37;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.magic = true;
            item.width = 28;
            item.height = 30;
            item.useTime = 21;
            item.mana = 10;
            item.useAnimation = 21;
            item.useStyle = 5;
            item.knockBack = 3;
            item.value = 50000;
            item.rare = 4;
            item.UseSound = SoundID.Item34;
            item.autoReuse = true;
            item.shootSpeed = 10;
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
