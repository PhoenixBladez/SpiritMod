using Microsoft.Xna.Framework;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.BossLoot.AtlasDrops
{
	public class Earthshatter : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Earthshatter");
			Tooltip.SetDefault("Shoots out Earthen rocks along with arrows");
		}


		private Vector2 newVect;
		public override void SetDefaults()
		{
			Item.damage = 51;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 20;
			Item.height = 40;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ProjectileID.Starfury;
			Item.useAmmo = AmmoID.Arrow;
			Item.knockBack = 6;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Cyan;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.shootSpeed = 18f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			int proj = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<Earthrock>(), damage, knockback, player.whoAmI);
			Projectile newProj = Main.projectile[proj];
			newProj.friendly = true;
			newProj.hostile = false;
			Vector2 origVect = velocity;
			for (int X = 0; X <= 2; X++)
			{
				if (Main.rand.NextBool(2))
					newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(82, 1800) / 10));
				else
					newVect = origVect.RotatedBy(-System.Math.PI / (Main.rand.Next(82, 1800) / 10));
				
				Projectile.NewProjectile(source, position.X, position.Y, newVect.X, newVect.Y, type, damage, knockback, player.whoAmI);
			}
			return false;
		}
	}
}