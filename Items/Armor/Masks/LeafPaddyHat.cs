using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.Masks
{
	[AutoloadEquip(EquipType.Head)]
	public class LeafPaddyHat : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rice Paddy Hat");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;

			item.value = 3000;
			item.rare = ItemRarityID.Blue;
			item.vanity = true;
		}
        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
            => drawAltHair = true;

    }
}
