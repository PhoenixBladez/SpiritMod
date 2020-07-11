using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Flail;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Flail
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
			item.useAnimation = 15;
			item.useTime = 15;
			item.knockBack = 0;
			item.value = Item.sellPrice(0, 1, 20, 0);
			item.damage = 19;
			item.noUseGraphic = true;
			item.shoot = ModContent.ProjectileType<TentacleChainProj>();
			item.shootSpeed = 22f;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.melee = true;
			item.channel = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 direction9 = new Vector2(speedX, speedY).RotatedBy(Main.rand.NextFloat(-0.22f, 0.22f));
			Projectile.NewProjectile(position, direction9, type, damage, knockBack, player.whoAmI, direction9.X, direction9.Y);
			return false;
		}
	}
}