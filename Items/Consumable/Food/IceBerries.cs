using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Buffs;
using Terraria;
using SpiritMod.NPCs.AuroraStag;
using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Consumable.Food
{
	public class IceBerries : FoodItem
	{
		internal override Point Size => new(30, 42);

		public override void StaticDefaults()
		{
			DisplayName.SetDefault("Ice Berry");
			Tooltip.SetDefault("Grants immunity to being on fire\nPerhaps some mystical creature would like this?");
		}

		public override bool? UseItem(Player player)
		{
			if (player.whoAmI != Main.myPlayer)
				return false;

			MyPlayer myPlayer = player.GetModPlayer<MyPlayer>();
			AuroraStag auroraStag = myPlayer.hoveredStag;

			if (auroraStag != null && !auroraStag.Scared && !auroraStag.NPC.immortal && auroraStag.TameAnimationTimer == 0) {
				auroraStag.TameAnimationTimer = AuroraStag.TameAnimationLength;
				myPlayer.hoveredStag = null;

				if (Main.netMode == NetmodeID.MultiplayerClient)
					SpiritMod.WriteToPacket(SpiritMod.Instance.GetPacket(4), (byte)MessageType.TameAuroraStag, auroraStag.NPC.whoAmI).Send();
			}
			else
				player.AddBuff(ModContent.BuffType<IceBerryBuff>(), 19600);

			return true;
		}
	}
}
