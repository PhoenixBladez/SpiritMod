using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
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
			Item.damage = 44;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 20;
			Item.height = 38;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ProjectileID.Shuriken;
			Item.useAmmo = AmmoID.Arrow;
			Item.knockBack = 1;
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item5;
			Item.value = Item.buyPrice(0, 5, 0, 0);
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.autoReuse = true;
			Item.shootSpeed = 13f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			if (charger++ >= 5)
			{
				for (int i = 0; i < 5; ++i)
				{
					Vector2 vel = velocity.RotatedByRandom(0.5f);
					int proj = Projectile.NewProjectile(source, position.X, position.Y, vel.X, vel.Y, ProjectileID.GreekFire3, 50, knockback, player.whoAmI, 0f, 0f);

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