using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.InfernonDrops
{
	public class SevenSins : ModItem
	{
		int charger;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Seven Sins");
			Tooltip.SetDefault("Occasionally shoots out a volley of flames");
		}

		public override void SetDefaults()
		{
			item.damage = 44;
			item.noMelee = true;
			item.ranged = true;
			item.width = 20;
			item.height = 38;
			item.useTime = 22;
			item.useAnimation = 22;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.Shuriken;
			item.useAmmo = AmmoID.Arrow;
			item.knockBack = 1;
			item.rare = ItemRarityID.Pink;
			item.UseSound = SoundID.Item5;
			item.value = Item.buyPrice(0, 5, 0, 0);
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.autoReuse = true;
			item.shootSpeed = 13f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (charger++ >= 5)
			{
				for (int i = 0; i < 5; ++i)
				{
					Vector2 vel = new Vector2(speedX, speedY).RotatedByRandom(0.5f);
					int proj = Projectile.NewProjectile(position.X, position.Y, vel.X, vel.Y, ProjectileID.GreekFire3, 50, knockBack, player.whoAmI, 0f, 0f);

					Main.projectile[proj].hostile = false;
					Main.projectile[proj].friendly = true;
					Main.projectile[proj].penetrate = 2;
				}
				charger = 0;
			}
			return true;
		}
	}
}