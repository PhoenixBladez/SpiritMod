using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Equipment
{
	public class SandsOfTime : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sands of Time");
			Tooltip.SetDefault("Summons or ends a sandstorm\nOnly usable in a desert");
		}

		public override void SetDefaults()
		{
			Item.width = 44;
			Item.height = 48;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.noMelee = true;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Blue;
			Item.mana = 20;
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = false;
		}

		public override bool CanUseItem(Player player) => player.ZoneDesert;

		public override bool? UseItem(Player player)
		{
			if(Sandstorm.Happening) {
				Sandstorm.Happening = false;
			}
			else {
				Sandstorm.Happening = true;
				Sandstorm.TimeLeft = 6000;
				Sandstorm.Severity = 1;
				Sandstorm.IntendedSeverity = 1;
			}

			if (Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.WorldData);

			return null;
		}
	}
}