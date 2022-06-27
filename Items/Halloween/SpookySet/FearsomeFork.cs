using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Halloween.SpookySet
{
	public class FearsomeFork : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fearsome Fork");
			Tooltip.SetDefault("Launches pumpkins");
		}


		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.width = 24;
			Item.height = 24;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.useAnimation = 27;
			Item.useTime = 27;
			Item.shootSpeed = 5f;
			Item.knockBack = 8f;
			Item.damage = 67;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.shoot = ModContent.ProjectileType<Projectiles.Held.FearsomeFork>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.SpookyWood, 12);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}