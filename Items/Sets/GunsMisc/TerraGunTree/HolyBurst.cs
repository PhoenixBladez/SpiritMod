using Microsoft.Xna.Framework;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GunsMisc.TerraGunTree
{
	public class HolyBurst : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Maelstrom");
			Tooltip.SetDefault("Bullets shot are surrounded in angelic energy\nBullets shot inflict Angel's Light, a stacking debuff\nIf enemies receive three stacks, holy light rains down upon them\nStacks last for 1 second");
		}

		public override void SetDefaults()
		{
			Item.damage = 29;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 50;
			Item.height = 28;
			Item.useTime = 7;
			Item.useAnimation = 21;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 0.7f;
			Item.useTurn = false;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item31;
			Item.autoReuse = false;
			Item.shoot = ProjectileID.CrystalBullet;
			Item.shootSpeed = .02f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 45f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) {
				position += muzzleOffset;
			}
			int p = Projectile.NewProjectile(source, position.X, position.Y, velocity.X / 1.75f, velocity.Y / 1.75f, type, damage, knockback, player.whoAmI);
			Main.projectile[p].GetGlobalProjectile<SpiritGlobalProjectile>().shotFromHolyBurst = true;
			return false;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.ClockworkAssaultRifle, 1);
			recipe.AddIngredient(ItemID.HallowedBar, 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
