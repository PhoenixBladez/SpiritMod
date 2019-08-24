using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class GraniteStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granite Staff");
			Tooltip.SetDefault("Shoots a cluster of Granite Spikes! Critical hits inflict Energy Flux, causing enemies to move spasmodically");
		}


		public override void SetDefaults()
		{
			item.damage = 20;
			item.magic = true;
			item.mana = 12;
			item.width = 44;
			item.height = 44;
			item.useTime = 24;
			item.useAnimation = 18;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 3;
			item.useTurn = false;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = 2;
			item.crit = 10;
			item.UseSound = SoundID.Item9;
			item.shoot = mod.ProjectileType("GraniteSpike");
			item.shootSpeed = 8f;
			item.autoReuse = false;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float spread = 60 * 0.0174f; //change 60 to degrees you want
			float baseSpeed = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
			double startAngle = Math.Atan2(speedX, speedY) - spread / 2;
			double deltaAngle = spread / 5; //change 5 to what you wan the number to be
			for (int i = 0; i < 3; i++)//change 5 to what you wan the number to be
			{
				double offsetAngle = startAngle + deltaAngle * i;
				Projectile.NewProjectile(position.X, position.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), item.shoot, damage, knockBack, player.whoAmI);
			}
			return false;
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
