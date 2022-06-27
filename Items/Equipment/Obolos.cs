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
            Item.width = 20;
            Item.height = 30;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.value = Item.buyPrice(gold: 15);
            Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item106;
            Item.noMelee = true;
            Item.mountType = Mod.Find<ModMount>("Obolos_Mount").Type;
        }  
    }
}