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
            item.shootSpeed = 16f;
			item.damage = 70;
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
				for (int i = 0; i < Main.maxProjectiles; ++i)
				{
					Projectile proj = Main.projectile[i];
					if (proj.active && proj.owner == Main.myPlayer && proj.type == ModContent.ProjectileType<QuasarOrb>())
					{
						if(proj.modProjectile is QuasarOrb orb)
						{
							if(orb.AiState == QuasarOrb.STATE_SLOWDOWN)
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

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse == 2)
				return false;

			Main.PlaySound(SoundID.Item117, player.Center);
			int proj = Projectile.NewProjectile(position, new Vector2(speedX,speedY), type, damage, knockBack, player.whoAmI);

			for (int i = 0; i < 3; i++)
			{
				int p = Projectile.NewProjectile(position, new Vector2(speedX, speedY).RotatedByRandom(MathHelper.PiOver2) * Main.rand.NextFloat(0.15f, 0.22f), ModContent.ProjectileType<QuasarOrbiter>(), damage, knockBack, player.whoAmI, proj, Main.rand.Next(3));
				Main.projectile[p].netUpdate = true;
			}
			return false;
		}

		public override void HoldItem(Player player)
		{

		}

		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Starjinx>(), 16);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
