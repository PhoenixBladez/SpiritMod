using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Ammo.Bullet
{
	public class SpectreBullet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spectre Bullet");
			Tooltip.SetDefault("A spectral bolt that homes on to enemies and occasionally saps their life");
		}

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.value = 1000;
			Item.rare = ItemRarityID.Cyan;
			Item.maxStack = 999;
			Item.damage = 10;
			Item.knockBack = 1.5f;
			Item.ammo = AmmoID.Bullet;
			Item.DamageType = DamageClass.Ranged;
			Item.consumable = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Bullet.SpectreBullet>();
			Item.shootSpeed = 9f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(333);
			recipe.AddIngredient(ItemID.SpectreBar, 3);
			recipe.AddIngredient(ItemID.SoulofMight, 1);
			recipe.AddIngredient(ItemID.SoulofFright, 1);
			recipe.AddIngredient(ItemID.SoulofSight, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}