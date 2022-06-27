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
            SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Armor/CapacitorSet/CapacitorLegs_Glow");
        }
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 30, 0);
			Item.rare = ItemRarityID.Green;

			Item.vanity = true;
		}
        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            glowMaskColor = Color.White;
        }
    }
}
