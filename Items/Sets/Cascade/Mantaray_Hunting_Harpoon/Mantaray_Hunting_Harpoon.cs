using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
 
namespace SpiritMod.Items.Sets.Cascade.Mantaray_Hunting_Harpoon
{
    public class Mantaray_Hunting_Harpoon : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 30;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 1;
            item.rare = 8;
			item.UseSound = SoundID.Item3;
            item.noMelee = true;
			item.mountType = mod.MountType("Mantaray_Mount");
			item.value = Item.sellPrice(gold: 5);
        }  
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Manta Harpoon");
			Tooltip.SetDefault("Summons a rideable manta ray mount\nManta Ray is incapable of movement on land");
		}
    }
}