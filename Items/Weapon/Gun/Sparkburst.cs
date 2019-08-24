using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Gun
{
	public class Sparkburst : ModItem
	{
		private Vector2 newVect;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sparkburst");
			Tooltip.SetDefault("Shoots out a burst of lingering flames");
		}


		public override void SetDefaults()
		{
			item.damage = 17;
			item.magic = true;
			item.width = 34;
			item.height = 22;
			item.mana = 8;
			item.useTime = 35;
			item.useAnimation = 35;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 0;
			item.useTurn = false;
			item.value = Terraria.Item.sellPrice(0, 0, 40, 0);
			item.rare = 3;
			item.UseSound = SoundID.Item36;
			item.autoReuse = false;
			item.shoot = 10;
			item.shootSpeed = 6f;
			//    item.useAmmo = AmmoID.Bullet;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 origVect = new Vector2(speedX, speedY);
			for (int X = 0; X <= 3; X++)
			{
				if (Main.rand.Next(2) == 1)
				{
					newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(72, 1800) / 10));
				}
				else
				{
					newVect = origVect.RotatedBy(-System.Math.PI / (Main.rand.Next(72, 1800) / 10));
				}
				Projectile.NewProjectile(position.X, position.Y, newVect.X, newVect.Y, mod.ProjectileType("FlameTrail"), damage, knockBack, player.whoAmI);
			}
			return false;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.WandofSparking, 1);
			recipe.AddIngredient(null, "EnchantedLeaf", 3);
			recipe.AddIngredient(null, "FloranBar", 6);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

	}
}