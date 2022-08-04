using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.BotanistSet
{
	[AutoloadEquip(EquipType.Head)]
	public class BotanistHat : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sunflower Strawhat");
			Tooltip.SetDefault("Keeps you happy");

			ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.White;
			Item.defense = 2;
		}

		public override void UpdateEquip(Player player) => player.AddBuff(BuffID.Sunflower, 2);

		public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<BotanistBody>() && legs.type == ModContent.ItemType<BotanistLegs>();

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "You gain increased rewards from herbs of all kinds\nYou have a powerful eye for spotting herbs";
			player.GetModPlayer<BotanistPlayer>().active = true;

			var tex = EquipLoader.GetEquipTexture(Mod, "AstronautBody", EquipType.Back);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Sunflower, 1)
				.AddIngredient(ItemID.Hay, 10)
				.AddIngredient(ItemID.FallenStar, 3)
				.AddTile(TileID.Loom)
				.Register();
		}
	}
}
