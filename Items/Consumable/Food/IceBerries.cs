using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Buffs;
using Terraria;
using SpiritMod.NPCs.AuroraStag;

namespace SpiritMod.Items.Consumable.Food
{
	public class IceBerries : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Berry");
			Tooltip.SetDefault("Grants immunity to being on fire\nPerhaps some mystical creature would like this?");
		}

		public override void SetDefaults()
		{
			item.width = item.height = 22;
			item.rare = 2;
			item.maxStack = 99;
			item.noUseGraphic = true;
			item.useStyle = ItemUseStyleID.EatingUsing;
			item.useTime = item.useAnimation = 30;

			item.noMelee = true;
			item.consumable = true;
			item.UseSound = SoundID.Item2;
			item.autoReuse = false;
		}

		public override bool UseItem(Player player)
		{
			if (player.whoAmI != Main.myPlayer)
				return false;

			MyPlayer myPlayer = player.GetModPlayer<MyPlayer>();
			AuroraStag auroraStag = myPlayer.hoveredStag;

			if (auroraStag != null && !auroraStag.Scared && !auroraStag.npc.immortal && auroraStag.TameAnimationTimer == 0) {
				auroraStag.TameAnimationTimer = AuroraStag.TameAnimationLength;
				myPlayer.hoveredStag = null;

				if (Main.netMode == NetmodeID.MultiplayerClient)
					SpiritMod.WriteToPacket(SpiritMod.instance.GetPacket(4), (byte)MessageType.TameAuroraStag, auroraStag.npc.whoAmI).Send();
			}
			else
				player.AddBuff(ModContent.BuffType<IceBerryBuff>(), 19600);

			return true;
		}
	}
}
