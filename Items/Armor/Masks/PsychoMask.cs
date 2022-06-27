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
            SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Armor/Masks/PsychoMask_Glow");
			ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = Item.buyPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.vanity = true;
		}

        public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
        {
            glowMaskColor = Color.White;
        }
    }
}
