using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace SpiritMod.Items
{
	public abstract class SpiritSetbonusHead : SpiritItem
	{
		public virtual string SetbonusText => "";
		public virtual SpiritPlayerEffect SetbonusEffect => null;
		private SpiritPlayerEffect _setbonus;
		public virtual bool SetBody(Item body) => false;
		public virtual bool SetLegs(Item legs) => false;

		public SpiritSetbonusHead() : base()
		{
			_setbonus = SetbonusEffect;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
			=> SetBody(body) && SetLegs(legs);

		public override void UpdateArmorSet(Player player)
		{
			if(_setbonus != null) {
				player.setBonus = SetbonusText;
				player.GetSpiritPlayer().setbonus = _setbonus;
				_setbonus.ItemUpdateArmorSet(player);
			}
		}
	}
}
