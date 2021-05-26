using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.StarjinxSet
{
	[AutoloadEquip(EquipType.Body)]
    public class StarlightMantle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starlight Mantle");
			Tooltip.SetDefault("8% increased magic damage\n20% increased flight time");
		}
		public override void SetDefaults()
        {
            item.width = 26;
            item.height = 18;
			item.value = Item.sellPrice(gold: 10);
			item.rare = ItemRarityID.Pink;
			item.defense = 10;
		}
		public override void UpdateEquip(Player player)
		{
			player.magicDamage += 0.08f;
			MyPlayer modplayer = (MyPlayer)player.GetModPlayer(mod, "MyPlayer");
			modplayer.WingTimeMaxMultiplier += 0.2f;
		}
		public override bool DrawBody() => false;
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod, "Starjinx", 11);
			recipe.AddIngredient(ItemID.Silk, 6);
			recipe.AddIngredient(ItemID.FallenStar, 7);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
    }
}
