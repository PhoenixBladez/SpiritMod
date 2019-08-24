using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Weapon.Magic
{
	public class TrueHallowedStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Hallowed Staff");
			Tooltip.SetDefault("Shoots out multiple swords with different effects.");
		}


		public override void SetDefaults()
		{
			item.damage = 60;
			item.magic = true;
			item.mana = 12;
			item.width = 70;
			item.height = 70;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 3;
			item.value = 120000;
			item.rare = 8;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("TrueHallowedSword1");
			item.shootSpeed = 16f;
		}

		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(null, "BrokenStaff", 1);
			modRecipe.AddIngredient(null, "HallowedStaff", 1);
			modRecipe.AddTile(134);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			//Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ProjectileID.IchorArrow, damage, knockBack, player.whoAmI, 0f, 0f);
			if (Main.rand.Next(2) == 1)
			{
				Vector2 origVect = new Vector2(speedX, speedY);
				Vector2 newVect = new Vector2();
				for (int X = 0; X <= 0; X++)
				{
					if (Main.rand.Next(2) == 1)
						newVect = origVect.RotatedBy(Math.PI / (Main.rand.Next(140, 150) / 5));
					else
						newVect = origVect.RotatedBy(-Math.PI / (Main.rand.Next(140, 150) / 5));

					int proj = Projectile.NewProjectile(position.X, position.Y, newVect.X, newVect.Y, mod.ProjectileType("TrueHallowedSword2"), damage, knockBack, player.whoAmI);
					Projectile newProj1 = Main.projectile[proj];
					newProj1.timeLeft = 240;

				}
			}

			float spread = 60 * 0.0174f; //change 60 to degrees you want
			float baseSpeed = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
			double startAngle = Math.Atan2(speedX, speedY) - spread / 2;
			double deltaAngle = spread / 0; //change 5 to what you wan the number to be
			for (int i = 0; i < 0; i++)//change 5 to what you wan the number to be
			{
				double offsetAngle = startAngle + deltaAngle * i;
				Projectile.NewProjectile(position.X, position.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), item.shoot, damage, knockBack, player.whoAmI);
			}
			return true;
		}

	}
}
