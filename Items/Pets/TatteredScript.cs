using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Pets
{
	public class TatteredScript : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tattered Script");
			Tooltip.SetDefault("Summons an Unbound Mask to follow the player!");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.ZephyrFish);
			item.shoot = mod.ProjectileType("CaptiveMaskPet");
			item.buffType = mod.BuffType("CaptiveMaskPetBuff");
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