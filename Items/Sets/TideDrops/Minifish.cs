using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.TideDrops
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
			Item.damage = 12;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 24;
			Item.height = 24;
			Item.useTime = Item.useAnimation = 18;
			Item.reuseDelay = 4;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 1;
			Item.useTurn = false;
			Item.useAmmo = AmmoID.Bullet;
			Item.UseSound = SoundID.Item11;
			Item.value = Item.sellPrice(0, 9, 50, 0);
			Item.rare = ItemRarityID.Orange;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 10f;
		}
		public override Vector2? HoldoutOffset() => new Vector2(-4, 0);
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<TribalScale>(), 5);
			recipe.AddIngredient(ItemID.Minishark);
			recipe.AddIngredient(ItemID.IllegalGunParts);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}