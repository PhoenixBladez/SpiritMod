using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class StoneHead : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stone Helmet");
			Tooltip.SetDefault("Increases melee damage by 1");
		}


		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 22;
			item.value = 0;
			item.rare = ItemRarityID.White;
			item.defense = 2;
		}
		public override void UpdateEquip(Player player) => player.GetSpiritPlayer().stoneHead = true;

		public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<StoneBody>() && legs.type == ModContent.ItemType<StoneLegs>();
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Grants immunity to knockback";
			player.noKnockback = true;
		}

		public override void ArmorSetShadows(Player player)
		{
			if (player.velocity.Y > 0)
				player.armorEffectDrawShadow = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StoneBlock, 35);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}