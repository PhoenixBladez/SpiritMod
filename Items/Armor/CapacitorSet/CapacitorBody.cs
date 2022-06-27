using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.CapacitorSet
{
	[AutoloadEquip(EquipType.Body)]
	public class CapacitorBody : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Capacitor's Robes");
            SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Armor/CapacitorSet/CapacitorBody_Glow");
        }
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = ItemRarityID.Green;

			Item.vanity = true;
		}
        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            glowMaskColor = Color.White;
        }
    }
}
