using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
	public class DepthSpiral : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Depth Spiral");
			Tooltip.SetDefault("Shoots out two Depth Bolts that circle around each other");

		}


		public override void SetDefaults()
		{
			item.damage = 58;
			item.melee = true;
			item.width = 34;
			item.height = 40;
			item.useTime = 28;
			item.useAnimation = 28;
			item.useStyle = 1;
			item.knockBack = 4;
			item.value = Terraria.Item.sellPrice(0, 5, 0, 0);
			item.rare = 5;
			item.UseSound = SoundID.Item1;
			item.shoot = mod.ProjectileType("DepthSpiral");
			item.shootSpeed = 7f;
			item.autoReuse = true;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.Next(5) == 0)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 15);
			}
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DepthShard", 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

	}
}
