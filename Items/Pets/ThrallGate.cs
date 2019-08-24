using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Pets
{
	public class ThrallGate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thrall's Gate");
			Tooltip.SetDefault("~Donator Item~\n'Leads to a Dragon's Domain'");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Fish);
			item.shoot = mod.ProjectileType("ThrallPet");
			item.buffType = mod.BuffType("ThrallBuff");
			item.UseSound = SoundID.Item8;
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