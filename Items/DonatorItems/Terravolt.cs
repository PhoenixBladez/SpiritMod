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


		int charger;
		public override void SetDefaults()
		{
			item.damage = 80;
			item.useTime = 25;
			item.useAnimation = 25;
			item.melee = true;
			item.width = 48;
			item.height = 48;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 6;
			item.value = item.value = Item.sellPrice(0, 2, 50, 0); ;
			item.rare = ItemRarityID.Yellow;
			item.shoot = ModContent.ProjectileType<ElectricityBolt>();
			item.shootSpeed = 25f;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useTurn = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Ectoplasm, 6);
            recipe.AddIngredient(ItemID.Keybrand, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();



		}
	}
}