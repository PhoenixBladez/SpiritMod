using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using SpiritMod.Projectiles;

namespace SpiritMod.Items.Weapon.Bow
{
    public class FrostSpine : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Spine");
			Tooltip.SetDefault("Occasionally shoots out a frost bolt");
		}



        public override void SetDefaults()
        {
            item.damage = 9;
            item.noMelee = true;
            item.ranged = true;
            item.width = 24;
            item.height = 38;
            item.useTime = 29;
            item.useAnimation = 29;
            item.useStyle = 5;
            item.shoot = 3;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 3;
            item.value = Terraria.Item.sellPrice(0, 0, 10, 0);
            item.rare = 1;
            item.UseSound = SoundID.Item5;
            item.autoReuse = false;
            item.shootSpeed = 7.5f;

        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (Main.rand.Next(6) == 1)
            {
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 8));
                Projectile.NewProjectile(position.X, position.Y, speedX * 1.2f, speedY * 1.2f, mod.ProjectileType("FrostSpine"), damage, knockBack, player.whoAmI, 0f, 0f);
            }
            return true; 
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "FrigidFragment", 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}