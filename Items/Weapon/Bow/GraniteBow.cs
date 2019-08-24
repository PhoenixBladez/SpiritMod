using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using SpiritMod.Projectiles;

namespace SpiritMod.Items.Weapon.Bow
{
    public class GraniteBow : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granite Bow");
			Tooltip.SetDefault("Occasionally shoots out a powerful energy fragment that inflicts Energy Flux, causing enemies to move spasmodically!");
		}



        public override void SetDefaults()
        {
            item.damage = 21;
            item.noMelee = true;
            item.ranged = true;
            item.width = 20;
            item.height = 38;
            item.useTime = 26;
            item.useAnimation = 26;
            item.useStyle = 5;
            item.shoot = 3;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 4;
            item.value = Terraria.Item.sellPrice(0, 8, 0, 0);
            item.rare = 2;
            item.crit = 10;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shootSpeed = 13f;

        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			if (Main.rand.Next(6) == 2)
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("GraniteSpike"), 15, knockBack, player.whoAmI, 0f, 0f);
            return true; 
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "GraniteChunk", 16);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}