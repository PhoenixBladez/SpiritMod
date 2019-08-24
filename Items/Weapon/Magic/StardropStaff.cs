using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class StardropStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tidal Staff");
			Tooltip.SetDefault("Shoots a splitting ball of water that occasionally inflicts Tidal Ebb, lowering enemy defense and life");
		}


        public override void SetDefaults()
        {
            item.damage = 26;
            item.magic = true;
            item.mana = 8;
            item.width = 36;
            item.height = 36;
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 4;
            item.value = Terraria.Item.sellPrice(0, 0, 40, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item20;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("StardropStaffProj");
            item.shootSpeed = 14f;
        }


        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Coral, 6);
            recipe.AddIngredient(null, "PearlFragment", 14);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
