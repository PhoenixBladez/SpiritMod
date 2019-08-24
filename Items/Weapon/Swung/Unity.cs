using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Swung
{
	public class Unity : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twilight Dawn");
			Tooltip.SetDefault("Shoots out a fiery sphere of energy that explodes on contact with foes\nUpon hitting an enemy or tile, the energy splits into multiple shadow embers\nRarely combusts hit foes, dealing more damage as you continue hitting foes\nDusk embers inflict Shadowflame\nHit enemies are illuminated by Holy Light");
		}


		public override void SetDefaults()
		{
			item.damage = 64;
			item.useTime = 26;
			item.useAnimation = 26;
			item.melee = true;
			item.width = 60;
			item.height = 64;
			item.useStyle = 1;
			item.knockBack = 4;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = 7;
			item.shootSpeed = 12;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useTurn = true;
			item.shoot = mod.ProjectileType("TwilightBlaze");
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for (int I = 0; I < 1; I++)
			{
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("TwilightBlaze"), 50, knockBack, player.whoAmI);
			}
			return false;
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(4) == 0)
			{
				target.AddBuff(mod.BuffType("StackingFireBuff"), 180, false);
				target.AddBuff(mod.BuffType("HolyLight"), 180, false);
				target.AddBuff(BuffID.ShadowFlame, 180);
			}
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.Next(5) == 0)
			{
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 6);
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Shadowflame);
			}
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DuskStone", 12);
			recipe.AddIngredient(null, "InfernalAppendage", 12);
			recipe.AddIngredient(null, "IlluminatedCrystal", 12);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 15);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}
}
