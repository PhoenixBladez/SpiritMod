using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace SpiritMod.Items.Weapon.Swung.Artifact
{
	public class DeathWind1 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Death Wind");
			Tooltip.SetDefault("'The Herald of Death itself'\nSummons a returning scythe on swing\nEach scythe takes up one minion slot\nAttacks inflict 'Death Wreathe'\nRight-click to cause nearby enemies to take far more damage\n7 second cooldown");
		}


		public override void SetDefaults()
		{
			item.damage = 25;
			item.summon = true;
			item.width = 42;
			item.height = 40;
			item.useTime = 22;
			item.useAnimation = 22;
			item.useStyle = 1;
			item.knockBack = 5;
			item.value = Item.sellPrice(0, 3, 0, 50);
			item.shoot = mod.ProjectileType("DeathWind1Proj");
			item.rare = 2;
			item.shootSpeed = 11f;
			item.UseSound = SoundID.Item69;
			item.autoReuse = true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine line = new TooltipLine(mod, "ItemName", "Artifact Weapon");
			line.overrideColor = new Color(100, 0, 230);
			tooltips.Add(line);
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (player.altFunctionUse == 2)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 110);
				Main.dust[dust].noGravity = true;
			}
			else
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 110);
				Main.dust[dust].noGravity = true;
			}
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			if (Main.rand.Next(9) == 2)
				target.AddBuff(mod.BuffType("DeathWreathe"), 180);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse == 2)
			{
				MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
				modPlayer.shootDelay1 = 420;
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, mod.ProjectileType("SoulNet"), 0, 0, player.whoAmI);
				return true;
			}
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
				if (modPlayer.shootDelay1 == 0)
					return true;
				return false;
			}

			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "OddKeystone", 1);
			recipe.AddIngredient(null, "RootPod", 1);
			recipe.AddIngredient(null, "GildedIdol", 1);
			recipe.AddIngredient(null, "DemonLens", 1);
			recipe.AddIngredient(null, "PrimordialMagic", 50);
			recipe.AddTile(null, "CreationAltarTile");
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

		}

	}
}
