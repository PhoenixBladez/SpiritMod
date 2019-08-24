using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class NightSkyStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nova's Spark");
			Tooltip.SetDefault("Shoots out a fast laser of starry energy\nEvery fifth strike on foes summons a more powerful homing beam of stars\n That beam rains down homing bolts from the sky");
		}


		int charger;
		private Vector2 newVect;
		public override void SetDefaults()
		{
			item.damage = 39;
			item.magic = true;
			item.mana = 7;
			item.width = 58;
			item.height = 58;
			item.useTime = 28;
			item.useAnimation = 28;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 2.5f;
			item.useTurn = false;
			item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
			item.rare = 4;
			item.UseSound = SoundID.Item72;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("NovaBeam1");
			item.shootSpeed = 8f;
		}

		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			charger++;
			if (charger >= 5)
			{
				Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX, speedY, mod.ProjectileType("NovaBeam2"), damage / 2 * 3, knockBack, player.whoAmI, 0f, 0f);
				charger = 0;
				return false;
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "BreathOfTheZephyr", 1);
			recipe.AddIngredient(null, "Desolate", 1);
			recipe.AddIngredient(null, "HowlingScepter", 1);
			recipe.AddIngredient(null, "GraniteStaff", 1);
			recipe.AddIngredient(null, "SteamParts", 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
