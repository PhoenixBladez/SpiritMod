using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GunsMisc.TerraGunTree
{
	public class TrueHolyBurst : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Hellstorm");
			Tooltip.SetDefault("Fires a five round burst of angelic energy\nBullets shot inflict 'Angel's Wrath', a stacking debuff\nDifferent amounts of holy light rains down on enemies based on the stacks of 'Angel's Wrath' they have");
		}


		public override void SetDefaults()
		{
			Item.damage = 35;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 50;
			Item.height = 28;
			Item.useTime = 6;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 1f;
			Item.useTurn = false;
			Item.value = Terraria.Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item31;
			Item.autoReuse = false;
			Item.shoot = ProjectileID.CrystalBullet;
			Item.shootSpeed = 1f;
			Item.useAmmo = AmmoID.Bullet;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y - 1)) * 45f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) {
				position += muzzleOffset;
			}
			int p = Projectile.NewProjectile(source, position.X, position.Y, velocity.X / 1.5f, velocity.Y / 1.5f, type, damage, knockback, player.whoAmI);
			Main.projectile[p].GetGlobalProjectile<SpiritGlobalProjectile>().shotFromTrueHolyBurst = true;
			return false;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<HolyBurst>(), 1);
            recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
