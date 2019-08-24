using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
    public class QuicksilverBow : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quicksilver Bow");
			Tooltip.SetDefault("Shoots two Quicksilver Arrows which release bouncing quicksilver globs upon hitting a foe");
		}



        public override void SetDefaults()
        {
            item.damage = 51;
            item.noMelee = true;
            item.ranged = true;
            item.width = 44;
            item.height = 62;
            item.useTime = 18;
            item.useAnimation = 18;
            item.useStyle = 5;
            item.shoot = 3;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 3;
            item.value = Terraria.Item.sellPrice(0, 10, 0, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shootSpeed = 14.8f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position.X -= player.width * .25f;
            Vector2 offset = new Vector2(speedY, -speedX);
            offset.Normalize();
            offset *= 10;
            int proj = mod.ProjectileType("QuicksilverArrow");
            Projectile.NewProjectile(position.X + offset.X, position.Y + offset.Y, speedX, speedY, proj, damage, knockBack, player.whoAmI);
            Projectile.NewProjectile(position.X - offset.X, position.Y - offset.Y, speedX, speedY, proj, damage, knockBack, player.whoAmI);
            return false;

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Material", 14);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
