using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class WheezerScale : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wheezer Scale");
			Tooltip.SetDefault("Melee hits on foes may cause them to emit a cloud of poisonous gas\n5% increased melee speed");
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 1, 40, 0);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().wheezeScale = true;
			player.GetAttackSpeed(DamageClass.Melee) += .05f;
		}
	}
}
