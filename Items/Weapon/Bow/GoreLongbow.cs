using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
    public class GoreLongbow : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gore Longbow");
			Tooltip.SetDefault("Arrows shot turn into Ichor arrows");
		}



        public override void SetDefaults()
        {
            item.damage = 34;
            item.noMelee = true;
            item.ranged = true;
            item.width = 24;
            item.height = 46;
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = 5;
            item.shoot = 3;
            item.useAmmo = AmmoID.Arrow;
			item.crit = 7;
            item.knockBack = 1;
            item.value = Terraria.Item.sellPrice(0, 0, 70, 0);
            item.rare = 4;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shootSpeed = 8f;
        }
            public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Terraria.Projectile.NewProjectile(position.X, position.Y, speedX, speedY, 278, damage, knockBack, player.whoAmI, 0f, 0f);
			Terraria.Projectile.NewProjectile(position.X, position.Y, speedX + (Main.rand.Next(200) / 100), speedY + (Main.rand.Next(200) / 100), 278, damage, knockBack, player.whoAmI, 0f, 0f);
            return false;

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "FleshClump", 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
