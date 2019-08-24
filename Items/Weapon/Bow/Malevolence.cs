using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Weapon.Bow
{
    public class Malevolence : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Malevolence");
			Tooltip.SetDefault("Transforms arrows into ghastly arrows\nShoots 2 arrows at once");
		}


		private Vector2 newVect;
        public override void SetDefaults()
        {
            item.width = 42;
			item.damage = 41;
			
            item.height = 40;

            item.value = Item.sellPrice(0, 6, 0, 0);
            item.rare = 6;

            item.crit = 4;
            item.knockBack = 3;

            item.useStyle = 5;
            item.useTime = 17;
            item.useAnimation = 17;

            item.useAmmo = AmmoID.Arrow;

            item.ranged = true;
            item.noMelee = true;
            item.autoReuse = true;

            item.shoot = 3;
            item.shootSpeed = 9;

            item.UseSound = SoundID.Item5;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            //Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ProjectileID.IchorArrow, damage, knockBack, player.whoAmI, 0f, 0f);
			
				Vector2 origVect = new Vector2(speedX, speedY);
			for (int X = 0; X <= 1; X++)
			{
				if (Main.rand.Next(2) == 1)
				{
					newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(112, 1800) / 10));
				}
				else
				{
					newVect = origVect.RotatedBy(-System.Math.PI / (Main.rand.Next(112, 1800) / 10));
				}
			int proj = Projectile.NewProjectile(position.X, position.Y, newVect.X, newVect.Y, mod.ProjectileType("HellArrow"), damage, knockBack, player.whoAmI);
				Projectile newProj1 = Main.projectile[proj];
				newProj1.timeLeft = 120;
				
			}
            return false;
        }
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "FieryEssence", 14);
            recipe.AddTile(null,"EssenceDistorter");
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }

    }
}