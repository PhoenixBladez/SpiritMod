using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using System;
using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Armor.Masks
{
	[AutoloadEquip(EquipType.Head)]
	public class PsychoMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Psycho Mask");
            Tooltip.SetDefault("'A flower? For meeee?'");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Armor/Masks/PsychoMask_Glow");
        }

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;

			item.value = Terraria.Item.buyPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.Blue;
			item.vanity = true;
		}
        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
            => drawHair = true;
        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            glowMaskColor = Color.White;
        }
    }
}
