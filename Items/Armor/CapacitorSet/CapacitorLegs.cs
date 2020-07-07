using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.CapacitorSet
{
	[AutoloadEquip(EquipType.Legs)]
	public class CapacitorLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Capacitor's Leggings");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Armor/CapacitorSet/CapacitorLegs_Glow");
        }
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.value = Item.sellPrice(0, 0, 30, 0);
			item.rare = ItemRarityID.Green;

			item.vanity = true;
		}
        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            glowMaskColor = Color.White;
        }
    }
}
