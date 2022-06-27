using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.RlyehianDrops
{
	public class TentacleChain : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brine Barrage");
		}

		public override void SetDefaults()
		{
			Item.width = 44;
			Item.height = 44;
			Item.rare = ItemRarityID.Orange;
			Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 16;
			Item.useTime = 16;
			Item.knockBack = 0;
			Item.value = Item.sellPrice(0, 1, 20, 0);
			Item.damage = 23;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<TentacleChainProj>();
			Item.shootSpeed = 11f;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.DamageType = DamageClass.Melee;
			Item.channel = true;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			// How far out the inaccuracy of the shot chain can be.
			float radius = 20f;
			// Sets ai[1] to the following value to determine the firing direction.
			// The smaller the value of NextFloat(), the more accurate the shot will be. The larger, the less accurate. This changes depending on your radius.
			// NextBool().ToDirectionInt() will have a 50% chance to make it negative instead of positive.
			// The Solar Eruption uses this calculation: Main.rand.NextFloat(0f, 0.5f) * Main.rand.NextBool().ToDirectionInt() * MathHelper.ToRadians(45f);
			float direction = Main.rand.NextFloat(0.25f, 1f) * Main.rand.NextBool().ToDirectionInt() * MathHelper.ToRadians(radius);
			Projectile projectile = Projectile.NewProjectileDirect(source , player.RotatedRelativePoint(player.MountedCenter), velocity, type, damage, knockback, player.whoAmI, 0f, direction);
			// Extra logic for the chain to adjust to item stats, unlike the Solar Eruption.
			if (projectile.ModProjectile is TentacleChainProj modItem)
			{
				modItem.firingSpeed = Item.shootSpeed * 2f;
				modItem.firingAnimation = Item.useAnimation * 3;
				modItem.firingTime = Item.useTime * 3;
			}
			return false;
		}
	}
}