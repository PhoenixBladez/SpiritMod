
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
            Item.damage = 32;
            Item.DamageType = DamageClass.Summon;
			Item.width = 48;
			Item.height = 49;
            Item.knockBack = 1.25f;
			Item.value = Item.sellPrice(0, 1, 50, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.defense = 1;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) => player.GetSpiritPlayer().spellswordCrest = true;

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<RogueCrest>(), 1);
            recipe.AddIngredient(ItemID.CrystalShard, 8);
            recipe.AddIngredient(ItemID.SoulofLight, 15);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}
