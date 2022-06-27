using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SpiritMod.Items.Armor.PlagueDoctor
{
	[AutoloadEquip(EquipType.Body)]
	public class PlagueDoctorRobe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plague Doctor's Robe");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Armor/PlagueDoctor/PlagueDoctorRobe_Glow");

		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 20;
			Item.value = Terraria.Item.sellPrice(0, 0, 18, 0);
			Item.rare = ItemRarityID.Green;
			Item.vanity = true;
		}
		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
		{
			glowMaskColor = Color.White;
		}
	}
}
