using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Material;

namespace SpiritMod.Items.Sets.StarjinxSet.QuasarGauntlet
{
	public class QuasarGauntlet : ModItem
	{
		public override void SetDefaults()
		{
            item.shoot = ModContent.ProjectileType<QuasarOrb>();
            item.shootSpeed = 14f;
			item.damage = 90;
			item.knockBack = 3.3f;
			item.magic = true;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 30;
			item.useTime = 30;
			item.channel = true;
			item.width = 26;
			item.height = 26;
			item.mana = 20;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.value = Item.sellPrice(gold: 1);
			item.rare = ItemRarityID.Pink;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quasar Glove");
			Tooltip.SetDefault("Launches a ball of cosmic energy\nGrows in size and power with every enemy it hits \nRight click to recall it");
		}	

		public override bool AltFunctionUse(Player player)
        {
            return true;
        }

		public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse != 2)
            {
				return player.ownedProjectileCounts[item.shoot] == 0;
            }
			else
			{
				bool foundproj = false;
				for (int i = 0; i < 1000; ++i)
				{
					if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == ModContent.ProjectileType<QuasarOrb>() && Main.projectile[i].ai[0] == 0)
					{
						Main.projectile[i].ai[0] = 1;
						foundproj = true;
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

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse == 2)
				return false;

			Main.PlaySound(SoundID.Item117, player.Center);
			int proj = Projectile.NewProjectile(position, new Vector2(speedX,speedY), type, damage, knockBack, player.whoAmI);

			for (int i = 0; i < 6; i++)
			{
				int p = Projectile.NewProjectile(position, Main.rand.NextVector2CircularEdge(2, 2), ModContent.ProjectileType<QuasarOrbiter>(), damage, knockBack, player.whoAmI, proj, Main.rand.Next(3));
				Main.projectile[p].netUpdate = true;
			}
			return false;
		}

		public override void HoldItem(Player player)
		{
			Vector2 handOffset = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
			if (player.direction != 1)
				handOffset.X = player.bodyFrame.Width - handOffset.X;
			if (player.gravDir != 1.0)
				handOffset.Y = player.bodyFrame.Height - handOffset.Y;

			Vector2 spawnPos = player.RotatedRelativePoint(player.position + handOffset - new Vector2(player.bodyFrame.Width - player.width, player.bodyFrame.Height - 42) / 2f, true) - player.velocity;

			for (int index = 0; index < 2; ++index)
			{
				Dust dust = Main.dust[Dust.NewDust(player.Center, 0, 0, DustID.Rainbow, player.direction * 2, 0.0f, 150, SpiritMod.StarjinxColor(Main.GlobalTime * 2), 1.3f)];
				dust.position = spawnPos;
				dust.velocity *= 0.0f;
				dust.noGravity = true;
				dust.fadeIn = 1f;
				dust.velocity += player.velocity;
				dust.scale *= 0.8f;

				if (Main.rand.Next(2) == 0)
				{
					dust.position += Utils.RandomVector2(Main.rand, -4f, 4f);
					dust.scale += Main.rand.NextFloat();
					if (Main.rand.Next(2) == 0)
						dust.customData = player;
				}
			}
		}

		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Starjinx>(), 16);
			recipe.AddIngredient(ItemID.FallenStar, 4);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
