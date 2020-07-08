using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Projectiles;

namespace SpiritMod.Items.Weapon.Gun
{
	public class TrueShadowShot : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nightbane");
			Tooltip.SetDefault("Shooting an enemy causes shadow bullets to appear");

		}


		public override void SetDefaults()
		{
			item.damage = 43;
			item.ranged = true;
			item.width = 65;
			item.height = 28;
			item.useTime = 16;
			item.useAnimation = 16;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 8;
			item.useTurn = false;
			item.value = Terraria.Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.UseSound = SoundID.Item36;
			item.autoReuse = true;
			item.shoot = ProjectileID.PurificationPowder;
			item.shootSpeed = 11f;
			item.useAmmo = AmmoID.Bullet;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int p = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			Main.projectile[p].GetGlobalProjectile<SpiritGlobalProjectile>().shotFromNightbane = true;
			return false;
		}
		public override void HoldItem(Player player)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if(modPlayer.shootDelay2 == 1) {
				Main.PlaySound(SoundID.MaxMana, -1, -1, 1, 1f, 0.0f);
				for(int index1 = 0; index1 < 5; ++index1) {
					int index2 = Dust.NewDust(player.position, player.width, player.height, 75, 0.0f, 0.0f, (int)byte.MaxValue, new Color(), (float)Main.rand.Next(20, 26) * 0.1f);
					Main.dust[index2].noLight = false;
					Main.dust[index2].noGravity = true;
					Main.dust[index2].velocity *= 0.5f;
				}
			}
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<ShadowShot>(), 1);
			recipe.AddIngredient(ModContent.ItemType<BrokenParts>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

	}
}
