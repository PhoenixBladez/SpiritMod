using Microsoft.Xna.Framework;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.BossLoot.AtlasDrops
{
	public class QuakeFist : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quake Fist");
			Tooltip.SetDefault("Launches Prismatic fire \nOccasionally inflicts foes with 'Unstable Affliction'");
		}


		private Vector2 newVect;

		public override void SetDefaults()
		{
			Item.damage = 67;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 19;
			Item.width = 30;
			Item.height = 34;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Shoot;//this makes the useStyle animate as a staff instead of as a gun
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 6;
			Item.useTurn = false;
			Item.value = Item.sellPrice(0, 7, 0, 0);
			Item.rare = ItemRarityID.Cyan;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<PrismaticBolt>();
			Item.shootSpeed = 16f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			int proj = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<PrismBolt2>(), damage, knockback, player.whoAmI);
			Projectile newProj = Main.projectile[proj];
			newProj.friendly = true;
			newProj.hostile = false;
			Vector2 origVect = velocity;
			for (int X = 0; X <= 2; X++) {
				if (Main.rand.NextBool(2)) {
					newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(82, 1800) / 10));
				}
				else {
					newVect = origVect.RotatedBy(-System.Math.PI / (Main.rand.Next(82, 1800) / 10));
				}
				int proj2 = Projectile.NewProjectile(source, position.X, position.Y, newVect.X, newVect.Y, type, damage, knockback, player.whoAmI);
			}
			for (int i = 0; i < 3; ++i) {
				if (Main.rand.NextBool(6)) {
					if (Main.myPlayer == player.whoAmI) {
						Vector2 mouse = Main.MouseWorld;
						Projectile.NewProjectile(source, mouse.X + Main.rand.Next(-80, 80), player.Center.Y - 1000 + Main.rand.Next(-50, 50), 0, Main.rand.Next(18, 28), ModContent.ProjectileType<AtlasBolt>(), 50, knockback, player.whoAmI);
					}
				}
			}
			return false;
		}
	}
}