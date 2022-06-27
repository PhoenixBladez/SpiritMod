using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Bullet;
using Terraria;
using Terraria.DataStructures;
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
			Item.damage = 24;
			Item.DamageType = DamageClass.Ranged;
			Item.Size = new Vector2(94, 30);
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useTurn = false;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 4;
			Item.channel = true;
			Item.value = Item.buyPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<LibertyProjHeld>();
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Rocket;
			Item.noUseGraphic = true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Projectile.NewProjectile(source, player.MountedCenter, velocity, ModContent.ProjectileType<LibertyProjHeld>(), damage, knockback, player.whoAmI);
			return false;
		}

		public override bool CanConsumeAmmo(Item item, Player player) => player.ownedProjectileCounts[Item.shoot] > 0;
	}
}