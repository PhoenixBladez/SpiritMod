using Microsoft.Xna.Framework;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
	public class CircleScimitar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Circe's Scimitar");
			Tooltip.SetDefault("Occaisionally shoots out a marble block");
		}


		public override void SetDefaults()
		{
			item.damage = 48;
			item.melee = true;
			item.width = 34;
			item.height = 40;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 4;
			item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.UseSound = SoundID.Item1;
			item.shoot = ModContent.ProjectileType<MarbleBrick>();
			item.shootSpeed = 6f;
			item.crit = 4;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int proj = Projectile.NewProjectile(position.X, position.Y, (speedX / 3) * 2, speedY, type, damage, knockBack, player.whoAmI);
			return true;
		}
	}
}