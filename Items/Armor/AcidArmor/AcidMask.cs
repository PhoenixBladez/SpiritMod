using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.AcidArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class AcidMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acid Mask");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 18;
			item.value = 46000;
			item.rare = ItemRarityID.LightRed;
			item.vanity = true;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<AcidBody>() && legs.type == ModContent.ItemType<AcidLegs>();
		}
		public override void UpdateArmorSet(Player player)
		{

			player.setBonus = "Getting hurt may release an acid explosion, causing enemies to suffer Acid Burn \nThrowing hits may instantly kill non boss enemies extremely rarely.";
			player.GetSpiritPlayer().acidSet = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Acid>(), 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}