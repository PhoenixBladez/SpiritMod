using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.AstronautVanity
{
	[AutoloadEquip(EquipType.Body, EquipType.Back)]
	public class AstronautBody : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Astronaut Suit");

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 15, 0);
			Item.rare = ItemRarityID.Green;
			Item.vanity = true;
		}

		public override void UpdateEquip(Player player)
		{
			if (player.armor[11].IsAir)
				player.back = (sbyte)EquipLoader.GetEquipSlot(Mod, nameof(AstronautBody), EquipType.Back);
		}
	}
}
