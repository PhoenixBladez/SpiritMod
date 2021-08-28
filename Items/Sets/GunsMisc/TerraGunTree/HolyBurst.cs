using Microsoft.Xna.Framework;
using SpiritMod.Projectiles;
using Terraria;
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
			item.damage = 29;
			item.ranged = true;
			item.width = 50;
			item.height = 28;
			item.useTime = 7;
			item.useAnimation = 21;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 0.7f;
			item.useTurn = false;
			item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.UseSound = SoundID.Item31;
			item.autoReuse = false;
			item.shoot = ProjectileID.CrystalBullet;
			item.shootSpeed = .02f;
			item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) {
				position += muzzleOffset;
			}
			int p = Projectile.NewProjectile(position.X, position.Y, speedX / 1.75f, speedY / 1.75f, type, damage, knockBack, player.whoAmI);
			Main.projectile[p].GetGlobalProjectile<SpiritGlobalProjectile>().shotFromHolyBurst = true;
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ClockworkAssaultRifle, 1);
			recipe.AddIngredient(ItemID.HallowedBar, 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
