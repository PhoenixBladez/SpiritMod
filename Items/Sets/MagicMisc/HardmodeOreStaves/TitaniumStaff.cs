using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MagicMisc.HardmodeOreStaves
{
	public class TitaniumStaff : ModItem
	{
		int counter;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Titanium Staff");
			Tooltip.SetDefault("Surrounds the player in blades\nHold left-click to release a barrage of blades");

			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 11;
			Item.channel = true;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 32;
			Item.useAnimation = 32;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 6;
			Item.useTurn = true;
			Item.value = Item.sellPrice(0, 1, 50, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item88;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<TitaniumStaffProj>();
			Item.shootSpeed = 30f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)  => false;
		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] > 0;

		public override void HoldItem(Player player)
		{
			counter++;
			int spikes = player.GetSpiritPlayer().shadowCount;
			if (counter >= 85 && !player.channel && spikes <= 3) {
				counter = 0;
				int num = 4 - spikes;
				for (int I = 0; I < num; I++) {
					int DegreeDifference = 360 / num;
					Projectile.NewProjectile((int)player.Center.X + (int)(Math.Sin(I * DegreeDifference) * 80), (int)player.Center.Y + (int)(Math.Sin(I * DegreeDifference) * 80), 0, 0, ModContent.ProjectileType<TitaniumStaffProj>(), Item.damage, Item.knockBack, player.whoAmI, 0, I * DegreeDifference);
					player.GetSpiritPlayer().shadowCount++;
				}
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.TitaniumBar, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
