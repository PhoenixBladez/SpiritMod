using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Summon.Dragon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon
{
	public class JadeStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Staff of the Jade Dragon");
			Tooltip.SetDefault("Summons two revolving ethereal dragons");
		}

		public override void SetDefaults()
		{
			item.damage = 23;
			item.magic = true;
			item.mana = 36;
			item.width = 44;
			item.height = 48;
			item.useTime = 80;
			item.useAnimation = 80;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 2.25f;
			item.value = 20000;
			item.rare = ItemRarityID.Orange;
			item.UseSound = new Terraria.Audio.LegacySoundStyle(3, 56);
			item.autoReuse = false;
			item.shoot = ModContent.ProjectileType<DragonHeadOne>();
			item.shootSpeed = 6f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			// Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 124));
			int dragonLength = 8;
			int offset = 0;
			if (speedX > 0) {
				offset = -32;
			}
			else {
				offset = 32;
			}

			int latestprojectile = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<DragonHeadOne>(), damage, knockBack, player.whoAmI, speedX, speedY); //bottom
			for (int i = 0; i < dragonLength; ++i) {
				latestprojectile = Projectile.NewProjectile(position.X + (i * offset), position.Y, 0, 0, ModContent.ProjectileType<DragonBodyOne>(), damage, 0, player.whoAmI, 0, latestprojectile);
			}
			latestprojectile = Projectile.NewProjectile(position.X + (dragonLength * offset), position.Y, 0, 0, ModContent.ProjectileType<DragonTailOne>(), damage, 0, player.whoAmI, 0, latestprojectile);

			latestprojectile = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<DragonHeadTwo>(), damage, knockBack, player.whoAmI, speedX, speedY); //bottom
			for (int j = 0; j < dragonLength; ++j) {
				latestprojectile = Projectile.NewProjectile(position.X + (j * offset), position.Y, 0, 0, ModContent.ProjectileType<DragonBodyTwo>(), damage, 0, player.whoAmI, 0, latestprojectile);
			}
			latestprojectile = Projectile.NewProjectile(position.X + (dragonLength * offset), position.Y, 0, 0, ModContent.ProjectileType<DragonTailTwo>(), damage, 0, player.whoAmI, 0, latestprojectile);
			//Main.projectile[(int)latestprojectile].realLife = projectile.whoAmI;
			return true;
		}
	}
}