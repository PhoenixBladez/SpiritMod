using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GunsMisc.CaptainsRegards
{
	public class CaptainsRegards : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Captain's Regards");
			Tooltip.SetDefault("'Pirate diplomacy at its finest'");
		}

		public override void SetDefaults()
		{
			Item.damage = 20;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 65;
			Item.height = 62;
			Item.useTime = 24;
			Item.useAnimation = 22;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 6;
			Item.useTurn = false;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item36;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.PurificationPowder;
			Item.shootSpeed = 6.8f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			int proj = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileID.CannonballFriendly, damage, knockback, player.whoAmI);
			Projectile newProj = Main.projectile[proj];
			newProj.friendly = true;
			newProj.hostile = false;
			Vector2 origVect = velocity;

			for (int X = 0; X < 3; X++)
			{
				Vector2 newVect = origVect.RotatedBy(-System.Math.PI / (Main.rand.Next(82, 1800) / 10) * (Main.rand.NextBool() ? -1 : 1));
				int proj2 = Projectile.NewProjectile(source, position.X, position.Y, newVect.X, newVect.Y, type, 45, knockback, player.whoAmI);
				Projectile newProj2 = Main.projectile[proj2];
				newProj2.timeLeft = Main.rand.Next(13, 30);
			}
			return false;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);
	}
}