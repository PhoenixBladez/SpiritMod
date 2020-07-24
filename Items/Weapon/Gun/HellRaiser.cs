using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Bullet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Gun
{
	public class HellRaiser : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hell Raiser");
			Tooltip.SetDefault("Converts regular bullets into high velocity, fiery bullets");
		}


		public override void SetDefaults()
		{
			item.damage = 70;
			item.ranged = true;
			item.width = 58;
			item.height = 32;
			item.useTime = 35;
			item.useAnimation = 35;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 8;
			item.useTurn = false;
			item.value = Terraria.Item.sellPrice(0, 12, 0, 0);
			item.rare = ItemRarityID.LightPurple;
			item.UseSound = SoundID.Item11;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<HellBullet>();
			item.shootSpeed = 19f;
			item.useAmmo = AmmoID.Bullet;
			item.crit = 18;
		}
		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (type == ProjectileID.Bullet) {
				type = ModContent.ProjectileType<HellBullet>();
			}
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FieryEssence>(), 14);
			recipe.AddTile(ModContent.TileType<Tiles.Furniture.EssenceDistorter>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}
	}
}