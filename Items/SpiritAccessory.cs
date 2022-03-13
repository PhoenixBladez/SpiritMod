using System.Collections.Generic;
using Terraria;

namespace SpiritMod.Items
{
	public abstract class SpiritAccessory : SpiritItem
	{
		public virtual bool CanEquipVanity => true;
		public virtual float AllDamage => 0f;
		public virtual float MeleeDamage => 0f;
		public virtual float RangedDamage => 0f;
		public virtual float MagicDamage => 0f;
		public virtual int AllCrit => 0;
		public virtual int MeleeCrit => 0;
		public virtual int RangedCrit => 0;
		public virtual int MagicCrit => 0;
		public virtual float MeleeSpeed => 0f;
		public virtual float MovementSpeed => 0f;
		public virtual int LifeRegen => 0;
		public virtual int ArmorPenetration => 0;

		public virtual List<SpiritPlayerEffect> AccessoryEffects => new List<SpiritPlayerEffect>();
		public readonly List<SpiritPlayerEffect> _accEffects;
		public virtual List<int> MutualExclusives => new List<int>();
		public readonly List<int> _mutExclusives;

		public SpiritAccessory() : base()
		{
			_accEffects = AccessoryEffects;
			_mutExclusives = MutualExclusives;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			foreach (var acc in _accEffects) {
				player.GetSpiritPlayer().effects.Add(acc);
				acc.ItemUpdateAccessory(player, hideVisual);
			}
			player.allDamage += AllDamage;
			player.meleeDamage += MeleeDamage;
			player.rangedDamage += RangedDamage;
			player.magicDamage += MagicDamage;
			player.AddAllCrit(AllCrit);
			player.meleeCrit += MeleeCrit;
			player.rangedCrit += RangedCrit;
			player.magicCrit += MagicCrit;
			player.meleeSpeed += MeleeSpeed;
			player.moveSpeed += MovementSpeed;
			player.maxRunSpeed += MovementSpeed;
			player.lifeRegen += LifeRegen;
			player.armorPenetration += ArmorPenetration;
		}

		public override bool CanEquipAccessory(Player player, int slot)
		{
			if (slot < 10) {
				int maxAccessoryIndex = 5 + player.extraAccessorySlots;
				for (int i = 3; i < 3 + maxAccessoryIndex; i++) {
					// We need "slot != i" because we don't care what is currently in the slot we will be replacing.
					if (slot != i && _mutExclusives.Contains(player.armor[i].type)) {
						return false;
					}
				}
			}
			return CanEquipVanity;
		}
	}
}
