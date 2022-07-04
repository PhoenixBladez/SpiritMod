
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class MartianTransmitter : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Martian Transmitter");
			Tooltip.SetDefault("Summons the martian invasion \n'Broadcasting on strange frequencies'");
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 16;
			Item.rare = ItemRarityID.Cyan;
			Item.maxStack = 99;
			Item.value = 100000;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useTime = Item.useAnimation = 20;
			Item.noMelee = true;
			Item.consumable = true;
			Item.autoReuse = false;
			Item.UseSound = SoundID.Item43;
		}

		public override bool? UseItem(Player player)
		{
			NPC.NewNPC(player.GetSource_ItemUse(Item), (int)player.Center.X, (int)player.Center.Y - 64, 399);
			return null;
		}
	}
}
