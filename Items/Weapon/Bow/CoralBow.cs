using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using SpiritMod.Projectiles;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
    public class CoralBow : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tidal Bow");
			Tooltip.SetDefault("Arrows shot inflict Tidal Ebb, which lowers enemy defense and life");
		}



        public override void SetDefaults()
        {
            item.damage = 25;
            item.noMelee = true;
            item.ranged = true;
            item.width = 18;
            item.height = 46;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = 5;
            item.shoot = 1;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 4;
            item.value = Terraria.Item.sellPrice(0, 0, 30, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.useTurn = false;
            item.shootSpeed = 11.2f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int p = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
            Main.projectile[p].GetGlobalProjectile<SpiritGlobalProjectile>(mod).shotFromCoralBow = true;
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Coral, 3);
            recipe.AddIngredient(null, "PearlFragment", 12);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}