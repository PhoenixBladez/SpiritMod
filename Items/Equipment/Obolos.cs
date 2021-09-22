using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace SpiritMod.Items.Equipment
{
    public class Obolos : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Obolos");
			Tooltip.SetDefault("Summons a rideable Ethereal Boat for you to sail \n'Payment recieved'");
		}

		public override void SetDefaults()
        {
            item.width = 20;
            item.height = 30;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.value = Item.buyPrice(gold: 15);
            item.rare = ItemRarityID.Pink;
			item.UseSound = SoundID.Item106;
            item.noMelee = true;
            item.mountType = mod.MountType("Obolos_Mount");
        }  
    }
}