using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
	public class MysticalCage : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mystical Cage");
			Tooltip.SetDefault("Summons a Cultfish to follow you\nReveals nearby treasure\n~Donator Item~");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Fish);
			item.value = 25000;
			item.rare = 3;
			item.shoot = mod.ProjectileType("Caltfist");
			item.buffType = mod.BuffType("CaltfistPetBuff");
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(item.buffType, 3600, true);
			}
		}

		public override bool CanUseItem(Player player)
		{
			return player.miscEquips[0].IsAir;
		}

		
	}
}