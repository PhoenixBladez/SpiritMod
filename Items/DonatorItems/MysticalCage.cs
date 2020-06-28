using SpiritMod.Buffs.Pet;
using SpiritMod.Projectiles.Pet;
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
			Tooltip.SetDefault("Summons a Cultfish to follow you\nReveals nearby treasure");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Fish);
			item.value = 25000;
			item.rare = ItemRarityID.Orange;
			item.shoot = ModContent.ProjectileType<Caltfist>();
			item.buffType = ModContent.BuffType<CaltfistPetBuff>();
		}

		public override void UseStyle(Player player)
		{
			if(player.whoAmI == Main.myPlayer && player.itemTime == 0) {
				player.AddBuff(item.buffType, 3600, true);
			}
		}

		public override bool CanUseItem(Player player)
		{
			return player.miscEquips[0].IsAir;
		}


	}
}