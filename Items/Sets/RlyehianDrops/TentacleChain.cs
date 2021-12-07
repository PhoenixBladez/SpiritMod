using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Flail;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.RlyehianDrops
{
	public class TentacleChain : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brine Barrage");
			// Tooltip.SetDefault("Plugs into tiles, changing the chain into a shocking livewire");

		}


		public override void SetDefaults()
		{
			item.width = 44;
			item.height = 44;
			item.rare = ItemRarityID.Orange;
			item.noMelee = true;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 16;
			item.useTime = 16;
			item.knockBack = 0;
			item.value = Item.sellPrice(0, 1, 20, 0);
			item.damage = 23;
			item.noUseGraphic = true;
			item.shoot = ModContent.ProjectileType<TentacleChainProj>();
			item.shootSpeed = 11f;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.melee = true;
			item.channel = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			// How far out the inaccuracy of the shot chain can be.
			float radius = 20f;
			// Sets ai[1] to the following value to determine the firing direction.
			// The smaller the value of NextFloat(), the more accurate the shot will be. The larger, the less accurate. This changes depending on your radius.
			// NextBool().ToDirectionInt() will have a 50% chance to make it negative instead of positive.
			// The Solar Eruption uses this calculation: Main.rand.NextFloat(0f, 0.5f) * Main.rand.NextBool().ToDirectionInt() * MathHelper.ToRadians(45f);
			float direction = Main.rand.NextFloat(0.25f, 1f) * Main.rand.NextBool().ToDirectionInt() * MathHelper.ToRadians(radius);
			Projectile projectile = Projectile.NewProjectileDirect(player.RotatedRelativePoint(player.MountedCenter), new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI, 0f, direction);
			// Extra logic for the chain to adjust to item stats, unlike the Solar Eruption.
			if (projectile.modProjectile is TentacleChainProj modItem)
			{
				modItem.firingSpeed = item.shootSpeed * 2f;
				modItem.firingAnimation = item.useAnimation * 3;
				modItem.firingTime = item.useTime * 3;
			}
			return false;
		}
	}
}