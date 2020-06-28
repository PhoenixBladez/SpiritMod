using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Bullet.Crimbine;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Gun
{
	public class Crimbine : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Harvester");
			Tooltip.SetDefault("Converts bullets into bones\nRight-click to shoot a slow-moving bloody amalgam\nShooting the bloody amalgam creates an explosion of organs with different effects\n5 second cooldown");
		}


		public override void SetDefaults()
		{
			item.damage = 14;
			item.ranged = true;
			item.width = 58;
			item.height = 32;
			item.useTime = 9;
			item.useAnimation = 9;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 0;
			item.useTurn = false;
			item.shoot = ModContent.ProjectileType<CrimbineBone>();
			item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.LightRed;
			item.shootSpeed = 10f;
			item.autoReuse = true;
			item.useAmmo = AmmoID.Bullet;
			item.crit = 6;
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool CanUseItem(Player player)
		{
			if(player.altFunctionUse == 2) {

				MyPlayer modPlayer = player.GetSpiritPlayer();
				if(modPlayer.shootDelay2 == 0)
					return true;
				return false;
			} else {
				return true;
			}
		}
		public override void HoldItem(Player player)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if(modPlayer.shootDelay2 == 1) {
				Main.PlaySound(SoundID.MaxMana, -1, -1, 1, 1f, 0.0f);
				for(int index1 = 0; index1 < 5; ++index1) {
					int index2 = Dust.NewDust(player.position, player.width, player.height, 5, 0.0f, 0.0f, (int)byte.MaxValue, new Color(), (float)Main.rand.Next(20, 26) * 0.1f);
					Main.dust[index2].noLight = false;
					Main.dust[index2].noGravity = true;
					Main.dust[index2].velocity *= 0.5f;
				}
			}
		}
		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
			if(Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) {
				position += muzzleOffset;
			}
			if(player.altFunctionUse == 2) {
				Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 95));
				MyPlayer modPlayer = player.GetSpiritPlayer();
				modPlayer.shootDelay2 = 300;
				type = ModContent.ProjectileType<CrimbineAmalgam>();
				speedX /= 4;
				speedY /= 4;
			} else {
				Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 11));
				item.shootSpeed = 10f;
				float spread = 8 * 0.0174f;//45 degrees converted to radians
				float baseSpeed = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
				double baseAngle = Math.Atan2(speedX, speedY);
				double randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
				speedX = baseSpeed * (float)Math.Sin(randomAngle);
				speedY = baseSpeed * (float)Math.Cos(randomAngle);
				type = ModContent.ProjectileType<CrimbineBone>();
			}
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Boomstick);
			recipe.AddIngredient(ItemID.TheUndertaker);
			recipe.AddIngredient(ItemID.PhoenixBlaster, 1);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}
	}
}