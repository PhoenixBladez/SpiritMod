using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class HallowedStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hallowed Staff");
			Tooltip.SetDefault("Summons a Horde of Magical Swords!");
		}


        public override void SetDefaults()
        {
            item.damage = 51;
            item.magic = true;
            item.mana = 9;
            item.width = 48;
            item.height = 48;
            item.useTime = 20;
            item.useAnimation = 54;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 1;
            item.value = 20000;
            item.rare = 5;
            item.UseSound = SoundID.Item20;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("HallowedSword");
            item.shootSpeed = 9f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HallowedBar, 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
