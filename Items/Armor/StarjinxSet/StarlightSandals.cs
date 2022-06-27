using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.StarjinxSet
{
	[AutoloadEquip(EquipType.Legs)]
    public class StarlightSandals : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starlight Sandals");
			Tooltip.SetDefault("8% increased magic damage\n25% increased movement speed and fall speed\nIncreases jump speed");
		}

		public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 16;
            Item.value = Item.sellPrice(gold : 8, silver : 50);
            Item.rare = ItemRarityID.Pink;
            Item.defense = 6;
        }

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Magic) += 0.08f;
			player.moveSpeed *= 1.25f;
			player.jumpSpeedBoost += 1.75f;
			player.maxFallSpeed *= 1.25f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(Mod, "Starjinx", 9);
			recipe.AddIngredient(ItemID.Silk, 5);
			recipe.AddIngredient(ItemID.FallenStar, 6);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
    }
}
