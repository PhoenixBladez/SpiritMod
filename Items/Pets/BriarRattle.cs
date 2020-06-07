using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Pets
{
	public class BriarRattle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thorny Rattle");
			Tooltip.SetDefault("Summons a glowing Briarthorn Slime pet");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Fish);
			item.shoot = ModContent.ProjectileType<BriarSlimePet>();
			item.buffType = ModContent.BuffType<BriarSlimeBuff>();
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