using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class IchorPendant : SpiritAccessory
	{
		public override string SetDisplayName => "Ichor Pendant";
		public override string SetTooltip => "6% increased melee damage\nMelee hits occasionally inflict Ichor";

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.buyPrice(gold: 3);
			item.rare = ItemRarityID.LightRed;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			player.meleeDamage += 0.06f;
		}

		public override void PlayerModifyHitNPC(Player player, Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
		{
			if(Main.rand.NextBool(10)) target.AddBuff(BuffID.Ichor, 180);
		}
	}
}
