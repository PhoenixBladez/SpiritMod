﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Material;

namespace SpiritMod.Items.Sets.StarjinxSet.QuasarGauntlet
{
	public class QuasarGauntlet : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quasar Glove");
			Tooltip.SetDefault("Launches a ball of cosmic energy\nGrows in size and power with every enemy it hits \nRight click to recall it");
		}

		public override void SetDefaults()
		{
			Item.shoot = ModContent.ProjectileType<QuasarOrb>();
			Item.shootSpeed = 16f;
			Item.damage = 70;
			Item.knockBack = 3.3f;
			Item.DamageType = DamageClass.Magic;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.channel = true;
			Item.width = 26;
			Item.height = 26;
			Item.mana = 20;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Pink;
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse != 2)
				return player.ownedProjectileCounts[Item.shoot] == 0;
			else
			{
				bool foundproj = false;
				for (int i = 0; i < Main.maxProjectiles; ++i)
				{
					Projectile proj = Main.projectile[i];
					if (proj.active && proj.owner == Main.myPlayer && proj.type == ModContent.ProjectileType<QuasarOrb>())
					{
						if (proj.ModProjectile is QuasarOrb orb)
						{
							if (orb.AiState == QuasarOrb.STATE_SLOWDOWN)
							{
								orb.AiState = QuasarOrb.STATE_ANTICIPATION;
								orb.Timer = 0;
								proj.netUpdate = true;
								foundproj = true;
							}
						}
					}
				}

				return foundproj;
			}
		}

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			if (player.altFunctionUse == 2)
				mult = 0;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			if (player.altFunctionUse == 2)
				return false;

			SoundEngine.PlaySound(SoundID.Item117, player.Center);
			int proj = Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockback, player.whoAmI);

			for (int i = 0; i < 3; i++)
			{
				int p = Projectile.NewProjectile(position, new Vector2(speedX, speedY).RotatedByRandom(MathHelper.PiOver2) * Main.rand.NextFloat(0.15f, 0.22f), ModContent.ProjectileType<QuasarOrbiter>(), damage, knockback, player.whoAmI, proj, Main.rand.Next(3));
				Main.projectile[p].netUpdate = true;
			}
			return false;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Starjinx>(), 16);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}