using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
    public class Marrowrend : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Marrowrend");
			Tooltip.SetDefault("Occasionally shoots out spurts of Cursed Flame");
		}


		int charger;
        public override void SetDefaults()
        {
            item.damage = 54;
            item.noMelee = true;
            item.ranged = true;
            item.width = 26;
            item.height = 62;
            item.useTime = 14;
			item.useAnimation = 14;
            item.useStyle = 5;
            item.shoot = 3;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 3;
            item.value = Terraria.Item.sellPrice(0, 4, 0, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item5;          
            item.autoReuse = true;
            item.shootSpeed = 10.5f;
            item.crit = 8;
        }
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			charger++;
			if (charger >= 5)
			{
				for (int I = 0; I < 5; I++)
			{
			Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float) Main.rand.Next(-230, 230) / 100), speedY + ((float) Main.rand.Next(-230, 230) / 100), mod.ProjectileType("CursedBone"), damage, knockBack, player.whoAmI, 0f, 0f);
			}
			charger = 0;
			}
			return true;
		}
		 public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null,"CursedFire", 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}