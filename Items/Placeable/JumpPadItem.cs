using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable
{
	public class JumpPadItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jump Pad");
			Tooltip.SetDefault("'Take to the skies!'");
		}


		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.rare = ItemRarityID.Green;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.createTile = Mod.Find<ModTile>("JumpPadTile").Type;
			Item.maxStack = 999;
			Item.autoReuse = false;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.consumable = true;

		}
	}
}
