using Microsoft.Xna.Framework;
using SpiritMod.Items.Ammo;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Bullet;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Gun
{
	public class CryoGun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Winter's Wake");
			Tooltip.SetDefault("Fires rockets");
		}

		public override void SetDefaults()
		{
			item.damage = 30;
			item.crit = 4;
			item.noMelee = true;
			item.rare = ItemRarityID.Orange;
			item.width = 50;
			item.height = 26;
			item.useAnimation = 60;
			item.useTime = 60;
			item.knockBack = 2.5f;
			item.UseSound = SoundID.Item13;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.autoReuse = false;
			item.shoot = ProjectileID.RocketI;
			item.shootSpeed = 10f;
			item.useAmmo = AmmoID.Rocket;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-8, 0);

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Handgun);
			recipe.AddIngredient(ModContent.ItemType<CryoliteBar>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}