
using SpiritMod.Items.Material;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    [AutoloadEquip(EquipType.Neck)]
    public class CleftHorn : SpiritAccessory
    {
        public override string SetDisplayName => "Cleft Horn";
        public override string SetTooltip => "Increases armor penetration by 3\nMelee attacks occasionally strike enemies twice";
        public override int ArmorPenetration => 3;
        public override List<SpiritPlayerEffect> AccessoryEffects => new List<SpiritPlayerEffect>() {
            new CleftHornEffect()
        };

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.buyPrice(silver: 50);
            item.rare = ItemRarityID.Green;
            item.accessory = true;
            item.defense = 1;
        }
    }

	public class CleftHornEffect : SpiritPlayerEffect
	{
		public override void PlayerOnHitNPC(Player player, Item item, NPC target, int damage, float knockback, bool crit)
		{
			if (item.melee && Main.rand.NextBool(9)) {
				target.StrikeNPC(item.damage / 2, 0f, 0, crit);
			}
		}
	}
}
