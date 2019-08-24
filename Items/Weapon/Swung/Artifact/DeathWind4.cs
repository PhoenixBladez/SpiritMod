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
	public class DeathWind4 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Death Wind");
			Tooltip.SetDefault("You are the Artifact Keeper of the Undead\nSummons a homing, returning scythe on swing\nEach scythe takes up one minion slot\nAttacks inflict 'Death Wreathe'\nRight-click to cause nearby enemies to take far more damage\n6 second cooldown\nAttacks may ignore enemy defense\nAttacks may grant you 'Soul Reap,' greatly boosting life regeneration\nEnemies affected by the right-click may explode into a cluster of Souls\nEnemies that are killed have a chance to be resurrected as a group of Necromancers\nUp to four Necromancers can exist at once");
		}


		public override void SetDefaults()
		{
			item.damage = 73;
			item.summon = true;
			item.width = 42;
			item.height = 40;
			item.useTime = 13;
			item.useAnimation = 13;
			item.useStyle = 1;
			item.knockBack = 8;
			item.value = Item.sellPrice(0, 11, 0, 50);
			item.shoot = mod.ProjectileType("DeathWind4Proj");
			item.rare = 10;
			item.shootSpeed = 16f;
			item.UseSound = SoundID.Item69;
			item.autoReuse = true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine line = new TooltipLine(mod, "ItemName", "Artifact Weapon");
			line.overrideColor = new Color(100, 0, 230);
			tooltips.Add(line);
			foreach (TooltipLine line2 in tooltips)
			{
				if (line2.mod == "Terraria" && line2.Name == "ItemName")
				{
					line2.overrideColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
					break;
				}
			}
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
			if (Main.rand.Next(5) == 2)
				target.AddBuff(mod.BuffType("DeathWreathe3"), 240);
			if (Main.rand.Next(6) == 1)
				damage = damage + (int)(target.defense);
			if (Main.rand.Next(6) == 2)
				player.AddBuff(mod.BuffType("SoulReap"), 240);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse == 2)
			{

				MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
				modPlayer.shootDelay1 = 360;
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, mod.ProjectileType("SoulNet1"), 0, 0, player.whoAmI);
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
			recipe.AddIngredient(null, "DeathWind3", 1);
			recipe.AddIngredient(null, "RadiantEmblem", 1);
			recipe.AddIngredient(null, "PlanteraBloom", 1);
			recipe.AddIngredient(null, "ApexFeather", 1);
			recipe.AddIngredient(null, "UnrefinedRuneStone", 1);
			recipe.AddIngredient(null, "Catalyst", 1);
			recipe.AddIngredient(null, "PrimordialMagic", 150);
			recipe.AddTile(null, "CreationAltarTile");
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

	}
}
