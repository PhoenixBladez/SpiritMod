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
			Item.width = 36;
			Item.height = 36;
			Item.maxStack = 999;
			Item.rare = ItemRarityID.Red;
			Item.value = Item.buyPrice(3, 0, 0, 0);
			Item.useAnimation = 45;
			Item.useTime = 45;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.UseSound = SoundID.Item44;
			Item.consumable = true;
		}

		public override bool? UseItem(Player player)
		{
			player.AddBuff(ModContent.BuffType<Buffs.FateToken>(), 3600);
			player.GetSpiritPlayer().fateToken = true;
			player.GetSpiritPlayer().shootDelay3 = 7200;

			Main.NewText("Fate has blessed you");
			return null;
		}

		public override bool CanUseItem(Player player) => player.GetSpiritPlayer().shootDelay3 == 0;
	}
}
