using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ToolsMisc.OreWarhammersSet
{
	public class MythrilWarhammer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mythril Warhammer");
		}


		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 38;
			Item.value = 10000;
			Item.rare = ItemRarityID.LightRed;

			Item.hammer = 83;

			Item.damage = 44;
			Item.knockBack = 6f;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 30;
			Item.useAnimation = 30;

			Item.DamageType = DamageClass.Melee;
			Item.useTurn = true;
			Item.autoReuse = true;

			Item.UseSound = SoundID.Item1;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.MythrilBar, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}