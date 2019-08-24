using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Gun
{
	public class HellfireShotgun : ModItem

	{
		int charger;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hellfire Shotgun");
			Tooltip.SetDefault(" 'Death Comes...'\nFires powerful hellfire bullets\nEvery 20 bullets, you unleash Death Blossom\nHellfire Bullets have a chance to steal life");
		}

		public override void SetDefaults()
		{
			item.damage = 49;
			item.ranged = true;
			item.width = 65;
			item.height = 21;
			item.useTime = 32;
			item.useAnimation = 32;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 9;
			item.useTurn = true;
			item.value = Item.sellPrice(0, 8, 0, 0);
			item.rare = 8;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("ReaperBlast");
			item.shootSpeed = 15f;
			item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 origVect = new Vector2(speedX, speedY);
			for (int X = 0; X <= 6; X++)
			{
				Vector2 vel;
				if (Main.rand.Next(2) == 1)
					vel = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(200, 3000) / 12));
				else
					vel = origVect.RotatedBy(-System.Math.PI / (Main.rand.Next(200, 3000) / 12));

				Projectile.NewProjectile(position.X, position.Y, vel.X, vel.Y, mod.ProjectileType("ReaperBlast"), damage / 4 * 3, knockBack, player.whoAmI);
			}

			Main.PlaySound(SoundLoader.customSoundType, player.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Reaper1"));
			Vector2 vector82 = -Main.player[Main.myPlayer].Center + Main.MouseWorld;
			float ai = Main.rand.Next(100);
			Vector2 vector83 = Vector2.Normalize(vector82) * item.shootSpeed;
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("ReaperBlast"), damage, knockBack, player.whoAmI);

			charger++;
			if (charger == 20)
			{
				for (int I = 0; I < 1; I++)
				{
					int dust = Dust.NewDust(player.position + player.velocity, player.width + 40, player.height + 40, 109, player.velocity.X * 0.5f, player.velocity.Y * 0.5f);
					Main.PlaySound(SoundLoader.customSoundType, player.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Reaper2"));
					float spread = 10f * 0.0174f;
					double startAngle = Math.Atan2(speedX, speedY) - spread / 2;
					double deltaAngle = spread / 8f;
					double offsetAngle;
					int i;
					for (i = 0; i < 20; i++)
					{
						offsetAngle = (startAngle + deltaAngle * (i + i * i) / 2f) + 32f * i;
						Projectile.NewProjectile(position.X, position.Y, (float)(Math.Sin(offsetAngle) * 3f), (float)(Math.Cos(offsetAngle) * 3f), item.shoot, damage, knockBack, player.whoAmI);
						Projectile.NewProjectile(position.X, position.Y, (float)(-Math.Sin(offsetAngle) * 3f), (float)(-Math.Cos(offsetAngle) * 3f), item.shoot, damage, knockBack, player.whoAmI);
					}
				}
			}
			else if (charger == 21)
			{
				for (int I = 0; I < 1; I++)
				{
					int dust = Dust.NewDust(player.position + player.velocity, player.width + 40, player.height + 40, 109, player.velocity.X * 0.5f, player.velocity.Y * 0.5f);
					float spread = 10f * 0.0174f;
					double startAngle = Math.Atan2(speedX, speedY) - spread / 2;
					double deltaAngle = spread / 8f;
					double offsetAngle;
					int i;
					for (i = 0; i < 20; i++)
					{
						offsetAngle = (startAngle + deltaAngle * (i + i * i) / 2f) + 32f * i;
						Projectile.NewProjectile(position.X, position.Y, (float)(Math.Sin(offsetAngle) * 3f), (float)(Math.Cos(offsetAngle) * 3f), item.shoot, damage, knockBack, player.whoAmI);
						Projectile.NewProjectile(position.X, position.Y, (float)(-Math.Sin(offsetAngle) * 3f), (float)(-Math.Cos(offsetAngle) * 3f), item.shoot, damage, knockBack, player.whoAmI);
					}
				}
			}
			else if (charger == 22)
			{
				for (int I = 0; I < 1; I++)
				{
					int dust = Dust.NewDust(player.position + player.velocity, player.width + 40, player.height + 40, 109, player.velocity.X * 0.5f, player.velocity.Y * 0.5f);
					float spread = 10f * 0.0174f;
					double startAngle = Math.Atan2(speedX, speedY) - spread / 2;
					double deltaAngle = spread / 8f;
					double offsetAngle;
					int i;
					for (i = 0; i < 20; i++)
					{
						offsetAngle = (startAngle + deltaAngle * (i + i * i) / 2f) + 32f * i;
						Projectile.NewProjectile(position.X, position.Y, (float)(Math.Sin(offsetAngle) * 3f), (float)(Math.Cos(offsetAngle) * 3f), item.shoot, damage, knockBack, player.whoAmI);
						Projectile.NewProjectile(position.X, position.Y, (float)(-Math.Sin(offsetAngle) * 3f), (float)(-Math.Cos(offsetAngle) * 3f), item.shoot, damage, knockBack, player.whoAmI);
					}
				}

			}
			if (charger >= 23)
				charger = 0;

			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.OnyxBlaster, 1);
			recipe.AddIngredient(null, "SunShard", 2);
			recipe.AddIngredient(null, "FieryEssence", 5);
			recipe.AddIngredient(null, "DuskStone", 6);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

	}
}
