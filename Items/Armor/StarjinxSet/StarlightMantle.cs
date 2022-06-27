using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.StarjinxSet
{
	[AutoloadEquip(EquipType.Body)]
    public class StarlightMantle : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starlight Mantle");
			Tooltip.SetDefault("8% increased magic damage\n20% increased flight time");
		}

		public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 18;
			Item.value = Item.sellPrice(gold: 10);
			Item.rare = ItemRarityID.Pink;
			Item.defense = 10;
		}

		public override void UpdateEquip(Player player)
		{
			player.magicDamage += 0.08f;
			MyPlayer modplayer = player.GetModPlayer<MyPlayer>();
			modplayer.WingTimeMaxMultiplier += 0.2f;
		}

		public override bool DrawBody() => false;

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(Mod, "Starjinx", 11);
			recipe.AddIngredient(ItemID.Silk, 6);
			recipe.AddIngredient(ItemID.FallenStar, 7);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
    }
}
