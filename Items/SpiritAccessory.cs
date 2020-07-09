using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace SpiritMod.Items
{
	public abstract class SpiritAccessory : SpiritPlayerAffectingItem
	{
		public override void UpdateAccessory(Player player, bool hideVisual) 
			=> player.GetSpiritPlayer().accessories.Add(this);
	}
}
