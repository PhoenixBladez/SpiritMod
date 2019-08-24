using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material.Artifact
{
    public class PrimordialMagic : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Primordial Magic");
			Tooltip.SetDefault("'A fragment of the elements...'");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 6));
            ItemID.Sets.ItemNoGravity[item.type] = true;
            ItemID.Sets.ItemIconPulse[item.type] = true;
        }


        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 35;
            item.value = 100;
            item.rare = 1;
            item.maxStack = 999;

        }
    }
}