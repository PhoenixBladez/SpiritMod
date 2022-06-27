using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

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
			player.GetDamage(DamageClass.Generic) += AllDamage;
			player.GetDamage(DamageClass.Melee) += MeleeDamage;
			player.GetDamage(DamageClass.Ranged) += RangedDamage;
			player.GetDamage(DamageClass.Magic) += MagicDamage;
			player.GetCritChance(DamageClass.Generic) += AllCrit;
			player.GetCritChance(DamageClass.Melee) += MeleeCrit;
			player.GetCritChance(DamageClass.Ranged) += RangedCrit;
			player.GetCritChance(DamageClass.Magic) += MagicCrit;
			player.GetAttackSpeed(DamageClass.Melee) += MeleeSpeed;
			player.moveSpeed += MovementSpeed;
			player.maxRunSpeed += MovementSpeed;
			player.lifeRegen += LifeRegen;
			player.GetArmorPenetration(DamageClass.Generic) += ArmorPenetration;
		}

		public override bool CanEquipAccessory(Player player, int slot, bool modded)
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
