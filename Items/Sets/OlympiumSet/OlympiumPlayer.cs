using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.OlympiumSet
{
	public class OlympiumPlayer : ModPlayer
	{
		public bool eleutherios = false;
		public float eleutheoriosStrength = 0;

		public override void ResetEffects() => eleutherios = false;

		public override void GetHealLife(Item item, bool quickHeal, ref int healValue)
		{
			if (eleutherios) //Only set value as this is called every frame when the item is being hovered over or used. 
				healValue = (int)(healValue * .85f); //We add the buff in OlympiumGlobalItem.UseItem
		}

		public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
		{
			if (eleutherios)
				mult += eleutheoriosStrength + 1;
		}
	}
}
