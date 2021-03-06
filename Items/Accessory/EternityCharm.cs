using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class EternityCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eternity Charm");
			Tooltip.SetDefault("You are the champion of Spirits\nLaunches a multitude of Soul Shards when damaged");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(7, 8));
		}


		public override void SetDefaults()
		{
			item.width = 18;
			item.expert = true;
			item.height = 18;
			item.value = Item.buyPrice(0, 22, 0, 0);
			item.rare = ItemRarityID.Purple;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.moveSpeed += 0.5f;
			player.maxRunSpeed += 5f;
			player.GetSpiritPlayer().OverseerCharm = true;
		}

	}
}
