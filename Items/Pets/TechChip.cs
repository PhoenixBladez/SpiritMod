using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Pets
{
	public class TechChip : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stardrive Chip");
			Tooltip.SetDefault("It's inscribed in an Astral language\nSummons a Star Spider to run alongside you");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Fish);
			item.shoot = mod.ProjectileType("CogSpiderPet");
			item.buffType = mod.BuffType("CogSpiderPetBuff");
			item.UseSound = SoundID.Item93;
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