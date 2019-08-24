using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Weapon.Bow
{
    public class RuneBow : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hex Bow");
			Tooltip.SetDefault("Occasionally shoots out a cluster of runes");
		}


		private Vector2 newVect;
        public override void SetDefaults()
        {
            item.width = 18;
			item.damage = 25;
			
            item.height = 40;
            item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
            item.rare = 5;

            item.crit = 4;
            item.knockBack = 3;

            item.useStyle = 5;
            item.useTime = 21;
            item.useAnimation = 21;

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
			if (Main.rand.Next(4) == 1)
			{
				Vector2 origVect = new Vector2(speedX, speedY);
			for (int X = 0; X <= 4; X++)
			{
				if (Main.rand.Next(2) == 1)
				{
					newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(92, 1800) / 10));
				}
				else
				{
					newVect = origVect.RotatedBy(-System.Math.PI / (Main.rand.Next(92, 1800) / 10));
				}
			int proj = Projectile.NewProjectile(position.X, position.Y, newVect.X, newVect.Y, mod.ProjectileType("Rune"), damage, knockBack, player.whoAmI);
				Projectile newProj1 = Main.projectile[proj];
				newProj1.timeLeft = 120;
				
			}
			}
            return true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Rune", 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
