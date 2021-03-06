using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SpiritMod.Items.Consumable
{
	public class FateToken : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fate Token");
			Tooltip.SetDefault("Taking fatal damage will instead return you to 500 health\n2 minute cooldown");

		}


		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 36;
			item.maxStack = 999;
			item.rare = ItemRarityID.Red;
			item.value = Item.buyPrice(3, 0, 0, 0);
			item.useAnimation = 45;
			item.useTime = 45;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.UseSound = SoundID.Item44;
			item.consumable = true;
		}


		public override bool UseItem(Player player)
		{
			player.AddBuff(ModContent.BuffType<Buffs.FateToken>(), 3600);
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (!modPlayer.fateToken) {
				modPlayer.fateToken = true;
			}
			Main.NewText("Fate has blessed you");
			modPlayer.shootDelay3 = 7200;
			return true;
		}
		public override bool CanUseItem(Player player)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (modPlayer.shootDelay3 == 0)
				return true;
			return false;
		}
	}
}
