using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material.Artifact
{
    public class DarkCrest : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Crest");
			Tooltip.SetDefault("'Shadows can be glimpsed within'");
            ItemID.Sets.ItemNoGravity[item.type] = true;
            ItemID.Sets.ItemIconPulse[item.type] = true;
        }


        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 26;
            item.value = 500;
            item.rare = 6;
            item.maxStack = 1;
        }
    }
}