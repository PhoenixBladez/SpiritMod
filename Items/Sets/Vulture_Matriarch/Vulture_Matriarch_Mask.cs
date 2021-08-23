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
            item.width = 26;
            item.height = 34;
            item.rare = ItemRarityID.Blue;
            item.vanity = true;
        }

		public override void SetStaticDefaults() => DisplayName.SetDefault("Vulture Matriarch Mask");

		public override void UpdateEquip(Player player) => Vulture_Matriarch_Mask_Visuals.maskEquipped = true;
	}
}