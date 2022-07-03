using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace SpiritMod.Items.Sets.Vulture_Matriarch
{
    [AutoloadEquip(EquipType.Head)]
    public class Vulture_Matriarch_Mask : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 34;
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
        }

		public override void SetStaticDefaults() => DisplayName.SetDefault("Vulture Matriarch Mask");

		public override void UpdateEquip(Player player) => player.GetModPlayer<Vulture_Matriarch_Mask_Visuals>().maskEquipped = true;
	}
}