
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class SpellswordCrest : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spellsword's Crest");
			Tooltip.SetDefault("Summons a holy sword to fight for you\nThis sword does not take up minion slots");
		}


		public override void SetDefaults()
		{
            item.damage = 27;
            item.summon = true;
			item.width = 48;
			item.height = 49;
            item.knockBack = 1.25f;
			item.value = Item.sellPrice(0, 1, 50, 0);
			item.rare = 4;
			item.defense = 1;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
            player.GetSpiritPlayer().spellswordCrest = true;
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<RogueCrest>(), 1);
            recipe.AddIngredient(ItemID.CrystalShard, 8);
            recipe.AddIngredient(ItemID.SoulofLight, 15);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
