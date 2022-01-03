using SpiritMod.Projectiles;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class LongFuse : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Long Fuse");
			Tooltip.SetDefault("Explosives burn slowly when this is in the inventory"); //needs reword
		}

		public override void SetDefaults()
		{
			item.Size = new Microsoft.Xna.Framework.Vector2(22, 28);
			item.rare = ItemRarityID.Blue;
			item.value = Item.sellPrice(0, 0, 40, 0);
		}

		public override void UpdateInventory(Player player) => player.GetModPlayer<MyPlayer>().longFuse = true;
	}
}
