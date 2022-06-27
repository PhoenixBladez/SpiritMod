using SpiritMod.Items.Material;
using SpiritMod.Projectiles.DonatorItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.DonatorItems
{
	public class Terravolt : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terravolt");
			Tooltip.SetDefault("Launches a beam of electricity ");
		}

		public override void SetDefaults()
		{
			Item.damage = 80;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.DamageType = DamageClass.Melee;
			Item.width = 48;
			Item.height = 48;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = Item.value = Item.sellPrice(0, 2, 50, 0); ;
			Item.rare = ItemRarityID.Yellow;
			Item.shoot = ModContent.ProjectileType<ElectricityBolt>();
			Item.shootSpeed = 25f;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useTurn = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ItemID.Ectoplasm, 6);
            recipe.AddIngredient(ItemID.Keybrand, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}