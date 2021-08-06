using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Bullet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.LaunchersMisc.Liberty
{
	public class LibertyItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Liberty");
			Tooltip.SetDefault("Charges up to fire a rocket");
		}

		public override void SetDefaults()
		{
			item.damage = 24;
			item.ranged = true;
			item.Size = new Vector2(94, 30);
			item.useTime = 40;
			item.useAnimation = 40;
			item.useTurn = false;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 4;
			item.channel = true;
			item.value = Item.buyPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.Orange;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<LibertyProjHeld>();
			item.shootSpeed = 12f;
			item.useAmmo = AmmoID.Rocket;
			item.noUseGraphic = true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(player.MountedCenter, new Vector2(speedX, speedY), ModContent.ProjectileType<LibertyProjHeld>(), damage, knockBack, player.whoAmI);
			return false;
		}

		public override bool ConsumeAmmo(Player player) => player.ownedProjectileCounts[item.shoot] > 0;
	}
}