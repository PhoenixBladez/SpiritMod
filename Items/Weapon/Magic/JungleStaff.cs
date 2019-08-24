using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class JungleStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thornball Staff");
			Tooltip.SetDefault("Shoots a bouncing poisonous spore");
		}


        public override void SetDefaults()
        {
            item.damage = 19;
            item.magic = true;
            item.mana = 13;
            item.width = 38;
            item.height = 38;
            item.useTime = 24;
            item.useAnimation = 24;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 4;
            item.value = 20000;
            item.rare = 3;
            item.UseSound = SoundID.Item20;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("ThornSpike");
            item.shootSpeed = 13f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.JungleSpores, 12);
            recipe.AddIngredient(ItemID.Stinger, 3);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
