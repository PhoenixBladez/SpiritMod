using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SpiritBiomeDrops
{
	public class Gravehunter : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gravehunter");
			Tooltip.SetDefault("'You can't tell if you guide the trigger or it guides you'");
		}


		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 24;
			Item.height = 46;
			Item.useTime = 6;
			Item.useAnimation = 18;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.reuseDelay = 35;
			Item.shoot = ProjectileID.Shuriken;
			Item.useAmmo = AmmoID.Arrow;
			Item.crit = 12;
			Item.knockBack = 1;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.shootSpeed = 6f;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Vector2 origVect = velocity;
			Vector2 newVect = Vector2.Zero;

			if (Main.rand.Next(2) == 1) {
				newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(140, 1800) / 10));
			}
			else {
				newVect = origVect.RotatedBy(-System.Math.PI / (Main.rand.Next(140, 1800) / 10));
			}
			int proj2 = Projectile.NewProjectile(source, position.X, position.Y, newVect.X, newVect.Y, type, damage, knockback, player.whoAmI);
			return false;
		}

	}
}