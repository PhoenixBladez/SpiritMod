using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Gun
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
			item.damage = 35;
			item.ranged = true;
			item.width = 50;
			item.height = 28;
			item.useTime = 6;
			item.useAnimation = 30;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 1f;
			item.useTurn = false;
			item.value = Terraria.Item.sellPrice(0, 5, 0, 0);
			item.rare = 8;
			item.UseSound = SoundID.Item31;
			item.autoReuse = false;
			item.shoot = 89;
			item.shootSpeed = 1f;
			item.useAmmo = AmmoID.Bullet;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY - 1)) * 45f;
			if(Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) {
				position += muzzleOffset;
			}
			int p = Projectile.NewProjectile(position.X, position.Y, speedX / 1.5f, speedY / 1.5f, type, damage, knockBack, player.whoAmI);
			Main.projectile[p].GetGlobalProjectile<SpiritGlobalProjectile>().shotFromTrueHolyBurst = true;
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<HolyBurst>(), 1);
			recipe.AddIngredient(ModContent.ItemType<BrokenParts>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
