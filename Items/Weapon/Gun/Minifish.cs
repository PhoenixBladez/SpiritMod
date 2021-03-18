using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Gun
{
	public class Minifish : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gatling Guppy");
			Tooltip.SetDefault("Hitting enemies spawns temporary additional minifish that fire towards your cursor, and do not consume ammo\n'Strength in numbers'");
        }
		public override void SetDefaults()
		{
			item.damage = 12;
			item.ranged = true;
			item.width = 24;
			item.height = 24;
			item.useTime = item.useAnimation = 18;
			item.reuseDelay = 4;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 1;
			item.useTurn = false;
			item.useAmmo = AmmoID.Bullet;
			item.UseSound = SoundID.Item11;
			item.value = Item.sellPrice(0, 9, 50, 0);
			item.rare = ItemRarityID.Orange;
			item.autoReuse = true;
			item.shoot = ProjectileID.Bullet;
			item.shootSpeed = 10f;
		}
		public override Vector2? HoldoutOffset() => new Vector2(-4, 0);
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<TribalScale>(), 5);
			recipe.AddIngredient(ItemID.Minishark);
			recipe.AddIngredient(ItemID.IllegalGunParts);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}