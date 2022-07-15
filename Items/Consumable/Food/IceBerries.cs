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
			Item.width = Item.height = 22;
			Item.rare = ItemRarityID.Green;
			Item.maxStack = 99;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTime = Item.useAnimation = 30;

			Item.noMelee = true;
			Item.consumable = true;
			Item.UseSound = SoundID.Item2;
			Item.autoReuse = false;
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
