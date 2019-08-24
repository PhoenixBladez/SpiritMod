using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material.Artifact
{
    public class KeystoneShard : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Keystone Shard");
			Tooltip.SetDefault("'Fragments of a powerful relic'");
            ItemID.Sets.ItemIconPulse[item.type] = true;
        }


        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = 100;
            item.rare = 1;
            item.maxStack = 1;
        }
    }
}