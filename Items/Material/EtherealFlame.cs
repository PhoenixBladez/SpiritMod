using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class EtherealFlame : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ethereal Flame");
		}


        public override void SetDefaults()
        {
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.width = 24;
            item.height = 28;
            item.value = 1000;
            item.rare = 5;

            item.maxStack = 999;
        }
    }
}